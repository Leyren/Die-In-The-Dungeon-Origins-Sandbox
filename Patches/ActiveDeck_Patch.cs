using DieInTheDungeonOriginsSandbox;
using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonSandbox.Core;
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
        static void MaxDiceInHand_Postfix(ref bool __result)
        {
            if (PatchData.UnlimitedDeck) __result = false;
        }
    }
}
