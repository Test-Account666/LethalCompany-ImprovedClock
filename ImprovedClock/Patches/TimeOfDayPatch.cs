using HarmonyLib;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
public class TimeOfDayPatch {
    [HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]
    [HarmonyPrefix]
    private static void SetHudTimeRefresh() {
        if (!ConfigManager.enableRealtimeClock.Value) return;

        TimeOfDay.Instance.changeHUDTimeInterval = 4F;
    }
}