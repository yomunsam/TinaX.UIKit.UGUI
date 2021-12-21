using TinaX.UIKit.UGUI.Page.Group;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Canvas
{
#nullable enable
    /// <summary>
    /// UIKit uGUI Canvas 挂在组件 基础类
    /// </summary>
    public class UIKitUGUICanvasComponentBase : MonoBehaviour //被Services创建并管理的Canvas挂载这个
    {
        protected UIKitUGUICanvas? m_UIKitCanvas;


        public UIKitUGUICanvas? UIKitCanvas => m_UIKitCanvas;

        protected void Awake()
        {
            
        }

        public virtual void EnsureUIKitCanvas(string? canvasName = null)
        {
            var defaultCanvasName = $"UIKit.UGUI-{this.gameObject.name}";
            if (m_UIKitCanvas == null)
                m_UIKitCanvas = new UIKitUGUICanvas(new UGUIPageGroup(this.transform, "Root"), canvasName ?? defaultCanvasName);
        }

        [ContextMenu("Print UI Tree")]
        public void PrintUITree()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Valid only when playing");
                return;
            }
#endif
            this.m_UIKitCanvas?.RootGroup.PrintUITree();
        }
    }
#nullable restore
}
