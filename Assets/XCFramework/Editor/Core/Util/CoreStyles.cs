using System;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace KCFramework.Editor
{
    public static class CoreStyles
    {
        public static GUIStyle richLabel;
        public static GUIStyle whiteRichLabel;
        public static GUIStyle whiteLabel;
        public static GUIStyle whiteBoldLabel;
        public static GUIStyle multilineLabel;
        public static GUIStyle centeredLabel;
        public static GUIStyle miniLabel;
        public static GUIStyle centeredMiniLabel;
        public static GUIStyle centeredMiniLabelWhite;
        public static GUIStyle centeredMiniLabelBlack;
        public static GUIStyle largeTitle;
        public static GUIStyle title;
        public static GUIStyle tabArea;
        public static GUIStyle area;
        public static GUIStyle levelArea;
        public static GUIStyle textAreaLineBreaked;
        public static GUIStyle separator;
        public static GUIStyle monospaceLabel;
        public static string highlightStrongBlue;
        public static string highlightBlue;
        public static string highlightStrongRed;
        public static string highlightRed;
        public static string highlightStrongGreen;
        public static string highlightGreen;

        /// <summary>
        /// 初始化标志位
        /// </summary>
        private static bool initialized = false;

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            EditorApplication.update += new EditorApplication.CallbackFunction(CoreStyles.Update);
        }

        public static void Update()
        {
            if (CoreStyles.initialized)
                return;
            CoreStyles.Initialize();
        }

        public static void Initialize()
        {
            try
            {
                CoreStyles.richLabel = new GUIStyle(EditorStyles.label);
                CoreStyles.richLabel.richText = true;
                CoreStyles.whiteRichLabel = new GUIStyle(CoreStyles.richLabel);
                CoreStyles.whiteRichLabel.normal.textColor = UnityEngine.Color.white;
                CoreStyles.whiteLabel = new GUIStyle(EditorStyles.whiteLabel);
                CoreStyles.whiteLabel.normal.textColor = UnityEngine.Color.white;
                CoreStyles.whiteBoldLabel = new GUIStyle(EditorStyles.whiteBoldLabel);
                CoreStyles.whiteBoldLabel.normal.textColor = UnityEngine.Color.white;
                CoreStyles.multilineLabel = new GUIStyle(EditorStyles.label);
                CoreStyles.multilineLabel.clipping = TextClipping.Clip;
                CoreStyles.centeredLabel = new GUIStyle(EditorStyles.label);
                CoreStyles.centeredLabel.alignment = TextAnchor.MiddleCenter;
                CoreStyles.centeredMiniLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                CoreStyles.centeredMiniLabel.normal.textColor = EditorStyles.label.normal.textColor;
                CoreStyles.miniLabel = new GUIStyle(CoreStyles.centeredMiniLabel);
                CoreStyles.miniLabel.alignment = CoreStyles.richLabel.alignment;
                CoreStyles.centeredMiniLabelWhite = new GUIStyle(CoreStyles.centeredMiniLabel);
                CoreStyles.centeredMiniLabelWhite.normal.textColor = UnityEngine.Color.white;
                CoreStyles.centeredMiniLabelBlack = new GUIStyle(CoreStyles.centeredMiniLabel);
                CoreStyles.centeredMiniLabelBlack.normal.textColor = UnityEngine.Color.black;
                CoreStyles.textAreaLineBreaked = new GUIStyle(EditorStyles.textArea);
                CoreStyles.textAreaLineBreaked.clipping = TextClipping.Clip;
                CoreStyles.monospaceLabel = new GUIStyle(CoreStyles.textAreaLineBreaked);
                CoreStyles.monospaceLabel.font = Resources.Load<Font>("Fonts/CourierNew");
                CoreStyles.monospaceLabel.fontSize = 11;
                CoreStyles.largeTitle = new GUIStyle(EditorStyles.label);
                CoreStyles.largeTitle.normal.textColor = EditorStyles.label.normal.textColor;
                CoreStyles.largeTitle.fontStyle = FontStyle.Bold;
                CoreStyles.largeTitle.fontSize = 21;
                CoreStyles.title = new GUIStyle(EditorStyles.label);
                CoreStyles.title.normal.textColor = EditorStyles.label.normal.textColor;
                CoreStyles.title.fontStyle = FontStyle.Bold;
                CoreStyles.title.fontSize = 18;

                CoreStyles.tabArea = new GUIStyle(EditorStyles.textArea);
//            CoreStyles.berryArea.normal.background = EditorIcons.GetBuildInIcon("BerryArea");
                CoreStyles.tabArea.border = new RectOffset(4, 4, 5, 3);
                CoreStyles.tabArea.margin = new RectOffset(2, 2, 2, 2);
                CoreStyles.tabArea.padding = new RectOffset(2, 2, 2, 2);
                CoreStyles.area = new GUIStyle(EditorStyles.textArea);
//            CoreStyles.area.normal.background = EditorIcons.GetBuildInIcon("Area");
                CoreStyles.area.border = new RectOffset(4, 4, 5, 3);
                CoreStyles.area.margin = new RectOffset(2, 2, 2, 2);
                CoreStyles.area.padding = new RectOffset(4, 4, 4, 4);

                CoreStyles.levelArea = new GUIStyle(CoreStyles.area);
//            CoreStyles.levelArea.normal.background = EditorIcons.GetBuildInIcon("LevelArea");
                CoreStyles.levelArea.border = new RectOffset(4, 4, 5, 3);
                CoreStyles.levelArea.margin = new RectOffset(2, 2, 2, 2);
                CoreStyles.levelArea.padding = new RectOffset(4, 4, 4, 4);

                CoreStyles.separator = new GUIStyle(CoreStyles.area);
//            CoreStyles.separator.normal.background = EditorIcons.GetBuildInIcon("Separator");
                CoreStyles.separator.border = new RectOffset(2, 2, 2, 2);
                CoreStyles.separator.margin = new RectOffset(0, 0, 0, 0);
                CoreStyles.separator.padding = new RectOffset(0, 0, 0, 0);
                CoreStyles.highlightStrongBlue =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "8888ff" : "4444ff") + "ff>{0}</color>";
                CoreStyles.highlightBlue =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "5555bb" : "222266") + "ff>{0}</color>";
                CoreStyles.highlightStrongRed =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "ff8888" : "ff4444") + "ff>{0}</color>";
                CoreStyles.highlightRed =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "bb5555" : "662222") + "ff>{0}</color>";
                CoreStyles.highlightStrongGreen =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "88ff88" : "44ff44") + "ff>{0}</color>";
                CoreStyles.highlightGreen =
                    "<color=#" + (EditorGUIUtility.isProSkin ? "55bb55" : "226622") + "ff>{0}</color>";
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}