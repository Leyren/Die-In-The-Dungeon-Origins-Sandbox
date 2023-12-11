using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using UniverseLib.UI.Models;
using UniverseLib.UI;
using static DieInTheDungeonOriginsSandbox.UI.PluginUI;
using UniverseLib.Utility;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    public class ModificationWidget<T> : Widget
    {
        private readonly Func<string> retrieveData;
        private readonly Text currentData;
        private readonly ButtonRef applyButton;

        public ModificationWidget(string name, GameObject parent, string title, T defaultValue, Action<T> applyModification, Func<string> retrieveData = null, Func<T, T> validateInput = null)
        {
            GameObject container = UIFactory.CreateHorizontalGroup(parent, $"{name}-horizontal", false, false, true, true, spacing: 5, padding: new Vector4(5, 10, 5, 10));
            Text description = UIFactory.CreateLabel(container, $"{name}-title", title);
            UIFactory.SetLayoutElement(description.gameObject, minWidth: WIDTH_LONG, minHeight: ROW_HEIGHT);

            InputFieldRef input = CreateInputField(container, $"{name}-input", defaultValue.ToString());
            applyButton = CreateButton(container, "Apply", w: WIDTH_MEDIUM);

            this.retrieveData = retrieveData;
            if (retrieveData != null)
            {
                string textId = $"{name}-text";
                currentData = UIFactory.CreateLabel(container, textId, "N/A");
                UIFactory.SetLayoutElement(currentData.gameObject, minWidth: WIDTH_MEDIUM, minHeight: ROW_HEIGHT);
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
            _widgetRoot = container;
        }

        public override void Update()
        {
            base.Update();
            if (!Enabled) return;

            if (retrieveData != null) currentData.text = retrieveData.Invoke();
        }

        protected override void UpdateEditable()
        {
            SetEnabled(applyButton, Enabled);
        }
    }
}
