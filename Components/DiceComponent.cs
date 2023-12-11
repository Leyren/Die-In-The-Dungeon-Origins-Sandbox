using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonOriginsSandbox.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniverseLib.UI;
using DieInTheDungeonOriginsSandbox.Core;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class DiceComponent : PluginComponent
    {
        public DiceComponent(GameObject panelRoot) : base(panelRoot)
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            UIFactory.CreatePanel("Dice-component", _panelRoot, out var contentholder);
            UIFactory.CreateLabel(contentholder, "dice-component-title", "Change Dices", fontSize: 18);
            PluginUI.CreateButton(contentholder, "Dice Upgrade Menu", onClick: CheatActions.OpenDiceUpgradeMenu);
            PluginUI.CreateButton(contentholder, "Upgrade All Dices", onClick: CheatActions.UpgradeAllDices);
            PluginUI.CreateButton(contentholder, "Get Random Dice", onClick: CheatActions.GetRandomDice);
            new DropdownWidget<DiceData>(contentholder, "Add Dice", CheatActions.GetAllDices().ToArray(), d => d.ToString(), CheatActions.GetSelectedDice);
        }
    }
}
