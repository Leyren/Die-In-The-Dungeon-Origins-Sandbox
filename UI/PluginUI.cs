using DieInTheDungeonOriginsSandbox.Core;
using System;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace DieInTheDungeonOriginsSandbox.UI
{
    internal static class PluginUI
    {
        public const int WIDTH_LONG = 250;
        public const int WIDTH_MEDIUM = 100;
        public const int WIDTH_SMALL = 50;
        public const int ROW_HEIGHT = 25;
        public static readonly Color DEFAULT_COLOR = new(0.25f, 0.25f, 0.25f);
        public static readonly Color BACKGROUND_COLOR = new(0.065f, 0.065f, 0.065f);
        public static readonly Color PANEL_COLOR = BACKGROUND_COLOR.Mix(Color.white, 0.1f);

        // no expanding, no padding, no background, just horizontal layout with a small spacing
        public static GameObject CreateSimpleHorizontalLayout(GameObject parent, string name = "horizontal")
        {
            GameObject container = UIFactory.CreateHorizontalGroup(parent, name, false, false, true, true, spacing: 5, padding: new Vector4(0, 0, 0, 0));
            GameObject.Destroy(container.GetComponent<Image>());
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
    }
}
