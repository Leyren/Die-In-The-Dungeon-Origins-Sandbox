using DieInTheDungeonOriginsSandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal class RelicSelectorComponent: PluginComponent
    {

        private RectTransform RelicSelectorLayoutRect;

        public RelicSelectorComponent(GameObject parent): base(parent, "Relics") {
            InitializeUI();
        }

        private void InitializeUI()
        {
            PluginUI.CreateButton(_panelRoot, "Open Relic Selection", onClick: OpenRelicSelector);
            PluginUI.CreateButton(_panelRoot, "Get All Relics", onClick: ObtainAllRelics);
        }

        public override void Update()
        {
            if (RelicSelectorLayoutRect != null)
            {
                if (!CanvasManager.Instance.relicSelector.isActiveAndEnabled)
                {
                    // This is only a fallback, in case there was an exception during relic selection.
                    DisableLayoutModifications();
                }
                else
                {
                    float value = Input.GetKey(KeyCode.RightArrow) ? -1 : Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
                    RelicSelectorLayoutRect.Translate(Vector2.right * value * 0.2f);
                }
            }
        }

        private List<RelicData> GetAllRelics()
        {
            FieldInfo field = typeof(Relics).GetField("allRelicsAndCurses", BindingFlags.NonPublic | BindingFlags.Instance);
            List<RelicData> relics = new((List<RelicData>)field.GetValue(Relics.Instance));
            return relics.OrderBy(e => e.rarity).ToList();
        }

        private void ObtainAllRelics()
        {
            GetAllRelics().ForEach(Relics.Instance.AddRelic);
        }

        private void OpenRelicSelector()
        {
            var allRelics = GetAllRelics();
            CanvasManager.Instance.relicSelector.ShowRelicsFromThisPool(allRelics, allRelics.Count, (v) => DisableLayoutModifications());
            RelicSelectorLayoutRect = CanvasManager.Instance.relicSelector.layoutRT;
        }

        private void DisableLayoutModifications()
        {
            Plugin.Log.LogInfo("Reset RelicSelectorLayoutRect");
            RelicSelectorLayoutRect.localPosition = Vector3.zero;
            RelicSelectorLayoutRect = null;
        }
    }
}
