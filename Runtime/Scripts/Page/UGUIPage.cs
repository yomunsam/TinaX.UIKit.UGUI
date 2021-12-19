using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.Page.View;
using TinaX.UIKit.UGUI.Page.Group;
using TinaX.UIKit.UGUI.Page.View;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page
{
#nullable enable

    public class UGUIPage : UIPageBase
    {
        protected IPageViewProvider<UGUIPageView, UGUIPage> m_uGUIViewProvider { get; set; }

        protected UGUIPageView? m_uGUIPageView { get; set; }

        protected Transform? m_Transform { get; set; }

        public UGUIPage(IPageViewProvider<UGUIPageView, UGUIPage> viewProvider) : base(viewProvider)
        {
            this.m_uGUIViewProvider = viewProvider;
        }

        public Transform? Transform => m_Transform;

        public override async UniTask ReadyViewAsync(CancellationToken cancellationToken = default)
        {
            if(m_Content == null || m_uGUIViewProvider == null)
            {
                m_uGUIPageView = await m_uGUIViewProvider!.GetPageViewGenericAsync(this, cancellationToken);
                m_Content = m_uGUIPageView;
            }
        }

        public override void DisplayView()
        {
            m_uGUIPageView?.Display();
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
