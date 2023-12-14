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
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.Utility;

namespace DieInTheDungeonOriginsSandbox;

[BepInPlugin("DieInTheDungeonsSandbox", "DieInTheDungeonsSandbox", "1.0.0")]
public class Plugin : BaseUnityPlugin
{

    internal static ManualLogSource Log;
    public static UIBase UIBase { get; private set; }

    private CheatPanel panel;
    private GameObject toggleButton;

    internal static Harmony harmony;

    private void Awake()
    {
        Plugin.Log = base.Logger;
        Logger.LogInfo($"Loading plugin...");

        UniverseLib.Universe.Init(OnUIInitialized, logHandler: (s, l) => { });

        PluginConfig.ReadConfig(Config);

        harmony = new Harmony("leyren.dieinthedungeons");
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

    private bool autoReopen = false;

    
    void UpdateUI()
    {
        if (panel != null && panel.Enabled && !PluginUtil.IsGamePlaying())
        {
            panel.SetActive(false);
            autoReopen = true;
            EventSystem.current.SetSelectedGameObject(panel.UIRoot);
        }

        if (PluginUtil.IsGamePlaying())
        {
            // Plugin.Log.LogInfo(FloorSystem.Instance.battle.CurrentTurnState);
            if (panel == null)
            {
                // Initialization
                Plugin.Log.LogInfo("Initialized Cheat Panel");
                panel = new CheatPanel(UIBase);
                panel.SetActive(false);
                if (PluginConfig.ShowButton)
                {
                    toggleButton = UIUtil.CreateToggleButton(UIBase, panel);
                }
            }

            toggleButton?.SetActive(!panel.Enabled);

            if (panel.Enabled)
            {
                ConfigManager.Force_Unlock_Mouse = Contains(Input.mousePosition, panel.Rect.position, panel.Rect.rect.size);
                panel.Update();
            } else if (autoReopen)
            {
                autoReopen = false;
                panel.SetActive(true);
            }

            if (Input.GetKeyDown(PluginConfig.Hotkey))
            {
                panel.Toggle();
            }
        }
    }

    private static bool Contains(Vector2 pos, Vector2 lower, Vector2 size)
    {
        return pos.x > lower.x && pos.x < lower.x + size.x && pos.y < lower.y && pos.y > lower.y - size.y;
    }
}

