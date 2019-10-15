using UnityEngine;

namespace XCFramework.Editor
{
    public abstract class MetaEditor : MetaEditor<MonoBehaviour>
    {
        public override MonoBehaviour FindTarget()
        {
            return (MonoBehaviour) null;
        }
    }
}