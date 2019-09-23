using System;
using UnityEngine;

namespace XCFramework.Editor
{
    public class PanelTabAttribute : Attribute
    {
        private float priority;

        private string title;
//        private Texture2D icon;

        public PanelTabAttribute(string title, int priority = 0)
        {
            this.title = title;
            this.priority = (float) priority;
        }

//        public PanelTabAttribute(string title, string icon, int priority = 0)
//        {
//            this.title = title;
//            this.priority = (float) priority;
//            if (string.IsNullOrEmpty(icon))
//                return;
//            this.icon = EditorIcons.GetBuildInIcon(icon);
//            if ((bool) ((UnityEngine.Object) this.icon))
//                return;
//            this.icon = EditorIcons.GetIcon(icon);
//        }

        public string Title
        {
            get { return this.title; }
        }

        public float Priority
        {
            get { return this.priority; }
        }

//        public Texture2D Icon
//        {
//            get
//            {
//                return this.icon;
//            }
//        }
    }
}