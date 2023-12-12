using BepInEx;
using BepInEx.Logging;
using DG.Tweening;
using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using DieInTheDungeonSandbox.Core;
using HarmonyLib;
using JetBrains.Annotations;
using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Transactions;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace DieInTheDungeonOriginsSandbox;

[BepInPlugin("leyren.dieinthedungeons.sandbox", "DieInTheDungeonsSandbox", "1.0.0")]
public class Plugin : BaseUnityPlugin
{

    internal static ManualLogSource Log;
    public static UIBase UIBase { get; private set; }

    private CheatPanel panel;
    private GameObject toggleButton;

    private void Awake()
    {
        Plugin.Log = base.Logger;
        Logger.LogInfo($"Loading plugin...");

        UniverseLib.Universe.Init(OnUIInitialized);

        PluginConfig.ReadConfig(Config);

        var harmony = new Harmony("leyren.dieinthedungeons");
        harmony.PatchAll();

        // Plugin startup logic
        Logger.LogInfo($"Plugin is loaded!");
    }

    private void OnUIInitialized()
    {
        UIBase = UniversalUI.RegisterUI("leyren.dieinthedungeons", UpdateUI);
    }

    void Update()
    {
    }

    void UpdateUI()
    {
        if (panel != null && panel.Enabled && !PluginUtil.IsGamePlaying())
        {
            panel.SetActive(false);
        }

        if (PluginUtil.IsGamePlaying())
        {
            if (panel == null)
            {
                // Initialization
                Plugin.Log.LogInfo("Initialized Cheat Panel");
                panel = new CheatPanel(UIBase);
                panel.SetActive(false);
                if (PluginConfig.ShowButton)
                {
                    toggleButton = CreateToggleButton();
                }
            }

            toggleButton?.SetActive(!panel.Enabled);

            if (panel.Enabled)
            {
                panel.Update();
            }

            if (Input.GetKeyDown(PluginConfig.Hotkey))
            {
                panel.Toggle();
            }
        }
    }

    public GameObject CreateToggleButton()
    {
        GameObject container = UIFactory.CreateHorizontalGroup(UIBase.RootObject, $"{name}-horizontal", true, true, true, true, spacing: 5, padding: new Vector4(0, 5, 5, 5));

        // Add a content size fitter to adjust the size based on the containing button
        var fitter = container.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        var rect = container.GetComponent<RectTransform>();

        // move to top center
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.anchoredPosition = new Vector2(0, 0);

        var button = PluginUI.CreateButton(container, $"Open Sandbox ({PluginConfig.Hotkey})", w: 175, h: 25, overrideColor: new Color(0.5f, 0.5f, 0.5f));
        button.Component.image.color = Color.red;

        // If button clicked, enable the panel, and if the panel closes, enable the button
        button.OnClick = () =>
        {
            panel.SetActive(true); 
            // Change focus to panel to make sure button is not in focus anymore
            EventSystem.current.SetSelectedGameObject(panel.UIRoot);
            container.SetActive(false);
        }; 

        return container;
    }

}

