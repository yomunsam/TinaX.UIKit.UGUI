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

        protected Transform? m_Transform { get; set; }

        protected IWrapperReflectionProvider? m_XBehaviourWrapperReflectionProvider { get; set; }


        public UGUIPage(IPageViewProvider<UGUIPageView, UGUIPage> viewProvider) : base(viewProvider)
        {
            this.m_uGuiViewProvider = viewProvider;
        }

        public UGUIPage(IPageViewProvider<UGUIPageView, UGUIPage> viewProvider, UGUIPageController? controller, IWrapperReflectionProvider? xBehaviourWrapperReflectionProvider = null) : base(viewProvider, controller)
        {
            this.m_uGuiViewProvider = viewProvider;
            this.m_UGuiPageController = controller;
            m_XBehaviourWrapperReflectionProvider = xBehaviourWrapperReflectionProvider;
        }

        public Transform? Transform => m_Transform;

        public UGUIPageController? UGUIController => m_UGuiPageController;

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider => m_XBehaviourWrapperReflectionProvider;


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

        public override void OnJoinGroup(UIPageGroup group)
        {
            throw new System.NotImplementedException();
        }

        public virtual void OnJoinUGUIGroup(UGUIPageGroup group)
        {
            m_Transform = group.RootTransform;
            this.DisplayView();
        }

        public override void OnLeaveGroup(UIPageGroup group)
        {
            throw new System.NotImplementedException();
        }
    }


#nullable restore
}
