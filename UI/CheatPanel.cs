using MEC;
using System;
using System.Collections.Generic;
using System.Text;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using UnityEngine;
using DieInTheDungeonOriginsSandbox.UI.Widgets;

namespace DieInTheDungeonOriginsSandbox.UI
{
    public class CheatPanel : UniverseLib.UI.Panels.PanelBase
    {
        public CheatPanel(UIBase owner) : base(owner) { }

        public override string Name => "Sandbox";
        public override int MinWidth => 100;
        public override int MinHeight => 200;
        public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
        public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
        public override bool CanDragAndResize => true;

        private readonly List<Widget> widgets = new();

        protected override void ConstructPanelContent()
        {

            widgets.Add(new ModificationWidget<int>("max-dice", ContentRoot, "Modify Max. Dice in Hand by", 0,
                applyModification: CheatActions.ModifyMaxDiceInHandBy,
                retrieveData: () => CheatActions.GetMaxDiceInHand().ToString(),
                validateInput: PluginUtil.NonNegativeInput)
                .EnabledIf(PluginUtil.IsGamePlaying)
                );

            widgets.Add(new ModificationWidget<int>("max-health", ContentRoot, "Modify Max Health by", 0,
                applyModification: CheatActions.ModifyMaxHealthBy,
                retrieveData: () => CheatActions.GetMaxHealth().ToString(),
                validateInput: PluginUtil.NonNegativeInput)
                .EnabledIf(PluginUtil.IsGamePlaying)
               );

            widgets.Add(new ToggleWidget(ContentRoot, "Invulnerable", (v) => Data.Invulnerable = v));
            widgets.Add(new ToggleWidget(ContentRoot, "Force Kill", (v) => Data.ForceKill = v));
            AddButton("Dice Upgrade Menu", CheatActions.OpenDiceUpgradeMenu);
            AddButton("Upgrade All Dices", CheatActions.UpgradeAllDices);
        }

        override public void Update()
        {
            foreach (var binding in widgets)
            {
                binding.Update();
            }
        }

        private void AddButton(string name, Action onClick)
        {
            ButtonRef b = UIFactory.CreateButton(ContentRoot, name, name);
            UIFactory.SetLayoutElement(b.GameObject, minWidth: 200, minHeight: 25);
            b.OnClick = onClick;
        }



    }
}
