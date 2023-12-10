using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace DieInTheDungeonOriginsSandbox.Patches
{

    [HarmonyPatch(typeof(Entity))]
    internal class Player_Patch
    {
        /// <summary>
        /// Incoming damage
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Entity.Damage))]
        static void Damage_Prefix(Entity __instance, ref Hit hit)
        {
            if (__instance is Player && Data.Invulnerable)
            {
                Plugin.Log.LogInfo($"Negating incoming damage of {hit.DamageAmount} by {hit.Attacker}.");
                hit = new Hit(0);
            } else if (__instance is Enemy && Data.ForceKill)
            {
                Plugin.Log.LogInfo($"Modifying damage of {hit.DamageAmount} to force-kill.");
                hit = Hit.ForceKill;
            }
        }
    }

}
