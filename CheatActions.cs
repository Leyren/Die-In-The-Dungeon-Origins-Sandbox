using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace DieInTheDungeonOriginsSandbox
{
    /// <summary>
    /// Additional layer of abstraction to act as kind of an 'API' between plugin code and game code.
    /// </summary>
    public static class CheatActions
    {

        public static int GetMaxDiceInHand()
        {
            return DiceManager.Instance.MaxDiceInHand();
        }

        public static void ModifyMaxDiceInHandBy(int amount)
        {
            Data.MaxDiceInHandModifier += amount;
        }

        public static void ModifyMaxHealthBy(int amount)
        {
            FloorSystem.Instance.Player.ChangeMaxHealthWithVariation(amount);
        }

        public static int GetMaxHealth()
        {
            return FloorSystem.Instance.Player.MaxHealth;
        }

        public static void OpenDiceUpgradeMenu()
        {
            if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice
                || FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectReward)
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

                // Open upgrade UI and redraw dices when done
                canvasManager.backpackUI.OpenToUpgrade(upgradeToMaxTier: false, maxTargetDice: 9999,
                    onComplete: () =>
                    {
                        if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice)
                        {
                            Timing.RunCoroutine(diceManager._DrawDiceToHand());
                        }
                    });

                // This is done to disable the 'return' button that would display the reward menu
                // and enable the 'skip' button
                canvasManager.backpackUI.openData.ReduceTargetDiceAmount();
            }
        }

        public static void UpgradeAllDices()
        {
            if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice
                || FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectReward)
            {
                DiceManager diceManager = DiceManager.Instance;
                CanvasManager canvasManager = CanvasManager.Instance;

                // If there are no dices to upgrade, abort
                if (!diceManager.CurrentRunDeck.dice.Any(d => d.HaveAnyUpgrade)) return;

                // Discard all dices from hand
                List<ActiveDice> dices = new(diceManager.diceInHand.AllDice);
                dices.ForEach(dice => diceManager.DiscardDice(canvasManager.hand.GetGameObjectDiceFromActiveDice(dice)));

                // Move everything back to draw pile
                diceManager.diceInDrawPile.AddRange(diceManager.diceInDiscardPile.AllDice);
                diceManager.diceInDiscardPile.Clear();

                diceManager.diceInDrawPile.RefreshVisuals();

                Timing.RunCoroutine(_UpgradeAllDices());
            }
        }

        static IEnumerator<float> _UpgradeAllDices()
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
            if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice)
            {
                Timing.RunCoroutine(diceManager._DrawDiceToHand());
            }
        }

        public static List<DiceData> GetAllDices()
        {
            FieldInfo field = typeof(DiceManager).GetField("allDice", BindingFlags.NonPublic | BindingFlags.Instance);
            List<DiceData> dices = new((List<DiceData>)field.GetValue(DiceManager.Instance));
            return dices.OrderBy(e => e.GetType()).ThenBy(e => e.tier).ThenBy(e => e.rarity).ToList();
        }

        public static void GetRandomDice()
        {
            var diceData = DiceManager.Instance.GetMultipleRandomDice(1)[0];
            var dice = new ActiveDice(diceData, FloorSystem.Instance.Player);
            DiceManager.Instance.ObtainNewDice(dice);
        }

        public static void GetNiceDice(DiceData diceData)
        {
            var dice = new ActiveDice(diceData, FloorSystem.Instance.Player);
            DiceManager.Instance.ObtainNewDice(dice);
        }

    }
}
