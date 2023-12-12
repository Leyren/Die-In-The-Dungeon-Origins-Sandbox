using MEC;
using System;
using System.Collections.Generic;
using System.Text;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using UnityEngine;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonOriginsSandbox.Components;

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

        private readonly List<PluginComponent> components = [];

        public Action OnPanelClosed;

        protected override void OnClosePanelClicked()
        {
            base.OnClosePanelClicked();
            // Note: UIModel has OnToggleEnabled, but that is often not called since changes via SetActive don't trigger it. 
            OnPanelClosed.Invoke();
        }

        protected override void ConstructPanelContent()
        {
            components.Add(new StatModifierComponent(ContentRoot));
            components.Add(new CombatTogglesComponent(ContentRoot));
            components.Add(new RelicSelectorComponent(ContentRoot));
            components.Add(new DiceComponent(ContentRoot));
        }

        override public void Update()
        {
            foreach (var component in components)
            {
                component.Update();
            }
        }
    }
}
