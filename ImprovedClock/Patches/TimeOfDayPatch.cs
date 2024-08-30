using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using MonoMod.Utils;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace ImprovedClock.Patches;

[HarmonyPatch(typeof(TimeOfDay))]
public static class TimeOfDayPatch {
    [HarmonyPatch(nameof(TimeOfDay.MoveTimeOfDay))]
    [HarmonyPrefix]
    private static void SetHudTimeRefresh() {
        if (!ConfigManager.enableRealtimeClock.Value) return;

        TimeOfDay.Instance.changeHUDTimeInterval = 4F;
    }

    [HarmonyPatch(nameof(TimeOfDay.SetShipLeaveEarlyClientRpc))]
    [HarmonyPrefix]
    private static void SetShipLeaveIconVisibility() => ShipLeaving();

    [HarmonyPatch(nameof(TimeOfDay.TimeOfDayEvents))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ListenForShipLeave(IEnumerable<CodeInstruction> instructions) {
        var originalInstructions = instructions.ToList();

        try {
            List<CodeInstruction> codeInstructions = [
                ..originalInstructions,
            ];

            for (var index = 0; index < codeInstructions.Count; index++) {
                var codeInstruction = codeInstructions[index];

                /*
                   IL_0051: ldarg.0      // this
                   IL_0052: ldc.i4.1
                   IL_0053: stfld        bool TimeOfDay::shipLeavingAlertCalled
                 */

                if (codeInstruction.opcode != OpCodes.Stfld) continue;

                if (codeInstruction.operand is not FieldInfo fieldInfo) continue;

                if (fieldInfo.FieldType != typeof(bool)) continue;

                if (!fieldInfo.Name.Contains("shipLeavingAlertCalled")) continue;

                codeInstructions.Insert(index + 1, new(OpCodes.Call, AccessTools.Method(typeof(TimeOfDayPatch), nameof(ShipLeaving))));

                ImprovedClock.Logger.LogInfo("Found ship leaving alert instruction!");
                break;
            }

            return codeInstructions;
        } catch (Exception exception) {
            exception.LogDetailed();

            return originalInstructions;
        }
    }

    public static void ShipLeaving() => ImprovedClock.spectatorClock?.SetShipLeaveIconVisible(true);
}