using System;
using System.Collections.Generic;
using System.Text;

namespace DieInTheDungeonOriginsSandbox
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
    }
}
