using BepInEx.Configuration;
using DieInTheDungeonOriginsSandbox.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonSandbox.Core
{
    internal class PluginConfig
    {
        public static KeyCode Hotkey;
        public static bool ShowButton;

        public static void ReadConfig(ConfigFile Config)
        {
            Hotkey = Config.Bind<KeyCode>("General", "Hotkey", KeyCode.F6, "Hotkey to open / close the sandbox").Value;
            ShowButton = Config.Bind<bool>("General", "Show-Button", true, "Show the 'Open Sandbox' button in-game, as alternative to the Hotkey").Value;
            PatchData.UnlimitedDeck = Config.Bind<bool>("Game", "Unlimited-Deck", false, "Remove the size limit of your deck (Backpack might not be able to display all dices however)").Value;
        }
    }
}
