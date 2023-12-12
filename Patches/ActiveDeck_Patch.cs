using DieInTheDungeonOriginsSandbox;
using DieInTheDungeonOriginsSandbox.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DieInTheDungeonSandbox.Patches { 

    [HarmonyPatch(typeof(ActiveDeck))]
    internal class ActiveDeck_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ActiveDeck.IsFull))]
        static void MaxDiceInHand_Postfix(ActiveDeck __instance, ref bool __result)
        {
            Plugin.Log.LogInfo("Current dices: " + __instance.dice.Count);
            __result = false;
        }
    }
}
