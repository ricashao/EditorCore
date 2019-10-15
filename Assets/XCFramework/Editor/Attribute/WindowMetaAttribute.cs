using System;

namespace KCFramework.Editor
{
    /// <summary>
    /// 管理window attribute
    /// </summary>
    public class WindowMetaAttribute : Attribute
    {
        private string windowName;

        public WindowMetaAttribute(string windowName)
        {
            this.windowName = windowName;
        }

        public string WindowName
        {
            get { return this.windowName; }
        }
    }
}