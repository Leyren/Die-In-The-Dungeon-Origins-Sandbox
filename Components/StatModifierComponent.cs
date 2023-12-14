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

        public StatModifierComponent(GameObject parent) : base(parent, "Stats")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            widgets.Add(new ModificationWidget<int>("max-dice", _panelRoot, "Modify Max. Dice in Hand by", 0,
                applyModification: ModifyMaxDiceInHandBy,
                retrieveData: () => GetMaxDiceInHand().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("energy", _panelRoot, "Modify Energy by", 0,
                applyModification: ModifyEnergyBy,
                retrieveData: () => GetEnergy().ToString()
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

            widgets.Add(new ModificationWidget<int>("enemy-max-health", _panelRoot, "Modify Enemy Max. Health by", 0,
                applyModification: ModifyEnemyMaxHealthBy,
                retrieveData: () => GetCurrentEnemyMaxHealth().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("enemy-heal", _panelRoot, "Modify Enemy Health by", 0,
                applyModification: ModifyEnemyHealthBy,
                retrieveData: () => GetCurrentEnemyHealth().ToString()
            ));

            widgets.Add(new ModificationWidget<int>("enemy-block", _panelRoot, "Modify Enemy Block by", 0,
                applyModification: ModifyEnemyBlockBy,
                retrieveData: () => GetCurrentEnemyBlock().ToString()
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

        public static void ModifyEnergyBy(int amount)
        {
            DiceManager.Instance.CurrentEnergy += amount;
        }

        public static void ModifyEnemyMaxHealthBy(int amount)
        {
            var battle = FloorSystem.Instance.battle;
            var enemy = battle.battleInfo.CurrentTargetEnemy;
            enemy.ChangeMaxHealthWithVariation(amount);
        }

        public static void ModifyEnemyHealthBy(int amount)
        {
            var battle = FloorSystem.Instance.battle;
            var player = FloorSystem.Instance.Player;
            var enemy = battle.battleInfo.CurrentTargetEnemy;
            if (amount > 0)
            {
                enemy.Heal(amount);
            }
            else
            {
                enemy.Damage(new Hit(-amount, attacker: player), in player.AudioEntity.combatAttackHitSFX);
                if (!battle.battleInfo.IsAnyEnemyLeft())
                {
                    battle.StartBattleActions();
                } else if (!battle.battleInfo.IsCurrentTargetValid())
                {
                    battle.battleInfo.ResetCurrentTargetIndex();
                }
            }
        }

        public static void ModifyEnemyBlockBy(int amount)
        {
            var battle = FloorSystem.Instance.battle;
            var enemy = battle.battleInfo.CurrentTargetEnemy;
            enemy.ChangeBlock(amount);
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

        public static int GetEnergy()
        {
            return DiceManager.Instance.CurrentEnergy;
        }
        public static int GetCurrentEnemyMaxHealth()
        {
            return FloorSystem.Instance.battle.battleInfo.CurrentTargetEnemy.MaxHealth;
        }

        public static int GetCurrentEnemyHealth()
        {
            return FloorSystem.Instance.battle.battleInfo.CurrentTargetEnemy.CurrentHealth;
        }
        public static int GetCurrentEnemyBlock()
        {
            return FloorSystem.Instance.battle.battleInfo.CurrentTargetEnemy.CurrentBlock;
        }
    }
}
