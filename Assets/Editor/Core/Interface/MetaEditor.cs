using UnityEngine;

namespace XCFramework.Editor
{
    public abstract class MetaEditor<T> : IMetaEditor where T : MonoBehaviour
    {
        private T target;
        public T metaTarget
        {
            get
            {
                if ((Object) this.target == (Object) null)
                    this.target = this.FindTarget();
                return this.target;
            }
        }
        
        public static void Repaint()
        {
            
        }
        
        public abstract T FindTarget();
        
        public abstract void OnGUI();

        public abstract bool Initialize();
        
        public virtual void OnFocus()
        {
        }
    }
}