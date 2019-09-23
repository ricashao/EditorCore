using UnityEditor;

namespace XCFramework.Editor
{
    public class PrefVariable
    {
        private string key = "";

        public PrefVariable(string _key)
        {
            this.key = _key;
        }

        public int Int
        {
            get { return EditorPrefs.GetInt(this.key); }
            set { EditorPrefs.SetInt(this.key, value); }
        }

        public float Float
        {
            get { return EditorPrefs.GetFloat(this.key); }
            set { EditorPrefs.SetFloat(this.key, value); }
        }

        public string String
        {
            get { return EditorPrefs.GetString(this.key); }
            set { EditorPrefs.SetString(this.key, value); }
        }

        public bool Bool
        {
            get { return EditorPrefs.GetBool(this.key); }
            set { EditorPrefs.SetBool(this.key, value); }
        }

        public bool IsEmpty()
        {
            return !EditorPrefs.HasKey(this.key);
        }

        public void Delete()
        {
            EditorPrefs.DeleteKey(this.key);
        }
    }
}