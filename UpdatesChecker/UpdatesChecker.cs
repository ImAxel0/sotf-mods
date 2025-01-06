using Alt.Json;
using HarmonyLib;
using RedLoader;
using SonsGameManager;
using SonsSdk;
using System.Collections;

namespace UpdatesChecker;

public class UpdatesChecker : SonsMod
{
    private static bool _hasFetched;
    private static bool _isSdkOK;
    private static List<Mod> _fetchedMods = new List<Mod>();
    private static int _updatesCount;
    private static string _content;

    protected override void OnInitializeMod()
    {
        RunCheck().RunCoro();
    }

    protected override void OnSdkInitialized()
    {
        _isSdkOK = true;
        HarmonyInstance.Patch(AccessTools.Method(typeof(ModSettingsUi), nameof(ModSettingsUi.Close)), 
            null, new HarmonyMethod(typeof(UpdatesChecker), nameof(OnModUiClose)));
        UpdatesCheckerUi.CreateTemp();
    }

    private static void OnModUiClose()
    {
        TooltipProvider.RaycastFor(UpdatesCheckerUi.Raycaster);
    }

    protected override void OnSonsSceneInitialized(ESonsScene sonsScene)
    {
        bool isTitle = sonsScene == ESonsScene.Title;
        if (_isSdkOK)
        {
            SUI.SUI.TogglePanel(UpdatesCheckerUi.MOD_UPDATESTEMP_ID, false);
            if (isTitle)
            {
                TooltipProvider.RaycastFor(UpdatesCheckerUi.Raycaster);
            }          
        }
        SUI.SUI.TogglePanel(UpdatesCheckerUi.MOD_UPDATES_ID, isTitle);
    }

    private static IEnumerator RunCheck()
    {
        CheckForUpdates();
        while (!_hasFetched || !_isSdkOK)
        {
            yield return null;  
        }
        SUI.SUI.TogglePanel(UpdatesCheckerUi.MOD_UPDATESTEMP_ID, false);
        UpdatesCheckerUi.Create(_updatesCount, _content);
    }

    private async static Task<Root> FetchModsOfPage(int page)
    {
        string url = $"https://api.sotf-mods.com/api/mods?&page={page}";
        using HttpClient client = new HttpClient();

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Root>(responseBody);
            return data;
        }
        catch (HttpRequestException e)
        {
            RLog.Error($"Request error: {e.Message}");
        }

        return null;
    }

    private async static void CheckForUpdates()
    {
        try
        {
            var res = await FetchModsOfPage(1);
            if (res == null)
            {
                RLog.Warning("Couldn't check mod updates");
                return;
            }
            var meta = res.Meta;
            var mods = res.Mods;

            if (meta.Pages > 1)
            {
                for (int i = 2; i <= meta.Pages; i++)
                {
                    try
                    {
                        var res2 = await FetchModsOfPage(i);
                        mods.AddRange(res2.Mods);
                    }
                    catch (Exception e)
                    {
                        RLog.Error($"Request error: {e.Message}");
                    }
                }
            }

            _fetchedMods = mods;

            foreach (var mod in RegisteredMods)
            {
                var fetchedMod = _fetchedMods.Find(fmod => fmod.ModId == mod.ID);
                if (fetchedMod != null) // only check for uploaded mods
                {
                    var installedVersion = new Version(mod.Manifest.Version);
                    var onlineVersion = new Version(fetchedMod.LatestVersion);
                    var result = installedVersion.CompareTo(onlineVersion);

                    if (result < 0) // installed is older than online
                    {
                        _updatesCount++;     
                        _content += $"<color=#ffffff>{fetchedMod.Name}</color> {installedVersion} -> {onlineVersion} ";
                    }
                }
            }
        }
        catch (HttpRequestException e)
        {
            RLog.Error($"Request error: {e.Message}");
        }

        _hasFetched = true;
    }
}