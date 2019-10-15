using System;

namespace KCFramework.Editor
{
    [Serializable]
    public class TreeFolder
    {
        public string path = "";
        public string name = "";

        public string fullPath
        {
            get
            {
                if (this.path.Length > 0)
                    return this.path + "/" + this.name;
                return this.name;
            }
            set
            {
                int length = value.LastIndexOf('/');
                if (length >= 0)
                {
                    this.path = value.Substring(0, length);
                    this.name = value.Substring(length + 1, value.Length - length - 1);
                }
                else
                {
                    this.path = "";
                    this.name = value;
                }
            }
        }

        public override int GetHashCode()
        {
            return this.fullPath.GetHashCode();
        }
    }
}