using System;
using System.Collections.Generic;
using System.Text;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using UnityEngine;
using UnityEngine.UI;
using static DieInTheDungeonSandbox.Core.UIUtil;
using System.ComponentModel;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    internal class ToggleWidget : Widget
    {
        private ButtonRef button;

        public ToggleWidget(GameObject parent, string name, Action<bool> onToggle)
        {
            bool toggle = false;
            button = CreateButton(parent, name);
            var baseColor = button.Component.image.color;

            button.OnClick = () =>
            {
                toggle ^= true;
                button.Component.image.color = toggle ? Color.green : baseColor;
                onToggle.Invoke(toggle);
            };
            _widgetRoot = button.GameObject;
        }

        protected override void UpdateEditable()
        {
            SetEnabled(button, Enabled);
        }
    }
}
