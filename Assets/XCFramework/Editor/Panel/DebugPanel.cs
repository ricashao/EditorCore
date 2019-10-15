using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace KCFramework.Editor
{
    [WindowMeta("DebegPanel")]
    public class DebugPanel : IPanel<DebugPanel>
    {
        [MenuItem("Window/Debug Panel")]
        public static DebugPanel CreateDebugPanel()
        {
            DebugPanel panel;
            if ((UnityEngine.Object) DebugPanel.instance == (UnityEngine.Object) null)
            {
                panel = EditorWindow.GetWindow<DebugPanel>();
                panel.Show();
                panel.OnEnable();
            }
            else
            {
                panel = ((DebugPanel) DebugPanel.instance);
                panel.Show();
            }

            return panel;
        }
    }
}