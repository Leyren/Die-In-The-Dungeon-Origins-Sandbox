using MEC;
using System;
using System.Collections.Generic;
using System.Text;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using UnityEngine;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonOriginsSandbox.Components;
using System.ComponentModel;
using UnityEngine.UI;
using static DeckData;

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
        public Action OnPanelOpened;

        public override void SetActive(bool value)
        {
            bool valueChange = this.Enabled ^ value;
            base.SetActive(value);

            if (!valueChange) return;

            // Note: UIModel has OnToggleEnabled, but that is often not called since changes via SetActive don't trigger it. 
            if (!value) OnPanelClosed?.Invoke();
            if (value) OnPanelOpened?.Invoke();
        }

        protected override void ConstructPanelContent()
        {
           GameObject scrollObj = UIFactory.CreateScrollView(ContentRoot, "root-scroll-view", out var container, out var Scrollbar, color: PluginUI.BACKGROUND_COLOR);

            UIFactory.SetLayoutElement(scrollObj, minHeight: 250, preferredHeight: 300, flexibleHeight: 9999, flexibleWidth: 9999);
            UIFactory.SetLayoutGroup<VerticalLayoutGroup>(container, spacing: 5, padTop: 5, padBottom: 5, padLeft: 5, padRight: 5);

            components.Add(new StatModifierComponent(container));
            components.Add(new CombatModificationComponent(container));
            components.Add(new RelicSelectorComponent(container));
            components.Add(new DiceComponent(container));
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
