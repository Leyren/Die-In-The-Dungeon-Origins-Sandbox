using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    public class Widget
    {
        private Func<bool> enabledIf;
        protected GameObject _widgetRoot;

        public bool Enabled { private set; get; }

        public Widget()
        {
            Enabled = true;
        }

        public virtual void Update()
        {
            if (enabledIf != null)
            {
                var newValue = enabledIf.Invoke();
                if (newValue != Enabled)
                {
                    Enabled = newValue;
                    UpdateEditable();
                }
            }
        }

        protected virtual void UpdateEditable()
        {

        }

        public GameObject GetWidgetRoot()
        {
            return _widgetRoot;
        }

        public Widget EnabledIf(Func<bool> predicate)
        {
            enabledIf = predicate;
            return this;
        }
    }
}
