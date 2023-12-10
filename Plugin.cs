﻿using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MEC;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace DieInTheDungeonOriginsSandbox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{

    internal static new ManualLogSource Log;
    public static UIBase UiBase { get; private set; }

    private void Awake()
    {
        Plugin.Log = base.Logger;
        Logger.LogInfo($"Loading plugin...");

        UniverseLib.Universe.Init(OnUIInitialized);

        // required to ensure that it is initialized before harmony patches it. this is required since Hit has a static member instance of itself
        // which would already call the patched methods in its constructor, before the patch was applied.
       /* System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Attack).TypeHandle);
        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(Hit).TypeHandle);

        var a = Attack.ForceKill;*/

        var harmony = new Harmony("leyren.dieinthedungeons");
        harmony.PatchAll();

        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    private MyPanel panel;
    private void OnUIInitialized()
    {
        UiBase = UniversalUI.RegisterUI("my.unique.ID", UiUpdate); 
        MyPanel myPanel = new(UiBase);
        panel = myPanel;
    }
    void UiUpdate()
    {
        panel.Update();
    }
}

public class Widget
{

    private Func<bool> enabledIf;
    protected bool _enabled = true;

    public Widget()
    {
    }

    public virtual void Update()
    {
        if (enabledIf != null)
        {
            _enabled = enabledIf.Invoke();
        }
    }

    public Widget EnabledIf(Func<bool> predicate)
    {
        enabledIf = predicate;
        return this;
    }
}

public class ModificationWidget<T> : Widget
{

    private readonly Func<string> retrieveData;
    private readonly Text currentData;
    private readonly ButtonRef applyButton;

    public ModificationWidget(string name, GameObject parent, string title, T defaultValue, Action<T> applyModification, Func<string> retrieveData = null, Func<T, T> validateInput = null)
    {
        GameObject container = UIFactory.CreateHorizontalGroup(parent, $"{name}-horizontal", false, false, true, true);
        Text description = UIFactory.CreateLabel(container, $"{name}-title", title);
        UIFactory.SetLayoutElement(description.gameObject, minWidth: 100, minHeight: 25);
        InputFieldRef input = UIFactory.CreateInputField(container, $"{name}-input", defaultValue.ToString());
        UIFactory.SetLayoutElement(input.GameObject, minWidth: 100, minHeight: 25);
        applyButton = UIFactory.CreateButton(container, $"{name}-button", "Apply");
        UIFactory.SetLayoutElement(applyButton.GameObject, minWidth: 200, minHeight: 25);

        this.retrieveData = retrieveData;
        if (retrieveData != null)
        {
            string textId = $"{name}-text";
            currentData = UIFactory.CreateLabel(container, textId, "N/A");
            UIFactory.SetLayoutElement(currentData.gameObject, minWidth: 100, minHeight: 25);
        }

        applyButton.OnClick = () =>
        {
            if (ParseUtility.TryParse<T>(input.Text, out var v, out var e))
            {
                v = validateInput != null ? validateInput.Invoke(v) : v;
                input.Text = v.ToString();
                applyModification.Invoke(v);
            }
        };
    }

    public override void Update()
    {
        base.Update();
        applyButton.Component.interactable = _enabled;
        if (!_enabled) return;
        if (retrieveData != null) currentData.text = retrieveData.Invoke();
    }
}

public class MyPanel : UniverseLib.UI.Panels.PanelBase
{
    public MyPanel(UIBase owner) : base(owner) { }

    public override string Name => "My Panel";
    public override int MinWidth => 100;
    public override int MinHeight => 200;
    public override Vector2 DefaultAnchorMin => new(0.25f, 0.25f);
    public override Vector2 DefaultAnchorMax => new(0.75f, 0.75f);
    public override bool CanDragAndResize => true;

    private List<Widget> widgets = new();

    protected override void ConstructPanelContent()
    {

        widgets.Add(new ModificationWidget<int>("max-dice", ContentRoot, "Modify Max. Dice in Hand by", 0,
            (v) => Data.MaxDiceInHandModifier = v,
            () => DiceManager.Instance.MaxDiceInHand().ToString(),
            (v) => Math.Max(0, v))
            .EnabledIf(() => GameManager.IsInitialized && GameManager.Instance.IsGamePlaying()));
        widgets.Add(new ModificationWidget<int>("max-health", ContentRoot, "Modify Max Health by", 0, 
            (v) => FloorSystem.Instance.Player.ChangeMaxHealthWithVariation(v), 
            () => FloorSystem.Instance.Player.MaxHealth.ToString(),
            (v) => Math.Max(0, v))
            .EnabledIf(() => GameManager.IsInitialized && GameManager.Instance.IsGamePlaying())
           );
        AddButtonToggle("Invulnerable", (v) => Data.Invulnerable = v);
        AddButtonToggle("Force Kill", (v) => Data.ForceKill = v);
        AddButton("Dice Upgrade Menu", () => {
            if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice
            || FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectReward)
            {
                // Note: most of this code only serves to get rid of the dices in the hand, because the upgrade menu doesn't take care of that
                // otherwise you can only upgrade the ones in your current draw pile
                // Get current dices in hand 
                List<ActiveDice> currentDiceOnHand = new List<ActiveDice>(DiceManager.Instance.diceInHand.AllDice);

                // Discard all dices
                for (int i = currentDiceOnHand.Count - 1; i >= 0; i--)
                {
                    GameObjectDice gameObjectDiceFromActiveDice = Singleton<CanvasManager>.Instance.hand.GetGameObjectDiceFromActiveDice(currentDiceOnHand[i]);
                    DiceManager.Instance.DiscardDice(gameObjectDiceFromActiveDice);
                }

                // Move everything back to draw pile
                DiceManager.Instance.diceInDrawPile.AddRange(DiceManager.Instance.diceInDiscardPile.AllDice);
                DiceManager.Instance.diceInDiscardPile.Clear();

                DiceManager.Instance.diceInDrawPile.RefreshVisuals();

                // Open upgrade UI and redraw dices when done
                Singleton<CanvasManager>.Instance.backpackUI.OpenToUpgrade(upgradeToMaxTier: false, maxTargetDice: 9999, 
                    onComplete: () => {
                        if (FloorSystem.Instance.battle.CurrentTurnState == BattleSystem.TurnState.SelectDice)
                            Timing.RunCoroutine(DiceManager.Instance._DrawDiceToHand());
                        });

                // This is done to disable the 'return' button that would display the reward menu
                // and enable the 'skip' button
                Singleton<CanvasManager>.Instance.backpackUI.openData.ReduceTargetDiceAmount();
            }
            });
    }

    private Dictionary<string, Action> dataBindings = new();

    public void Update()
    {
        foreach (var binding in widgets)
        {
            binding.Update();
        }
    }

    private void AddButton(string name, Action onClick)
    {
        ButtonRef b = UIFactory.CreateButton(ContentRoot, name, name);
        UIFactory.SetLayoutElement(b.GameObject, minWidth: 200, minHeight: 25);
        b.OnClick = onClick;
    }

    private void AddButtonToggle(string name, Action<bool> onToggle)
    {
        bool toggle = false;
        ButtonRef b = UIFactory.CreateButton(ContentRoot, name, name);
        Color baseColor = b.Component.image.color;
        UIFactory.SetLayoutElement(b.GameObject, minWidth: 200, minHeight: 25);
        b.OnClick = () => {
            toggle ^= true;
            b.Component.image.color = toggle ? Color.green : baseColor;
            onToggle.Invoke(toggle);
        };
    }

    private void AddModifierWidget(string name, string title, int defaultValue, Action<int> apply, Func<string> retrieveData = null, int? min = 0, int? max = null)
    {
        GameObject container = UIFactory.CreateHorizontalGroup(ContentRoot, $"{name}-horizontal", false, false, true, true);
        Text description = UIFactory.CreateLabel(container, $"{name}-title", title);
        UIFactory.SetLayoutElement(description.gameObject, minWidth: 100, minHeight: 25);
        InputFieldRef input = UIFactory.CreateInputField(container, $"{name}-input", defaultValue.ToString());
        UIFactory.SetLayoutElement(input.GameObject, minWidth: 100, minHeight: 25);
        ButtonRef applyButton = UIFactory.CreateButton(container, $"{name}-button", "Apply");
        UIFactory.SetLayoutElement(applyButton.GameObject, minWidth: 200, minHeight: 25);

        if (retrieveData != null) {
            string textId = $"{name}-text";
            Text currentData = UIFactory.CreateLabel(container, textId, "");
            UIFactory.SetLayoutElement(currentData.gameObject, minWidth: 100, minHeight: 25);
            dataBindings.Add(textId , () => currentData.text = retrieveData.Invoke());
        }

        applyButton.OnClick = () =>
        {
            if (ParseUtility.TryParse<int>(input.Text, out var v, out var e))
            {
                if (min != null) v = Math.Max(min.Value, v);
                if (max != null) v = Math.Min(max.Value, v);
                apply.Invoke(v);
            }
        };
    }

}