using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using DieInTheDungeonOriginsSandbox.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class StatModifierComponent : PluginComponent
    {
        private readonly List<Widget> widgets = [];

        public StatModifierComponent(GameObject panelRoot) : base(panelRoot)
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            widgets.Add(new ModificationWidget<int>("max-dice", _panelRoot, "Modify Max. Dice in Hand by", 0,
                applyModification: ModifyMaxDiceInHandBy,
                retrieveData: () => GetMaxDiceInHand().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("max-health", _panelRoot, "Modify Max Health by", 0,
                applyModification: ModifyMaxHealthBy,
                retrieveData: () => GetMaxHealth().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("heal", _panelRoot, "Modify Health by", 0,
                applyModification: ModifyHealthBy,
                retrieveData: () => GetHealth().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("block", _panelRoot, "Modify Block by", 0,
                applyModification: ModifyBlockBy,
                retrieveData: () => GetBlock().ToString()
            ));
        }


        public override void Update()
        {
            widgets.ForEach(widget => widget.Update());
        }

        public static int GetMaxDiceInHand()
        {
            return DiceManager.Instance.MaxDiceInHand();
        }

        public static void ModifyMaxDiceInHandBy(int amount)
        {
            PatchData.MaxDiceInHandModifier += amount;
        }

        public static void ModifyMaxHealthBy(int amount)
        {
            FloorSystem.Instance.Player.ChangeMaxHealthWithVariation(amount);
        }

        public static void ModifyHealthBy(int amount)
        {
            if (amount > 0)
            {
                FloorSystem.Instance.Player.Heal(amount);
            }
            else
            {
                FloorSystem.Instance.Player.Damage(new Hit(-amount), in FloorSystem.Instance.Player.AudioPlayer.looseHealthSFX);
            }
        }
        public static void ModifyBlockBy(int amount)
        {
            FloorSystem.Instance.Player.ChangeBlock(amount);
        }

        public static int GetMaxHealth()
        {
            return FloorSystem.Instance.Player.MaxHealth;
        }

        public static int GetHealth()
        {
            return FloorSystem.Instance.Player.CurrentHealth;
        }

        public static int GetBlock()
        {
            return FloorSystem.Instance.Player.CurrentBlock;
        }

    }
}
