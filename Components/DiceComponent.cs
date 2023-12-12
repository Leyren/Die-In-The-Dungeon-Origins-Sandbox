using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonOriginsSandbox.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniverseLib.UI;
using DieInTheDungeonOriginsSandbox.Core;
using UnityEngine.UI;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class DiceComponent : PluginComponent
    {
        public DiceComponent(GameObject parent) : base(parent, "Dices")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            PluginUI.CreateButton(_panelRoot, "Dice Upgrade Menu", onClick: CheatActions.OpenDiceUpgradeMenu);
            PluginUI.CreateButton(_panelRoot, "Upgrade All Dices", onClick: CheatActions.UpgradeAllDices);
            PluginUI.CreateButton(_panelRoot, "Get Random Dice", onClick: CheatActions.GetRandomDice);
            new DropdownWidget<DiceData>(_panelRoot, "Add Dice", CheatActions.GetAllDices().ToArray(), d => d.ToString(), CheatActions.GetSelectedDice);
        }
    }
}
