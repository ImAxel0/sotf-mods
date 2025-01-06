using Il2CppInterop.Runtime;
using RedLoader.Utils;
using RedLoader;
using UnityEngine;
using SonsSdk;

namespace PlaneMod;

public class AssetLoader
{
    private static List<AssetBundle> _bundles = new();
    public static List<GameObject> GameObjects = new();

    public static void LoadAllBundles()
    {
        RLog.Msg("[+] Loading bundles...");

        string modsDirectory = LoaderEnvironment.ModsDirectory;
        string[] bundleFiles = Directory.GetFiles(Path.Combine(modsDirectory, @"PlaneMod\AssetBundle"));

        foreach (string bundleFile in bundleFiles)
        {
            RLog.Msg("Loading " + bundleFile);
            _bundles.Add(AssetBundle.LoadFromFile(bundleFile));
        }
    }

    public static void LoadAllAssets()
    {
        RLog.Msg("[+] Loading assets...");

        int counter = 0;
        foreach (AssetBundle bundle in _bundles)
        {
            string[] assetPath = bundle.GetAllAssetNames();
            RLog.Msg("Loading " + assetPath.LastOrDefault() + "Asset");

            GameObject original = bundle.LoadAsset(assetPath.Last(), Il2CppType.Of<GameObject>()).Cast<GameObject>();
            RLog.Msg("GameObject name: " + original.name);

            GameObjects.Add(UnityEngine.Object.Instantiate(original));
            LoadShaders(GameObjects.ElementAt(counter));
            counter++;
        }
    }

    private static void LoadShaders(GameObject gameObject)
    {
        MeshRenderer goMeshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (goMeshRenderer != null)
        {
            foreach (var material in goMeshRenderer.materials)
            {
                if (material.shader.isSupported)
                    material.shader = Shader.Find("Sons/HDRPLit");
            }
        }

        for (int j = 0; j < gameObject.transform.childCount; j++)
        {
            MeshRenderer meshRenderer = gameObject.transform.GetChild(j).GetComponent<MeshRenderer>();
            if (meshRenderer == null) { continue; }

            foreach (var material in meshRenderer.materials)
            {
                if (material.shader.isSupported)
                    material.shader = Shader.Find("Sons/HDRPLit");
            }
        }
    }

}

