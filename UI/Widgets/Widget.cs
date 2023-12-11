using System;
using System.Collections.Generic;
using System.Text;

namespace DieInTheDungeonOriginsSandbox.UI.Widgets
{
    public class Widget
    {
        private Func<bool> enabledIf;
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

        public Widget EnabledIf(Func<bool> predicate)
        {
            enabledIf = predicate;
            return this;
        }
    }
}
