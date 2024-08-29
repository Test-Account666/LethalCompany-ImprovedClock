using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using MonoMod.Utils;
using UnityEngine;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(HUDManager))]
public static class HUDManagerPatch {
    [HarmonyPatch(nameof(HUDManager.OnEnable))]
    [HarmonyPostfix]
    private static void SetClockColor() => ImprovedClock.SetClockColorAndSize();

    [HarmonyPatch(nameof(HUDManager.SetClock))]
    [HarmonyPrefix]
    // ReSharper disable once InconsistentNaming
    // This method name is a joke btw. I don't have anything against anyone.
    private static bool SetNormalHumanBeingClock(float timeNormalized, float numberOfHours, bool createNewLine, ref string __result) {
        if (!ConfigManager.enableNormalHumanBeingClock.Value) {
            ImprovedClock.spectatorClock?.SetClock();
            return true;
        }

        var hudManager = HUDManager.Instance;

        var time = (int) (timeNormalized * (60.0 * numberOfHours)) + 360;
        var hours = (int) Mathf.Floor(time / 60F);
        hudManager.newLine = createNewLine? "\n" : " ";
        hudManager.amPM = "12:00" + hudManager.newLine + "AM";
        if (hours >= 24) {
            hudManager.clockNumber.text = "24:00 " + hudManager.newLine;
            __result = "24:00\n";

            ImprovedClock.spectatorClock?.SetClock();
            return false;
        }

        hudManager.amPM = hours >= 12? hudManager.newLine + "PM" : hudManager.newLine + "AM";
        var minutes = time % 60;
        var timeString = $"{hours:00}:{minutes:00}";
        hudManager.clockNumber.text = timeString;
        __result = timeString;

        ImprovedClock.spectatorClock?.SetClock();
        return false;
    }

    [HarmonyPatch(nameof(HUDManager.SetClockVisible))]
    [HarmonyPrefix]
    private static void ShowClockInShip(ref bool visible) {
        if (visible) return;

        var localPlayer = StartOfRound.Instance.localPlayerController;

        if (!ConfigManager.showClockInShip.Value && localPlayer.isInHangarShipRoom) return;

        if (!ConfigManager.showClockInFacility.Value && localPlayer.isInsideFactory) return;

        visible = true;
    }

    [HarmonyPatch(nameof(HUDManager.SetClockVisible))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ModifyTargetAlpha(IEnumerable<CodeInstruction> instructions) {
        var originalInstructions = instructions.ToList();

        try {
            List<CodeInstruction> codeInstructions = [
                ..originalInstructions,
            ];

            for (var index = 0; index < codeInstructions.Count; index++) {
                var instruction = codeInstructions[index];

                if (instruction.opcode != OpCodes.Ldc_R4) continue;

                ImprovedClock.Logger.LogInfo("Successfully found correct instruction!");

                codeInstructions[index] = new(OpCodes.Call, AccessTools.Method(typeof(HUDManagerPatch), nameof(GetTargetAlpha)));
                codeInstructions.Insert(index + 1, new(OpCodes.Stloc_0));
                codeInstructions.Insert(index + 1, new(OpCodes.Ldloc_0));
                break;
            }

            return codeInstructions;
        } catch (Exception exception) {
            exception.LogDetailed();

            return originalInstructions;
        }
    }

    public static float GetTargetAlpha() {
        var playerControllerB = StartOfRound.Instance.localPlayerController;

        if (playerControllerB.isInsideFactory) return ConfigManager.clockVisibilityInFacility.Value / 100F;

        return playerControllerB.isInHangarShipRoom? ConfigManager.clockVisibilityInShip.Value / 100F : 1F;
    }

    [HarmonyPatch(nameof(HUDManager.RemoveSpectateUI))]
    [HarmonyPostfix]
    private static void DisableSpectatorClock() {
        if (ImprovedClock.spectatorClock == null) {
            ImprovedClock.Logger.LogError("Couldn't find SpectatorClock!");
            return;
        }

        ImprovedClock.spectatorClock.SetClockVisible(false);
        ImprovedClock.spectatorClock?.SetShipLeaveIconVisible(false);
    }
}