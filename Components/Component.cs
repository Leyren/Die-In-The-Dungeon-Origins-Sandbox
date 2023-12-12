using DieInTheDungeonOriginsSandbox.Core;
using DieInTheDungeonOriginsSandbox.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal abstract class PluginComponent
    {
        protected GameObject _panelRoot;

        public PluginComponent(GameObject parent, string title) {
            var name = GetType().Name;

            // Create panel for this component
            UIFactory.CreatePanel($"{name}-root", parent, out var contentHolder, bgColor: PluginUI.PANEL_COLOR);
            var layout = contentHolder.GetComponent<VerticalLayoutGroup>();
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;
            layout.spacing = 5;

            var fitter = contentHolder.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            _panelRoot = contentHolder;

            // Add title
            Text text = UIFactory.CreateLabel(_panelRoot, $"{name}-title", title, fontSize: 16);
            text.fontStyle = FontStyle.Bold;
            text.color = Color.white.Mix(Color.black, 0.1f);

            // Separator line
            GameObject obj = UIFactory.CreateUIObject($"{name}-separator", _panelRoot);
            UIFactory.SetLayoutElement(obj, minHeight: 1, preferredHeight: 1, flexibleWidth: 9999);
            Image img = obj.AddComponent<Image>();
            img.color = PluginUI.BACKGROUND_COLOR;

            UIFactory.CreateUIObject("spacer", _panelRoot);
            UIFactory.CreateUIObject("spacer", _panelRoot);
        }

        public virtual void Update()
        {

        }

    }
}
