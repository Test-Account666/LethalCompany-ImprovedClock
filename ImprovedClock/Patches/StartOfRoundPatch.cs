using HarmonyLib;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(StartOfRound))]
public static class StartOfRoundPatch {
    [HarmonyPatch(nameof(StartOfRound.EndOfGame), MethodType.Enumerator)]
    [HarmonyPrefix]
    private static void DisableSpectatorClock() {
        if (ImprovedClock.spectatorClock == null) {
            ImprovedClock.Logger.LogError("Couldn't find SpectatorClock!");
            return;
        }

        ImprovedClock.spectatorClock.SetClockVisible(false);
    }
}