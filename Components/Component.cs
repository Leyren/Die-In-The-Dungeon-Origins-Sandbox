using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DieInTheDungeonOriginsSandbox.Components
{
    internal abstract class PluginComponent
    {
        protected GameObject _panelRoot;

        public PluginComponent(GameObject panelRoot) {
            _panelRoot = panelRoot;
        }

        public virtual void Update()
        {

        }

    }
}
