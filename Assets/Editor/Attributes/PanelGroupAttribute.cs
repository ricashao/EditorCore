using System;

namespace XCFramework.Editor
{
    public class PanelGroupAttribute : Attribute
    {
        private string group;

        public PanelGroupAttribute(string group)
        {
            this.group = group;
        }

        public string Group
        {
            get { return this.group; }
        }
    }
}