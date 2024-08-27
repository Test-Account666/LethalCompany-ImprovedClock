using System.Collections;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
public static class PlayerControllerPatch {
    [HarmonyPatch(nameof(PlayerControllerB.Start))]
    [HarmonyPostfix]
    // ReSharper disable once InconsistentNaming
    private static void CreateSpectatorClock(PlayerControllerB __instance) => __instance.StartCoroutine(WaitAndCreateSpectatorClock(__instance));

    [HarmonyPatch(nameof(PlayerControllerB.KillPlayer))]
    [HarmonyPrefix]
    // ReSharper disable once InconsistentNaming
    private static void EnableSpectatorClock(PlayerControllerB __instance) {
        if (!ConfigManager.showClockInSpectator.Value) return;

        if (!StartOfRound.Instance.currentLevel.planetHasTime) return;

        var localPlayer = StartOfRound.Instance.localPlayerController;

        if (localPlayer != __instance) return;

        if (ImprovedClock.spectatorClock == null) {
            ImprovedClock.Logger.LogError("Couldn't find SpectatorClock!");
            return;
        }

        ImprovedClock.spectatorClock.SetClockVisible(true);
    }

    public static IEnumerator WaitAndCreateSpectatorClock(PlayerControllerB playerControllerB) {
        yield return new WaitUntil(() => StartOfRound.Instance != null && StartOfRound.Instance);

        var localPlayer = StartOfRound.Instance.localPlayerController;

        yield return new WaitUntil(() => localPlayer != null && localPlayer);

        if (localPlayer != playerControllerB) {
            ImprovedClock.Logger.LogDebug("Not local player!");
            yield break;
        }

        yield return new WaitUntil(() => HUDManager.Instance != null && HUDManager.Instance);

        yield return new WaitUntil(() => HUDManager.Instance.Clock != null);

        var canvasPanel = HUDManager.Instance.Clock.canvasGroup.transform.parent.parent;

        var spectatorClockObject = Object.Instantiate(ImprovedClock.spectatorClockPrefab, canvasPanel);

        spectatorClockObject.transform.SetSiblingIndex(1);

        ImprovedClock.spectatorClock = spectatorClockObject.GetComponent<SpectatorClock>();

        Object.DontDestroyOnLoad(ImprovedClock.spectatorClock);

        ImprovedClock.Logger.LogDebug("Clock created!");

        ImprovedClock.spectatorClock.SetClockVisible(false);
    }
}