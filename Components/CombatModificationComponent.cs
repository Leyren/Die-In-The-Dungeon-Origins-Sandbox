using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonSandbox.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UniverseLib.UI;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class CombatModificationComponent : PluginComponent
    {
        private readonly List<Widget> widgets = [];

        public CombatModificationComponent(GameObject parent) : base(parent, "Combat")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            GameObject container = UIUtil.CreateSimpleHorizontalLayout(_panelRoot);
            new ToggleWidget(container, "Invulnerable", (v) => PatchData.Invulnerable = v);
            new ToggleWidget(container, "Force Kill", (v) => PatchData.ForceKill = v);

            container = UIUtil.CreateSimpleHorizontalLayout(_panelRoot);
            UIUtil.CreateButton(container, "Kill selected enemy", onClick: KillSelectedEnemy);
            UIUtil.CreateButton(container, "Kill all enemies", onClick: KillAllEnemies);

            widgets.Add(new ModificationWidget<int>("skip-to-floor", _panelRoot, "Skip to Floor", 0,
                applyModification: SkipToFloor,
                validateInput: (v) => v = Math.Clamp(v, 1, RunInfo.Floors.Count),
                retrieveData: () => $"Max. {RunInfo.Floors.Count}"
            ));

            container = UIUtil.CreateSimpleHorizontalLayout(_panelRoot);
            new ToggleWidget(container, "Always Win Events", (v) => PatchData.AutoWinEventRolls ^= true);
        }

        public override void Update()
        {
            base.Update();
            foreach (var widget in widgets)
            {
                widget.Update();
            }
        }

        private void KillAllEnemies()
        {
            FloorSystem.Instance.battle.DebugKillAllEnemies();
        }

        private void KillSelectedEnemy()
        {
            FloorSystem.Instance.battle.DebugKillSelectedEnemy();
        }

        private void SkipToFloor(int floor)
        {
            FloorSystem.Instance.ForceMoveToFloor(floor);
        }


    }
}
