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
        public DiceComponent(GameObject panelRoot) : base(panelRoot)
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            UIFactory.CreatePanel("Dice-component", _panelRoot, out var contentHolder);
            var layout = contentHolder.GetComponent<VerticalLayoutGroup>();
            layout.childControlHeight = false;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            UIFactory.CreateLabel(contentHolder, "dice-component-title", "Change Dices", fontSize: 18);
            PluginUI.CreateButton(contentHolder, "Dice Upgrade Menu", onClick: CheatActions.OpenDiceUpgradeMenu);
            PluginUI.CreateButton(contentHolder, "Upgrade All Dices", onClick: CheatActions.UpgradeAllDices);
            PluginUI.CreateButton(contentHolder, "Get Random Dice", onClick: CheatActions.GetRandomDice);
            new DropdownWidget<DiceData>(contentHolder, "Add Dice", CheatActions.GetAllDices().ToArray(), d => d.ToString(), CheatActions.GetSelectedDice);
        }
    }
}
