using DieInTheDungeonOriginsSandbox.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using static EventOptionUI;

namespace DieInTheDungeonSandbox.Patches
{
    [HarmonyPatch(typeof(SlotsData))]
    internal class SlotsData_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(SlotsData.ExecuteRoll))]
        static bool ExecuteRoll(SlotsData __instance)
        {
            if (PatchData.AutoWinEventRolls) { 
            FieldInfo field = typeof(SlotsData).GetField("resultRoll", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(__instance, 999);
            __instance.minimumRoll = 0;
            return false;
        }
                return true;
            
        }
    }

    [HarmonyPatch(typeof(EventOptionUI))]
    internal class EventOptionUI_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EventOptionUI.CanClickOption))]
        static void CanClickOption_Postfix(EventOptionUI __instance, ref bool __result)
        {
            if (PatchData.AutoWinEventRolls)
            {
                // This method is also called for dialog options, in that case do nothing (so, only set it to true if 'should roll dice'
                __result = __instance.ShouldRollDice || __result;
            }
        }
    }
}
