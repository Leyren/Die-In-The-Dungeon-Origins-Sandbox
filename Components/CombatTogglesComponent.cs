using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniverseLib.UI;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class CombatTogglesComponent : PluginComponent
    {
        public CombatTogglesComponent(GameObject panelRoot) : base(panelRoot)
        {
            InitializeUI();
        }
        private void InitializeUI()
        {
            GameObject container = UIFactory.CreateHorizontalGroup(_panelRoot, $"combat-toggles-horizontal", false, false, true, true, spacing: 5, padding: new Vector4(5, 10, 5, 10));
            new ToggleWidget(container, "Invulnerable", (v) => PatchData.Invulnerable = v);
            new ToggleWidget(container, "Force Kill", (v) => PatchData.ForceKill = v);
        }
    }
}
