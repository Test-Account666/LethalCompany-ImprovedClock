using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
public static class PlayerControllerPatch {
    [HarmonyPatch(nameof(PlayerControllerB.KillPlayer))]
    [HarmonyPrefix]
    // ReSharper disable once InconsistentNaming
    private static void EnableSpectatorClock(PlayerControllerB __instance) {
        if (!ConfigManager.showClockInSpectator.Value) return;

        var localPlayer = StartOfRound.Instance.localPlayerController;

        if (localPlayer != __instance) return;

        var canvasPanel = HUDManager.Instance.Clock.canvasGroup.transform.parent.parent;

        var spectatorClockObject = Object.Instantiate(ImprovedClock.spectatorClockPrefab, canvasPanel);

        spectatorClockObject.transform.SetSiblingIndex(1);

        ImprovedClock.spectatorClock = spectatorClockObject.GetComponent<SpectatorClock>();
    }
}