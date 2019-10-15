using System;

namespace KCFramework.Editor
{
    public class TabAttribute : Attribute
    {
        private float priority;
        private string title;
        private string windowName;
        public TabAttribute(string windowName, string title, int priority = 0)
        {
            this.windowName = windowName;
            this.title = title;
            this.priority = (float) priority;
        }
        
        public string Title
        {
            get { return this.title; }
        }

        public float Priority
        {
            get { return this.priority; }
        }
        
        public string WindowName
        {
            get { return this.windowName; }
        }
    }
}