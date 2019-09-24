using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XCFramework.Editor
{
    public class IPanel<T> : EditorWindow
    {
        /*
         * 私有单例
         */
        protected static IPanel<T> instance;

        protected IMetaEditor current_editor;


        public TabAttribute editorAttribute;
        protected Action editorRender;


        /// <summary>
        /// 维护所有的页签分组
        /// </summary>
        protected Dictionary<string, List<System.Type>> editors;

        protected PrefVariable save_CurrentEditor;

        /// <summary>
        /// 样式
        /// </summary>
        protected UnityEngine.Color selectionColor;

        protected UnityEngine.Color bgColor;
        protected UnityEngine.Color defalutColor;


        public Vector2 editorScroll;
        public Vector2 tabsScroll;


        /// <summary>
        /// 生命周期就一次 用作初始化
        /// </summary>
        protected virtual void OnEnable()
        {
            Type type = this.GetType();
            WindowMetaAttribute windowmeta =
                (WindowMetaAttribute) ((IEnumerable<object>) type.GetCustomAttributes(true)).FirstOrDefault<object>(
                    (Func<object, bool>) (x => x is WindowMetaAttribute));
            if (windowmeta == null)
            {
                Debug.LogError("error no WindowMetaAttribute");
                return;
            }

            editors = new Dictionary<string, List<System.Type>>();
            this.titleContent.text = windowmeta.WindowName;
            save_CurrentEditor =
                new PrefVariable(string.Format("{0}_{1}_CurrentEditor", (object) 27990, windowmeta.WindowName));
            instance = this;

            //加载所有代码
            this.LoadEditors();
            //选择一个meta做默认
            this.ShowFirstEditor();
            this.selectionColor = UnityEngine.Color.Lerp(UnityEngine.Color.red, UnityEngine.Color.white, 0.7f);
            this.bgColor = UnityEngine.Color.Lerp(GUI.backgroundColor, UnityEngine.Color.black, 0.3f);
        }


        private static bool style_IsInitialized;
        private static GUIStyle style_Exeption;
        private static GUIStyle tabButtonStyle;
        public static Rect currectEditorRect;


        /// <summary>
        /// 初始化样式
        /// </summary>
        protected virtual void InitializeStyles()
        {
            IPanel<T>.style_Exeption = new GUIStyle(GUI.skin.label);
            IPanel<T>.style_Exeption.normal.textColor = new UnityEngine.Color(0.5f, 0.0f, 0.0f, 1f);
            IPanel<T>.style_Exeption.alignment = TextAnchor.UpperLeft;
            IPanel<T>.style_Exeption.wordWrap = true;
            IPanel<T>.tabButtonStyle = new GUIStyle(EditorStyles.miniButton);
            IPanel<T>.tabButtonStyle.normal.textColor = UnityEngine.Color.white;
            IPanel<T>.tabButtonStyle.active.textColor = new UnityEngine.Color(1f, 0.8f, 0.8f, 1f);
            IPanel<T>.style_IsInitialized = true;
        }

        public new void Show()
        {
            this.Show(false);
        }

        /// <summary>
        /// 展示选中的功能界面
        /// </summary>
        /// <param name="editor"></param>
        public virtual void Show(IMetaEditor editor)
        {
            EditorGUI.FocusTextInControl("");
            if (!editor.Initialize())
                return;
            this.current_editor = editor;
            this.save_CurrentEditor.String = editor.GetType().FullName;
            this.editorAttribute =
                (TabAttribute)
                ((IEnumerable<object>) editor.GetType().GetCustomAttributes(true)).FirstOrDefault<object>(
                    (Func<object, bool>) (x => x is TabAttribute));
            this.editorRender = new Action(this.current_editor.OnGUI);
        }

        /// <summary>
        /// 加载该界面的所有editor类型
        /// </summary>
        protected virtual void LoadEditors()
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

            typeList.RemoveAll((Predicate<System.Type>) (x =>
                ((IEnumerable<object>) x.GetCustomAttributes(true)).FirstOrDefault<object>(
                    (Func<object, bool>) (y => y is TabAttribute)) == null));
            typeList.RemoveAll((Predicate<System.Type>) (x =>
            {
                var t = (TabAttribute) ((IEnumerable<object>) x.GetCustomAttributes(true)).FirstOrDefault<object>(
                    (Func<object, bool>) (y => y is TabAttribute));
                return t.WindowName != this.titleContent.text;
            }));

            typeList.Sort((Comparison<System.Type>) ((a, b) =>
                ((TabAttribute) ((IEnumerable<object>) a.GetCustomAttributes(true)).FirstOrDefault<object>(
                    (Func<object, bool>) (x => x is TabAttribute))).Priority.CompareTo(
                    ((TabAttribute) ((IEnumerable<object>) b.GetCustomAttributes(true)).FirstOrDefault<object>(
                        (Func<object, bool>) (x => x is TabAttribute))).Priority)));
            this.editors.Clear();
            foreach (System.Type type3 in typeList)
            {
                TabGroupAttribute panelGroupAttribute =
                    (TabGroupAttribute) ((IEnumerable<object>) type3.GetCustomAttributes(true)).FirstOrDefault<object>(
                        (Func<object, bool>) (x => x is TabGroupAttribute));
                string key = panelGroupAttribute != null ? panelGroupAttribute.Group : "";
                if (!this.editors.ContainsKey(key))
                    this.editors.Add(key, new List<System.Type>());
                this.editors[key].Add(type3);
            }
        }

        /// <summary>
        /// 显示一个展示gui
        /// </summary>
        protected virtual void ShowFirstEditor()
        {
            if (!this.save_CurrentEditor.IsEmpty())
            {
                System.Type type1 = typeof(IMetaEditor);
                System.Type type2 = typeof(MetaEditor<>);
                string editorName = this.save_CurrentEditor.String;
                System.Type type3 = this.editors.Values
                    .SelectMany<List<System.Type>, System.Type>(
                        (Func<List<System.Type>, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) x))
                    .FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => x.FullName == editorName));
                if (type1.IsAssignableFrom(type3) && type3 != type1 && type3 != type2)
                {
                    this.Show((IMetaEditor) Activator.CreateInstance(type3));
                    return;
                }
            }

            System.Type type = this.editors.Values
                .SelectMany<List<System.Type>, System.Type
                >((Func<List<System.Type>, IEnumerable<System.Type>>) (x => (IEnumerable<System.Type>) x))
                .FirstOrDefault<System.Type>((Func<System.Type, bool>) (x =>
                    ((IEnumerable<object>) x.GetCustomAttributes(true)).FirstOrDefault<object>(
                        (Func<object, bool>) (y => y is DefaultTabAttribute)) != null));
            if (type == null)
                return;
            this.Show((IMetaEditor) Activator.CreateInstance(type));
        }

        private void OnGUI()
        {
            try
            {
                CoreStyles.Update();
                if (!IPanel<T>.style_IsInitialized)
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
                    using (new GUIHelper.Vertical(CoreStyles.tabArea, new GUILayoutOption[2]
                    {
                        GUILayout.Width(150f),
                        GUILayout.ExpandHeight(true)
                    }))
                    {
                        this.tabsScroll = EditorGUILayout.BeginScrollView(this.tabsScroll);
                        this.DrawTabs();
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndScrollView();


                        Rect rect = EditorGUILayout.BeginVertical(CoreStyles.tabArea, GUILayout.ExpandWidth(true),
                            GUILayout.ExpandHeight(true));
                        this.editorScroll = EditorGUILayout.BeginScrollView(this.editorScroll);
                        if (this.current_editor != null && this.editorRender != null)
                        {
                            if (this.editorAttribute != null)
                                this.DrawTitle(this.editorAttribute.Title);
                            try
                            {
                                if (EditorApplication.isCompiling)
                                {
                                    GUILayout.Label("Compiling...", CoreStyles.centeredMiniLabel,
                                        GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                                }
                                else
                                {
                                    if (Event.current.type == EventType.Repaint)
                                        IPanel<T>.currectEditorRect = rect;
                                    this.editorRender();
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.LogException(ex);
                                GUILayout.Label(ex.ToString(), IPanel<T>.style_Exeption);
                            }
                        }
                        else
                            GUILayout.Label("Nothing selected", CoreStyles.centeredMiniLabel,
                                GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

                        EditorGUILayout.EndScrollView();
                    }
                }
            }
            catch (ArgumentException ex)
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
                TabAttribute tabAttribute =
                    (TabAttribute) ((IEnumerable<object>) type.GetCustomAttributes(true)).FirstOrDefault<object>(
                        (Func<object, bool>) (x => x is TabAttribute));
                if (tabAttribute != null && this.DrawTabButton(tabAttribute))
                    this.Show((IMetaEditor) Activator.CreateInstance(type));
            }
        }

        private bool DrawTabButton(TabAttribute tabAttribute)
        {
            bool flag = false;
            if (tabAttribute != null)
            {
                using (new GUIHelper.BackgroundColor(
                    this.editorAttribute == null || !this.editorAttribute.Match((object) tabAttribute)
                        ? UnityEngine.Color.white
                        : this.selectionColor))
                {
                    using (new GUIHelper.ContentColor(CoreStyles.centeredMiniLabel.normal.textColor))
                        flag = GUILayout.Button(new GUIContent(tabAttribute.Title), IPanel<T>.tabButtonStyle,
                            GUILayout.ExpandWidth(true));
                }

                if (this.editorAttribute != null && this.editorAttribute.Match((object) tabAttribute) &&
                    this.editorRender == null)
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
            if (!((UnityEngine.Object) instance != (UnityEngine.Object) null))
                return;
            instance.editorScroll = new Vector2(0.0f, position);
        }

        public static IMetaEditor GetCurrentEditor()
        {
            if (instance == (UnityEngine.Object) null)
                return (IMetaEditor) null;
            return (instance).current_editor;
        }

        public void Show(string editorName)
        {
            System.Type type = this.editors
                .SelectMany<KeyValuePair<string, List<System.Type>>, System.Type>(
                    (Func<KeyValuePair<string, List<System.Type>>, IEnumerable<System.Type>>) (x =>
                        (IEnumerable<System.Type>) x.Value))
                .FirstOrDefault<System.Type>((Func<System.Type, bool>) (x => x.Name == editorName));
            if (type == null)
                return;
            this.Show((IMetaEditor) Activator.CreateInstance(type));
        }
    }
}