using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class CombatModificationComponent : PluginComponent
    {
        public CombatModificationComponent(GameObject parent) : base(parent, "Combat")
        {
            InitializeUI();
        }
        private void InitializeUI()
        {
            GameObject container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            new ToggleWidget(container, "Invulnerable", (v) => PatchData.Invulnerable = v);
            new ToggleWidget(container, "Force Kill", (v) => PatchData.ForceKill = v);

            container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            PluginUI.CreateButton(container, "Kill selected enemy", onClick: KillSelectedEnemy);
            PluginUI.CreateButton(container, "Kill all enemies", onClick: KillAllEnemies);
        }

        private void KillAllEnemies()
        {
            FloorSystem.Instance.battle.DebugKillAllEnemies();
        }
        private void KillSelectedEnemy()
        {
            FloorSystem.Instance.battle.DebugKillSelectedEnemy();
        }
    }
}
