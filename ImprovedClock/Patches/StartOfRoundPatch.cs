using HarmonyLib;
using UnityEngine;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(StartOfRound))]
public static class StartOfRoundPatch {
    [HarmonyPatch(nameof(StartOfRound.ReviveDeadPlayers))]
    [HarmonyPrefix]
    private static void DestroySpectatorClock() => Object.Destroy(ImprovedClock.spectatorClock?.gameObject);
}