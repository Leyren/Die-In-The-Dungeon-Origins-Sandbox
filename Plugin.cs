using BepInEx;
using BepInEx.Logging;
using DG.Tweening;
using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using HarmonyLib;
using JetBrains.Annotations;
using MEC;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace DieInTheDungeonOriginsSandbox;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{

    internal static new ManualLogSource Log;
    public static UIBase UIBase { get; private set; }

    private CheatPanel panel;

    private static KeyCode hotkey;

    private void Awake()
    {
        Plugin.Log = base.Logger;
        Logger.LogInfo($"Loading plugin...");

        UniverseLib.Universe.Init(OnUIInitialized);

        hotkey = Config.Bind<KeyCode>("General",
                                "GreetingText",
                                KeyCode.F8,
                                "A greeting text to show when the game is launched").Value;

        var harmony = new Harmony("leyren.dieinthedungeons");
        harmony.PatchAll();

        // Plugin startup logic
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
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
        if (panel == null && PluginUtil.IsGamePlaying())
        {
            Plugin.Log.LogInfo("Initialized Cheat Panel");
            panel = new CheatPanel(UIBase);
            panel.SetActive(false);
            CreateToggleButton();
        }

        if (panel != null)
        {
            panel.Update();
        }
    }

    public void CreateToggleButton()
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

        var button = PluginUI.CreateButton(container, $"Open Sandbox (Foobar)", w: 125, h: 25, overrideColor: new Color(0.5f, 0.5f, 0.5f));
        button.Component.image.color = Color.red;

        // If button clicked, enable the panel, and if the panel closes, enable the button
        button.OnClick = () =>
        {
            panel.SetActive(true);
            container.SetActive(false);
        };

        panel.OnToggleEnabled += (v) => { container.SetActive(v); };
        panel.OnPanelClosed = () => { container.SetActive(true); };
    }

}

