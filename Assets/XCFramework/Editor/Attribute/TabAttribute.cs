using System;

namespace XCFramework.Editor
{
    public class TabAttribute : Attribute
    {
        private float priority;
        private string title;
        private string windowName;
        public TabAttribute(string windowName, string title, int priority = 0)
        {
            this.title = title;
            this.priority = (float) priority;
            this.windowName = windowName;
        }
    }
}