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
            GameObject container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            PluginUI.CreateButton(container, "Dice Upgrade Menu", onClick: CheatActions.OpenDiceUpgradeMenu);
            PluginUI.CreateButton(container, "Upgrade All Dices", onClick: CheatActions.UpgradeAllDices);

            container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            PluginUI.CreateButton(container, "Get Random Dice", onClick: CheatActions.GetRandomDice);
            new DropdownWidget<DiceData>(_panelRoot, "Add Dice", CheatActions.GetAllDices().ToArray(), d => d.ToString(), CheatActions.GetSelectedDice);
        }
    }
}
