using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonOriginsSandbox.Core
{
    public static class PluginUtil
    {
        public static bool IsGamePlaying()
        {
            return GameManager.IsInitialized && GameManager.Instance.IsGamePlaying();
        }

        public static int NonNegativeInput(int v)
        {
            return v < 0 ? 0 : v;
        }

        public static Color Mix(this Color c, Color other, float percentage)
        {
            return other * percentage + (1 - percentage) * c;
        }
    }
}
