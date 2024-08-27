using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ImprovedClock.Dependencies;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace ImprovedClock;

[BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class ImprovedClock : BaseUnityPlugin {
    public static ImprovedClock Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    public static SpectatorClock? spectatorClock;

    public static GameObject spectatorClockPrefab = null!;

    internal static void Patch() {
        Harmony ??= new(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        Harmony.PatchAll();

        Logger.LogDebug("Finished patching!");
    }

    private void Awake() {
        Logger = base.Logger;
        Instance = this;

        if (DependencyChecker.IsLobbyCompatibilityInstalled()) {
            Logger.LogInfo("Found LobbyCompatibility Mod, initializing support :)");
            LobbyCompatibilitySupport.Initialize();
        }

        ConfigManager.Initialize(Config);

        LoadAssetBundle();

        Patch();


        ConfigManager.clockColorRed.SettingChanged += (_, _) => SetClockColor();
        ConfigManager.clockColorGreen.SettingChanged += (_, _) => SetClockColor();
        ConfigManager.clockColorBlue.SettingChanged += (_, _) => SetClockColor();

        ConfigManager.showClockInSpectator.SettingChanged += (_, _) => spectatorClock?.SetClockVisible(ConfigManager.showClockInSpectator.Value);
        ConfigManager.clockVisibilityInSpectator.SettingChanged += (_, _) => spectatorClock?.SetClockVisibility();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    private static void LoadAssetBundle() {
        var assembly = Assembly.GetExecutingAssembly();

        var assemblyDirectory = Path.GetDirectoryName(assembly.Location);

        Debug.Assert(assemblyDirectory != null, nameof(assemblyDirectory) + " != null");
        var assetBundle = AssetBundle.LoadFromFile(Path.Combine(assemblyDirectory, "improvedclock"));

        spectatorClockPrefab = assetBundle.LoadAsset<GameObject>("Assets/LethalCompany/Mods/ImprovedClock/SpectatorClock.prefab");

        Object.DontDestroyOnLoad(spectatorClockPrefab);
    }

    internal static void Unpatch() {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }

    public static void SetClockColor() {
        var color = new Color(ConfigManager.clockColorRed.Value, ConfigManager.clockColorGreen.Value, ConfigManager.clockColorBlue.Value, 1F);

        var clockNumber = HUDManager.Instance.clockNumber;

        clockNumber.color = color;


        var clockBoxObject = clockNumber.transform.parent;

        var clockBoxImage = clockBoxObject.GetComponent<Image>();

        clockBoxImage.color = color;


        var clockImage = HUDManager.Instance.clockIcon;

        clockImage.color = color;


        if (spectatorClock == null || !spectatorClock) return;

        spectatorClock.clockNumber.color = color;
        spectatorClock.clockIcon.color = color;

        spectatorClock.clockBox.color = color;
    }
}