using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DieInTheDungeonSandbox.Core;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.Utility;
using static DieInTheDungeonSandbox.Core.UIUtil;
using static UnityEngine.UI.InputField;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    internal class DropdownWidget<T> : Widget
    {
        private int selectedIndex = 0;
        private readonly Dropdown dropdown;

        public DropdownWidget(GameObject parent, string name, T[] options, Func<T, string> displayString, Action<T> onAction)
        {
            GameObject container = UIUtil.CreateSimpleHorizontalLayout(parent, $"{name}-horizontal");

            var stringOptions = options.Select(displayString).ToArray();
            UIFactory.CreateDropdown(container, $"{name}-dropdown", out dropdown, stringOptions[0], 12, (i) => { selectedIndex = i; }, defaultOptions: stringOptions);
            UIFactory.SetLayoutElement(dropdown.gameObject, minWidth: WIDTH_LONG, minHeight: ROW_HEIGHT);

            var applyButton = UIUtil.CreateButton(container, name);
            applyButton.OnClick = () => onAction(options[selectedIndex]);

            _widgetRoot = container;
        }

    }
}
