using System;

namespace KCFramework.Editor
{
    public class TabGroupAttribute : Attribute
    {
        private string group;

        public TabGroupAttribute(string group)
        {
            this.group = group;
        }

        public string Group
        {
            get { return this.group; }
        }
    }
}