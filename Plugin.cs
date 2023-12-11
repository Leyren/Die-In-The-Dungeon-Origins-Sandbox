using BepInEx;
using BepInEx.Logging;
using DG.Tweening;
using DieInTheDungeonOriginsSandbox.UI;
using HarmonyLib;
using MEC;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Schema;
using UnityEngine;
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

    private void OnUIInitialized()
    {
        UIBase = UniversalUI.RegisterUI("leyren.dieinthedungeons", UpdateUI);
    }

    void UpdateUI()
    {
        if (panel == null && PluginUtil.IsGamePlaying())
        {
            Plugin.Log.LogInfo("Initialized Cheat Panel");
            panel = new CheatPanel(UIBase);
        }

        if (panel != null)
        {
            panel.Update();
        }
    }
}

