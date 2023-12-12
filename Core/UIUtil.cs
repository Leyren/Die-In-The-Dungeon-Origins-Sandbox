using DieInTheDungeonOriginsSandbox.Core;
using System;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace DieInTheDungeonSandbox.Core
{
    internal static class UIUtil
    {
        public const int WIDTH_LONG = 252;
        public const int WIDTH_MEDIUM = 84;
        public const int WIDTH_SMALL = 42;
        public const int ROW_HEIGHT = 25;
        public static readonly Color DEFAULT_COLOR = new(0.25f, 0.25f, 0.25f);
        public static readonly Color BACKGROUND_COLOR = new(0.065f, 0.065f, 0.065f);
        public static readonly Color PANEL_COLOR = BACKGROUND_COLOR.Mix(Color.white, 0.1f);

        // no expanding, no padding, no background, just horizontal layout with a small spacing
        public static GameObject CreateSimpleHorizontalLayout(GameObject parent, string name = "horizontal")
        {
            GameObject container = UIFactory.CreateHorizontalGroup(parent, name, false, false, true, true, spacing: 5, padding: new Vector4(0, 0, 0, 0));
            UnityEngine.Object.Destroy(container.GetComponent<Image>());
            return container;
        }

        public static ButtonRef CreateButton(GameObject parent, string text, int w = WIDTH_LONG, int h = ROW_HEIGHT, Action onClick = null, Color? overrideColor = null)
        {
            var button = UIFactory.CreateButton(parent, text, text);
            var rect = button.Component.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(w, h);
            UIFactory.SetLayoutElement(button.GameObject, minWidth: w, minHeight: h, preferredWidth: w, preferredHeight: h);

            Color baseColor = overrideColor.HasValue ? overrideColor.Value : DEFAULT_COLOR;
            ColorBlock cb = new()
            {
                normalColor = baseColor,
                highlightedColor = baseColor * 1.2f,
                selectedColor = baseColor * 0.7f,
                disabledColor = baseColor * 0.1f,
                colorMultiplier = 1
            };
            button.Component.colors = cb;
            button.OnClick = onClick;

            return button;
        }

        public static void SetEnabled(ButtonRef button, bool enabled)
        {
            button.Component.interactable = enabled;
            var text = button.Component.GetComponentInChildren<Text>();
            var color = text.color;
            color.a = enabled ? 1.0f : 0.5f;
            text.color = color;
        }

        public static InputFieldRef CreateInputField(GameObject parent, string name, string text, int w = WIDTH_MEDIUM, int h = ROW_HEIGHT)
        {
            InputFieldRef input = UIFactory.CreateInputField(parent, name, text);
            var inputRect = input.Component.placeholder.transform.parent.GetComponent<RectTransform>();
            inputRect.anchorMin = new Vector2(0.05f, 0);
            inputRect.anchorMax = new Vector2(0.95f, 1);
            UIFactory.SetLayoutElement(input.GameObject, minWidth: w, minHeight: h);
            return input;
        }


        public static GameObject CreateToggleButton(UIBase uiBase, PanelBase panel)
        {
            GameObject container = UIFactory.CreateHorizontalGroup(uiBase.RootObject, $"toggle-button-horizontal", true, true, true, true, spacing: 5, padding: new Vector4(0, 5, 5, 5));

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

            var button = UIUtil.CreateButton(container, $"Open Sandbox ({PluginConfig.Hotkey})", w: 175, h: 25, overrideColor: new Color(0.5f, 0.5f, 0.5f));
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
}
