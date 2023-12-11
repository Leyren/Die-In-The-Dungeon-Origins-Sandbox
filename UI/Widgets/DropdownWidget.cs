using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.Utility;
using static DieInTheDungeonOriginsSandbox.UI.PluginUI;
using static UnityEngine.UI.InputField;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    internal class DropdownWidget<T> : Widget
    {
        private int selectedIndex = 0;

        public DropdownWidget(GameObject parent, string name, T[] options, Func<T, string> displayString, Action<T> onAction)
        {
            GameObject container = UIFactory.CreateHorizontalGroup(parent, $"{name}-horizontal", false, false, true, true, spacing: 5, padding: new Vector4(5, 10, 5, 10));

            var stringOptions = options.Select(displayString).ToArray();
            UIFactory.CreateDropdown(container, $"{name}-dropdown", out var dropdown, stringOptions[0], 12, (i) => { selectedIndex = i; }, defaultOptions: stringOptions);
            UIFactory.SetLayoutElement(dropdown.gameObject, minWidth: WIDTH_LONG, minHeight: ROW_HEIGHT);

            var applyButton = PluginUI.CreateButton(container, $"{name}-button", name);
            applyButton.OnClick = () => onAction(options[selectedIndex]);
        }

    }
}
