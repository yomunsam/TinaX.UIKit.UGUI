using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.Page.View;
using TinaX.UIKit.UGUI.Page.Group;
using TinaX.UIKit.UGUI.Page.View;
using TinaX.XComponent.Warpper.ReflectionProvider;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page
{
#nullable enable

    public class UGUIPage : UIPageBase
    {
        protected IPageViewProvider<UGUIPageView, UGUIPage> m_uGuiViewProvider { get; set; }

        protected UGUIPageView? m_uGuiPageView { get; set; }

        protected UGUIPageController? m_UGuiPageController { get; set; }

        protected UGUIPageGroup? m_ParentUGUI { get; set; }

        /// <summary>
        /// 这里应该是指View （也就是UI Prefab实例化之后的GameObject）的Transform
        /// </summary>
        protected Transform? m_Transform { get; set; }

        protected IWrapperReflectionProvider? m_XBehaviourWrapperReflectionProvider { get; set; }



        public UGUIPage(string pageUri, IPageViewProvider<UGUIPageView, UGUIPage> viewProvider) : base(pageUri, viewProvider)
        {
            this.m_uGuiViewProvider = viewProvider;
        }

        public UGUIPage(string pageUri, IPageViewProvider<UGUIPageView, UGUIPage> viewProvider, UGUIPageController? controller, IWrapperReflectionProvider? xBehaviourWrapperReflectionProvider = null)
            : base(pageUri, viewProvider, controller)
        {
            this.m_uGuiViewProvider = viewProvider;
            this.m_UGuiPageController = controller;
            m_XBehaviourWrapperReflectionProvider = xBehaviourWrapperReflectionProvider;
        }

        public Transform? Transform => m_Transform;

        public UGUIPageController? UGUIController => m_UGuiPageController;

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider => m_XBehaviourWrapperReflectionProvider;

        public UGUIPageGroup? ParentUGUI => m_ParentUGUI;

        public override async UniTask ReadyViewAsync(CancellationToken cancellationToken = default)
        {
            if(m_Content == null || m_uGuiViewProvider == null)
            {
                m_uGuiPageView = await m_uGuiViewProvider!.GetPageViewGenericAsync(this, cancellationToken);
                m_Content = m_uGuiPageView;
            }
        }

        public override void DisplayView()
        {
            m_uGuiPageView?.Display();
        }

        public override void HideView()
        {
            throw new System.NotImplementedException();
        }

        

        public virtual void OnJoinUGUIGroup(UGUIPageGroup group)
        {
            this.m_Parent = group;
            this.m_ParentUGUI = group;
            this.DisplayView();
        }

        


        public void SetName(string name) 
            => m_Name = name;

        public void SetTransform(Transform transform) 
            => m_Transform = transform;
    }


#nullable restore
}
