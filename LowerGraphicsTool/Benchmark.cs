using TMPro;
using UnityEngine;
using SonsSdk;
using System.Collections;
using RedLoader;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TheForest;
using Endnight.Utilities;
using UnityEngine.InputSystem;
using Sons.Environment;
using RedLoader.Utils;
using SUI;
using Sons.Gui;

namespace LowerGraphicsTool;

public class Benchmark
{
    public static float CameraSpeed = 1.75f;
    public static float EachSceneTime = 10f;
    public static float DelayBetweenScene = 3f;

    public struct TimeBackup
    {
        public static int Hour;
        public static int Minutes;
    }

    public struct RecordedData
    {
        public static int MaxFps;
        public static int AverageFps;
        public static int MinFps;
    }

    public static void RunBenchmark()
    {
        LowerGraphicsToolUi.MainPanel.Active(false);
        if (Keyboard.current != null) InputSystem.DisableDevice(Keyboard.current);
        if (Mouse.current != null) InputSystem.DisableDevice(Mouse.current);
        if (Gamepad.current != null) InputSystem.DisableDevice(Gamepad.current);

        TimeBackup.Hour = TimeOfDayHolder.GetTimeOfDay().Hours;
        TimeBackup.Minutes = TimeOfDayHolder.GetTimeOfDay().Minutes;       
        DebugConsole.Instance._lockTimeOfDay("9");

        LowerGraphicsTool.FpsMeter.SetActive(true);
        var fpsText = LowerGraphicsTool.FpsMeter.transform.Find("Canvas/Panel/FpsReadout").GetComponent<TextMeshProUGUI>();

        Bench(fpsText).RunCoro();
    }

    private static IEnumerator Bench(TextMeshProUGUI fpsText)
    {
        List<string> fps = new();
        List<string> sanitizedFps = new();

        var sw = new Stopwatch();
        sw.Start();

        DebugConsole.Instance.SendCommand("freecamera on");
        var freeCam = GameObject.Find("FreeCamera");
        freeCam.GetComponent<FreeCameraController>().enabled = false;

        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // tend & knight scene
        freeCam.transform.position = new Vector3(-1042, 228, -613);
        freeCam.transform.localEulerAngles = new Vector3(0, 130, 0);
        yield return new WaitForSeconds(6);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
                freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }
        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // cross road scene
        freeCam.transform.position = new Vector3(1735, 48, -1202);
        freeCam.transform.localEulerAngles = new Vector3(359, 324, -0);
        yield return new WaitForSeconds(DelayBetweenScene);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
                freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }
        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // lake heavy scene
        freeCam.transform.position = new Vector3(-966, 117, -72);
        freeCam.transform.localEulerAngles = new Vector3(358.8f, 134.1f, 0);
        yield return new WaitForSeconds(DelayBetweenScene);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
            freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }
        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // snow house scene
        freeCam.transform.position = new Vector3(194, 526, -319);
        freeCam.transform.localEulerAngles = new Vector3(0.75f, 174, -0);
        yield return new WaitForSeconds(DelayBetweenScene);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
                freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }
        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // yacht scene
        freeCam.transform.position = new Vector3(448, 17, 1181);
        freeCam.transform.localEulerAngles = new Vector3(2, 258, 0);
        yield return new WaitForSeconds(DelayBetweenScene);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
                freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }
        LowerGraphicsToolUi.BlackScreenPanel.Active(true);

        // forest road scene
        freeCam.transform.position = new Vector3(526, 155, 1058);
        freeCam.transform.localEulerAngles = new Vector3(0.39f, 232, 0);
        yield return new WaitForSeconds(DelayBetweenScene);
        LowerGraphicsToolUi.BlackScreenPanel.Active(false);
        sw.Restart();
        while (sw.Elapsed.Seconds != EachSceneTime)
        {
            freeCam.transform.position = Vector3.MoveTowards(freeCam.transform.position,
                freeCam.transform.position + freeCam.transform.forward * 5,
                CameraSpeed * Time.deltaTime);

            fps.Add(fpsText.text);
            yield return null;
        }

        fps.RemoveAt(0);
        fps.ForEach(f =>
        {
            string noSpace = Regex.Replace(f, @"\s+", "");
            string sanitized = noSpace.Remove(noSpace.IndexOf("FPS"));
            sanitizedFps.Add(sanitized);
        });

        List<int> recordedFps = sanitizedFps
        .Select(s => { return int.TryParse(s, out int i) ? i : (int?)null; })
        .Where(i => i.HasValue)
        .Select(i => i.Value)
        .ToList();

        RecordedData.MaxFps = recordedFps.Max();
        RecordedData.AverageFps = (int)recordedFps.Average();
        RecordedData.MinFps = recordedFps.Min();
       
        LowerGraphicsToolUi.MaxFps.Value = $"Max: <color=#47B1E8>{recordedFps.Max()} FPS</color>";
        LowerGraphicsToolUi.AverageFps.Value = $"Average: <color=yellow>{recordedFps.Average():0} FPS</color>";
        LowerGraphicsToolUi.MinFps.Value = $"Min: <color=red>{recordedFps.Min()} FPS</color>";
        LowerGraphicsToolUi.BenchmarkResultsPanel.Active(true);

        if (Keyboard.current != null) InputSystem.EnableDevice(Keyboard.current);
        if (Mouse.current != null) InputSystem.EnableDevice(Mouse.current);
        if (Gamepad.current != null) InputSystem.EnableDevice(Gamepad.current);
        yield break;
    }

    public static void ExitBenchmark()
    {
        DebugConsole.Instance._setTimeOfDay($"{TimeBackup.Hour}:{TimeBackup.Minutes}");
        DebugConsole.Instance.SendCommand("freecamera off");
        LowerGraphicsToolUi.BenchmarkResultsPanel.Active(false);
        LowerGraphicsTool.FpsMeter.SetActive(LowerGraphicsTool.AlwaysShowFps);
    }

    public static void SaveBenchmark()
    {
        if (!Directory.Exists(Path.Combine(LoaderEnvironment.ModsDirectory, "LowerGraphicsTool/BenchmarkResults")))
        {
            Directory.CreateDirectory(Path.Combine(LoaderEnvironment.ModsDirectory, "LowerGraphicsTool/BenchmarkResults"));
        }
        var fileDir = Path.Combine(LoaderEnvironment.ModsDirectory, "LowerGraphicsTool/BenchmarkResults/Benchmarks.txt");
        string[] content =
        {
            $"{DateTime.Now:MM/dd/yyyy HH:mm}",
            $"Max FPS: {RecordedData.MaxFps}",
            $"Average FPS: {RecordedData.AverageFps}",
            $"Min FPS: {RecordedData.MinFps}",
            ""
        };
        File.AppendAllLines(fileDir, content);
        SonsTools.ShowMessageBox("Info", $"Benchmark result has been saved at {fileDir}");
    }
}
