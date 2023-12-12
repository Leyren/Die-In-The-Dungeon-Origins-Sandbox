using DieInTheDungeonOriginsSandbox.UI.Widgets;
using DieInTheDungeonOriginsSandbox.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UniverseLib.UI;
using DieInTheDungeonOriginsSandbox.Core;
using UnityEngine.UI;
using MEC;
using System.Linq;
using System.Reflection;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class DiceComponent : PluginComponent
    {
        public DiceComponent(GameObject parent) : base(parent, "Dices")
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            GameObject container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            PluginUI.CreateButton(container, "Open Upgrade Menu", onClick: OpenDiceUpgradeMenu);
            PluginUI.CreateButton(container, "Upgrade All Dices", onClick: UpgradeAllDices);

            container = PluginUI.CreateSimpleHorizontalLayout(_panelRoot);
            PluginUI.CreateButton(container, "Open Discard Menu", onClick: OpenDiceDiscardMenu);
            PluginUI.CreateButton(container, "Get Random Dice", onClick: GetRandomDice);
            new DropdownWidget<DiceData>(_panelRoot, "Add Dice", GetAllDices().ToArray(), d => d.ToString(), GetSelectedDice);
            new DropdownWidget<DiceData.Property>(_panelRoot, "Add Property to Dice", GetAllProperties(), d => d.ToString(), OpenGrantPropertyMenu);
        }

        private static bool CanModifyDices()
        {
            return FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice
                || FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectReward;
        }


        public static void OpenDiceUpgradeMenu()
        {
            WrapOpenDiceMenu(() => CanvasManager.Instance.backpackUI.OpenToUpgrade(upgradeToMaxTier: false, maxTargetDice: 9999, onComplete: RedrawDicesIfNeeded));
        }

        public static void OpenDiceDiscardMenu()
        {
            WrapOpenDiceMenu(() => CanvasManager.Instance.backpackUI.Open(BackpackUI.OpenData.State.RemoveDice, maxTargetDice: 9999, onComplete: RedrawDicesIfNeeded));
        }

        public static void OpenGrantPropertyMenu(DiceData.Property property)
        {
            WrapOpenDiceMenu(() => CanvasManager.Instance.backpackUI.OpenToGrantProperty(property, maxTargetDice: 9999, onComplete: RedrawDicesIfNeeded));
        }

        private static void WrapOpenDiceMenu(Action openAction)
        {
            if (!CanModifyDices()) return;

            CanvasManager canvasManager = CanvasManager.Instance;

            ClearHand();

            // Open discard UI and redraw dices when done
            openAction.Invoke();;

            // This is done to disable the 'return' button that would display the reward menu
            // and enable the 'skip' button
            canvasManager.backpackUI.openData.ReduceTargetDiceAmount();
        }

        public static void UpgradeAllDices()
        {
            if (!CanModifyDices()) return;

            // If there are no dices to upgrade, abort
            if (!DiceManager.Instance.CurrentRunDeck.dice.Any(d => d.HaveAnyUpgrade)) return;

            ClearHand();

            Timing.RunCoroutine(_UpgradeAllDices());
        }

        private static void ClearHand()
        {
            DiceManager diceManager = DiceManager.Instance;
            CanvasManager canvasManager = CanvasManager.Instance;

            // Note: most of this code only serves to get rid of the dices in the hand, because the upgrade menu doesn't take care of that
            // otherwise you can only upgrade the ones in your current draw pile
            // Get current dices in hand 
            List<ActiveDice> currentDiceOnHand = new(diceManager.diceInHand.AllDice);

            // Discard all dices
            currentDiceOnHand.ForEach(dice => diceManager.DiscardDice(canvasManager.hand.GetGameObjectDiceFromActiveDice(dice)));

            // Move everything back to draw pile
            diceManager.diceInDrawPile.AddRange(diceManager.diceInDiscardPile.AllDice);
            diceManager.diceInDiscardPile.Clear();

            diceManager.diceInDrawPile.RefreshVisuals();
        }

        private static void RedrawDicesIfNeeded()
        {
            if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice)
            {
                Timing.RunCoroutine(DiceManager.Instance._DrawDiceToHand());
            }
        }

        private static IEnumerator<float> _UpgradeAllDices()
        {
            DiceManager diceManager = DiceManager.Instance;
            List<ActiveDice> dicesToUpgrade;
            while ((dicesToUpgrade = diceManager.diceInDrawPile.AllDice.Where(d => d.HaveAnyUpgrade).ToList()).Count > 0)
            {
                foreach (var dice in dicesToUpgrade)
                {
                    diceManager.UpgradeDice(dice);
                    yield return Timing.WaitForOneFrame;
                }
            }
            RedrawDicesIfNeeded();
        }

        public static List<DiceData> GetAllDices()
        {
            FieldInfo field = typeof(DiceManager).GetField("allDice", BindingFlags.NonPublic | BindingFlags.Instance);
            List<DiceData> dices = new((List<DiceData>)field.GetValue(DiceManager.Instance));
            return dices.OrderBy(e => e.GetType()).ThenBy(e => e.tier).ThenBy(e => e.rarity).ToList();
        }

        public static DiceData.Property[] GetAllProperties()
        {
            return (DiceData.Property[]) Enum.GetValues(typeof(DiceData.Property));
        }

        public static void GetRandomDice()
        {
            var diceData = DiceManager.Instance.GetMultipleRandomDice(1)[0];
            var dice = new ActiveDice(diceData, FloorSystem.Instance.Player);
            DiceManager.Instance.ObtainNewDice(dice);
        }

        public static void GetSelectedDice(DiceData diceData)
        {
            var dice = new ActiveDice(diceData, FloorSystem.Instance.Player);
            DiceManager.Instance.ObtainNewDice(dice);
        }
    }
}
