using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DieInTheDungeonOriginsSandbox.Patches
{
    [HarmonyPatch(typeof(DiceManager))]
    internal class DiceManager_Patch
    {

        [HarmonyPostfix]
        [HarmonyPatch(nameof(DiceManager.MaxDiceInHand))]
        static void MaxDiceInHand_Postfix(ref int __result)
        {
            __result += Data.MaxDiceInHandModifier;
            //Plugin.Log.LogDebug("DiceManager.MaxDiceInHand - Postfix " + __result);
        }
    }
}
