using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace XCFramework.Editor
{
    public class DebugPanel : IPanel
    {
        /*
         * 私有单例
         */
        private static DebugPanel instance;
        /// <summary>
        /// 维护所有的页签分组
        /// </summary>
        private Dictionary<string, List<System.Type>> editors = new Dictionary<string, List<System.Type>>();
        private PrefVariable save_CurrentEditor = new PrefVariable(string.Format("{0}_DebugPanel_CurrentEditor", (object) 27990));
        private UnityEngine.Color selectionColor;
        private UnityEngine.Color bgColor;
        private UnityEngine.Color defalutColor;
        private IMetaEditor current_editor;
        public PanelTabAttribute editorAttribute;
        private Action editorRender;
        private static bool style_IsInitialized;
        private static GUIStyle style_Exeption;
        private static GUIStyle tabButtonStyle;
        public static Rect currectEditorRect;
        
        public Vector2 editorScroll;
        public Vector2 tabsScroll;
        
        /// <summary>
        /// 初始化样式
        /// </summary>
        private void InitializeStyles()
        {
            DebugPanel.style_Exeption = new GUIStyle(GUI.skin.label);
            DebugPanel.style_Exeption.normal.textColor = new UnityEngine.Color(0.5f, 0.0f, 0.0f, 1f);
            DebugPanel.style_Exeption.alignment = TextAnchor.UpperLeft;
            DebugPanel.style_Exeption.wordWrap = true;
            DebugPanel.tabButtonStyle = new GUIStyle(EditorStyles.miniButton);
            DebugPanel.tabButtonStyle.normal.textColor = UnityEngine.Color.white;
            DebugPanel.tabButtonStyle.active.textColor = new UnityEngine.Color(1f, 0.8f, 0.8f, 1f);
            DebugPanel.style_IsInitialized = true;
        }
        
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
                panel = DebugPanel.instance;
                panel.Show();
            }
            return panel;
        }
        
        public void Show(IMetaEditor editor)
        {
            EditorGUI.FocusTextInControl("");
            if (!editor.Initialize())
                return;
            this.current_editor = editor;
            this.save_CurrentEditor.String = editor.GetType().FullName;
            this.editorAttribute = (PanelTabAttribute) ((IEnumerable<object>) editor.GetType().GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (x => x is PanelTabAttribute));
            this.editorRender = new Action(this.current_editor.OnGUI);
        }


        /*
         * 生命周期只有一次 
         */
        protected override void OnEnable()
        {
            DebugPanel.instance = this;
            CoreStyles.Initialize();
            this.titleContent.text = "DebugPanel";
            //加载所有代码
            this.LoadEditors();
            //选择一个meta做默认
            this.ShowFirstEditor();
            this.selectionColor = UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.white, 0.7f);
            this.bgColor = UnityEngine.Color.Lerp(GUI.backgroundColor, UnityEngine.Color.black, 0.3f);
        }

        protected override void ShowFirstEditor()
        {
            if (!this.save_CurrentEditor.IsEmpty())
            {
                System.Type type1 = typeof (IMetaEditor);
                System.Type type2 = typeof (MetaEditor<>);
                string editorName = this.save_CurrentEditor.String;
                System.Type type3 = this.editors.Values.SelectMany<List<System.Type>, System.Type>((Func<List<System.Type>, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) x)).FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => x.FullName == editorName));
                if (type1.IsAssignableFrom(type3) && type3 != type1 && type3 != type2)
                {
                    this.Show((IMetaEditor) Activator.CreateInstance(type3));
                    return;
                }
            }
            System.Type type = this.editors.Values.SelectMany<List<System.Type>, System.Type>((Func<List<System.Type>, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) x)).FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => ((IEnumerable<object>) x.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (y => y is PanelDefaultAttribute)) != null));
            if (type == null)
                return;
            this.Show((IMetaEditor) Activator.CreateInstance(type));
        }

        protected override void LoadEditors()
        {
            System.Type type1 = typeof(IMetaEditor);
            System.Type type2 = typeof(MetaEditor<>);
            List<System.Type> typeList = new List<System.Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (System.Type type3 in assembly.GetTypes())
                    {
                        if (type1.IsAssignableFrom(type3) && type3 != type1 && type3 != type2)
                            typeList.Add(type3);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                }
            }
            
            typeList.RemoveAll((Predicate<System.Type>) (x => ((IEnumerable<object>) x.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (y => y is PanelTabAttribute)) == null));
            typeList.Sort((Comparison<System.Type>) ((a, b) => ((PanelTabAttribute) ((IEnumerable<object>) a.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (x => x is PanelTabAttribute))).Priority.CompareTo(((PanelTabAttribute) ((IEnumerable<object>) b.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (x => x is PanelTabAttribute))).Priority)));
            this.editors.Clear();
            foreach (System.Type type3 in typeList)
            {
                PanelGroupAttribute panelGroupAttribute = (PanelGroupAttribute) ((IEnumerable<object>) type3.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (x => x is PanelGroupAttribute));
                string key = panelGroupAttribute != null ? panelGroupAttribute.Group : "";
                if (!this.editors.ContainsKey(key))
                    this.editors.Add(key, new List<System.Type>());
                this.editors[key].Add(type3);
            }
        }

        private void OnGUI()
        {
            try
            {
                CoreStyles.Update();
                if (!DebugPanel.style_IsInitialized)
                    this.InitializeStyles();
                
                EditorGUILayout.Space();
                if (this.editorRender == null || this.current_editor == null)
                {
                    this.editorRender = (Action) null;
                    this.current_editor = (IMetaEditor) null;
                }
                this.defalutColor = GUI.backgroundColor;

                using (new GUIHelper.Horizontal(new GUILayoutOption[2]
                {
                    GUILayout.ExpandWidth(true),
                    GUILayout.ExpandHeight(true)
                }))
                {
                    using (new GUIHelper.Vertical(CoreStyles.berryArea, new GUILayoutOption[2]
                    {
                        GUILayout.Width(150f),
                        GUILayout.ExpandHeight(true)
                    }))
                    {
                        this.tabsScroll = EditorGUILayout.BeginScrollView(this.tabsScroll);
                        this.DrawTabs();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndScrollView();
                        
                        Rect rect = EditorGUILayout.BeginVertical(CoreStyles.berryArea, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        this.editorScroll = EditorGUILayout.BeginScrollView(this.editorScroll);
                        if (this.current_editor != null && this.editorRender != null)
                        {
                            if (this.editorAttribute != null)
                                this.DrawTitle(this.editorAttribute.Title);
                            try
                            {
                                if (EditorApplication.isCompiling)
                                {
                                    GUILayout.Label("Compiling...", CoreStyles.centeredMiniLabel, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                                }
                                else
                                {
                                    if (Event.current.type == EventType.Repaint)
                                        DebugPanel.currectEditorRect = rect;
                                    this.editorRender();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                GUILayout.Label(ex.ToString(), DebugPanel.style_Exeption);
                            }
                        }else
                            GUILayout.Label("Nothing selected", CoreStyles.centeredMiniLabel, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                        EditorGUILayout.EndScrollView();
                    }
                }

            }catch (ArgumentException ex)
            {
                if (ex.Message.StartsWith("Getting control") && ex.Message.StartsWith("GUILayout"))
                    return;
                Debug.LogException((Exception) ex);
            }
        }
        
        
        private void DrawTabs()
        {
            this.DrawTabs("");
            foreach (KeyValuePair<string, List<System.Type>> editor in this.editors)
            {
                if (!string.IsNullOrEmpty(editor.Key))
                    this.DrawTabs(editor.Key);
            }
        }
        
        private void DrawTabs(string group)
        {
            if (!this.editors.ContainsKey(group))
                return;
            if (!string.IsNullOrEmpty(group))
                this.DrawTabTitle(group);
            foreach (System.Type type in this.editors[group])
            {
                PanelTabAttribute tabAttribute = (PanelTabAttribute) ((IEnumerable<object>) type.GetCustomAttributes(true)).FirstOrDefault<object>((Func<object, bool>) (x => x is PanelTabAttribute));
                if (tabAttribute != null && this.DrawTabButton(tabAttribute))
                    this.Show((IMetaEditor) Activator.CreateInstance(type));
            }
        }
        
        private bool DrawTabButton(PanelTabAttribute tabAttribute)
        {
            bool flag = false;
            if (tabAttribute != null)
            {
                using (new GUIHelper.BackgroundColor(this.editorAttribute == null || !this.editorAttribute.Match((object) tabAttribute) ? UnityEngine.Color.white : this.selectionColor))
                {
                    using (new GUIHelper.ContentColor(CoreStyles.centeredMiniLabel.normal.textColor))
                        flag = GUILayout.Button(new GUIContent(tabAttribute.Title), DebugPanel.tabButtonStyle, GUILayout.ExpandWidth(true));
                }
                if (this.editorAttribute != null && this.editorAttribute.Match((object) tabAttribute) && this.editorRender == null)
                    flag = true;
            }
            return flag;
        }
        
        private void DrawTabTitle(string text)
        {
            GUILayout.Label(text, CoreStyles.centeredMiniLabel, GUILayout.ExpandWidth(true));
        }
        
        private void DrawTitle(string text)
        {
            using (new GUIHelper.Horizontal(new GUILayoutOption[0]))
            {
                GUILayout.Label(text, CoreStyles.largeTitle, GUILayout.ExpandWidth(true));
            }
            GUILayout.Space(10f);
        }
        
        public static void Scroll(float position)
        {
            if (!((UnityEngine.Object) DebugPanel.instance != (UnityEngine.Object) null))
                return;
            DebugPanel.instance.editorScroll = new Vector2(0.0f, position);
        }

        public static IMetaEditor GetCurrentEditor()
        {
            if ((UnityEngine.Object) DebugPanel.instance == (UnityEngine.Object) null)
                return (IMetaEditor) null;
            return DebugPanel.instance.current_editor;
        }
        
        public void Show(string editorName)
        {
            System.Type type = this.editors.SelectMany<KeyValuePair<string, List<System.Type>>, System.Type>((Func<KeyValuePair<string, List<System.Type>>, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) x.Value)).FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => x.Name == editorName));
            if (type == null)
                return;
            this.Show((IMetaEditor) Activator.CreateInstance(type));
        }
    }
}