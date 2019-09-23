using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCFramework.Editor
{
    public class IPanel : EditorWindow
    {
        protected virtual void OnEnable()
        {
        }

        public virtual void Show()
        {
            this.Show(false);
        }

        public virtual void Show(IMetaEditor editor)
        {
            
        }

        protected virtual void OnFocus()
        {
            
        }

        protected virtual void LoadEditors()
        {
            
        }

        protected virtual void ShowFirstEditor()
        {
            
        }
        
    }
}
