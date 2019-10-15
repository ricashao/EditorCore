using UnityEditor;

namespace KCFramework.Editor
{
    [WindowMeta("Debeg2Panel")]
    public class Debug2Panel : IPanel<Debug2Panel>
    {
        [MenuItem("Window/Debug2 Panel")]
        public static Debug2Panel CreateDebugPanel()
        {
            Debug2Panel panel;
            if ((UnityEngine.Object) Debug2Panel.instance == (UnityEngine.Object) null)
            {
                panel = EditorWindow.GetWindow<Debug2Panel>();
                panel.Show();
                panel.OnEnable();
            }
            else
            {
                panel = ((Debug2Panel) Debug2Panel.instance);
                panel.Show();
            }

            return panel;
        }
    }
}