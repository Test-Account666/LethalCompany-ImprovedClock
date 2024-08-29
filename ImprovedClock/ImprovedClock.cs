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


        ConfigManager.clockNumberColorRed.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockNumberColorGreen.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockNumberColorBlue.SettingChanged += (_, _) => SetClockColorAndSize();

        ConfigManager.clockBoxColorRed.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockBoxColorGreen.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockBoxColorBlue.SettingChanged += (_, _) => SetClockColorAndSize();

        ConfigManager.clockIconColorRed.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockIconColorGreen.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockIconColorBlue.SettingChanged += (_, _) => SetClockColorAndSize();

        ConfigManager.clockShipLeaveIconColorRed.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockShipLeaveIconColorGreen.SettingChanged += (_, _) => SetClockColorAndSize();
        ConfigManager.clockShipLeaveIconColorBlue.SettingChanged += (_, _) => SetClockColorAndSize();

        ConfigManager.clockSizeMultiplier.SettingChanged += (_, _) => SetClockColorAndSize();

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

    public static void SetClockColorAndSize() {
        if (HUDManager.Instance == null) return;

        const float colorMultiplier = 1F / 255F;

        var clockBoxScale = new Vector3(-0.5893304F, 0.5893304F, 0.5893303F);

        clockBoxScale *= (ConfigManager.clockSizeMultiplier.Value / 100F);

        var numberColor = new Color(colorMultiplier * ConfigManager.clockNumberColorRed.Value,
                                    colorMultiplier * ConfigManager.clockNumberColorGreen.Value,
                                    colorMultiplier * ConfigManager.clockNumberColorBlue.Value, 1F);

        var boxColor = new Color(colorMultiplier * ConfigManager.clockBoxColorRed.Value,
                                 colorMultiplier * ConfigManager.clockBoxColorGreen.Value,
                                 colorMultiplier * ConfigManager.clockBoxColorBlue.Value, 1F);

        var iconColor = new Color(colorMultiplier * ConfigManager.clockIconColorRed.Value,
                                  colorMultiplier * ConfigManager.clockIconColorGreen.Value,
                                  colorMultiplier * ConfigManager.clockIconColorBlue.Value, 1F);

        var shipLeaveIconColor = new Color(colorMultiplier * ConfigManager.clockShipLeaveIconColorRed.Value,
                                           colorMultiplier * ConfigManager.clockShipLeaveIconColorGreen.Value,
                                           colorMultiplier * ConfigManager.clockShipLeaveIconColorBlue.Value, 1F);

        var clockNumber = HUDManager.Instance.clockNumber;

        clockNumber.color = numberColor;


        var clockBoxObject = clockNumber.transform.parent;

        var clockBoxImage = clockBoxObject.GetComponent<Image>();

        clockBoxImage.color = boxColor;
        clockBoxObject.transform.localScale = clockBoxScale;


        var clockImage = HUDManager.Instance.clockIcon;

        clockImage.color = iconColor;


        var shipLeaveIcon = HUDManager.Instance.shipLeavingEarlyIcon;

        shipLeaveIcon.color = shipLeaveIconColor;


        if (spectatorClock == null || !spectatorClock) return;

        spectatorClock.clockNumber.color = numberColor;

        spectatorClock.clockIcon.color = iconColor;


        spectatorClock.clockBox.color = boxColor;
        spectatorClock.clockBox.transform.localScale = clockBoxScale;

        spectatorClock.shipLeaveIcon.color = shipLeaveIconColor;
    }
}