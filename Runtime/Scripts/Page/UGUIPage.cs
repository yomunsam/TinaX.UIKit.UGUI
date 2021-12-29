using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.Page.View;
using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.Page.BackgroundMask;
using TinaX.UIKit.UGUI.Page.Group;
using TinaX.UIKit.UGUI.Page.View;
using TinaX.XComponent.Warpper.ReflectionProvider;
using UniRx;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page
{
#nullable enable

    public class UGUIPage : UIPageBase, IUGUIPage, IBackgroundMaskInfo
    {
        //------------构造函数字段--------------------------------------------------------------------------------------------------------------

        protected IPageViewProvider<UGUIPageView, UGUIPage> m_uGuiViewProvider { get; set; }

        protected UGUIPageController? m_UGuiPageController { get; set; }

        protected IWrapperReflectionProvider? m_XBehaviourWrapperReflectionProvider { get; set; }


        //------------构造函数-----------------------------------------------------------------------------------------------------------------

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



        //------------内部字段--------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 这里应该是指View （也就是UI Prefab实例化之后的GameObject）的Transform
        /// </summary>
        protected Transform? m_Transform;

        protected UGUIPageGroup? m_ParentUGUI;

        protected UGUIPageView? m_uGuiPageView;

        //------------公开属性----------------------------------------------------------------------------------------------------------------------

        public Transform? Transform => m_Transform;

        public new UGUIPageController? Controller => m_UGuiPageController;

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider => m_XBehaviourWrapperReflectionProvider;

        public new UGUIPageGroup? Parent => m_ParentUGUI;

        public UIKitUGUICanvas? UGUICanvas => m_ParentUGUI?.UGUICanvas;

        public override int PageSize => 10; //UGUI每页Order占用10，以方便放一些东西

        /// <summary>
        /// 该UIPage是否使用遮罩
        /// 这个属性注意仅由Group管理
        /// </summary>
        public bool IsUseBackgroundMask { get; set; } 
        public bool CloseByBackgroundMask { get; set; }
        public Color? BackgroundMaskColor { get; set; }

        public virtual float RemoveMaskDelayTime => 0; //Todo: UI动画等原因需要延迟

        //------------公开方法---------------------------------------------------------------------------------------------------------------------------

        public override async UniTask ReadyViewAsync(CancellationToken cancellationToken = default)
        {
            if(m_Content == null || m_uGuiViewProvider == null)
            {
                m_uGuiPageView = await m_uGuiViewProvider!.GetPageViewAsync(this, cancellationToken);
                m_Content = m_uGuiPageView;
            }
        }

        public override void DisplayView(object?[]? args)
        {
            m_uGuiPageView?.Display(args);

            //调用Actions
            InvokeOnDisplayedActions();
        }

        public override void HideView()
        {
            throw new System.NotImplementedException();
        }

        public override void OnJoinGroup(UIPageGroup group, object?[]? displayMessageArgs)
        {
            base.OnJoinGroup(group, displayMessageArgs);
            if (m_Parent is UGUIPageGroup uGUIPageGroup)
                m_ParentUGUI = uGUIPageGroup;
        }

        public virtual void OnJoinUGUIGroup(UGUIPageGroup group, object?[]? displayMessageArgs)
        {
            this.m_Parent = group;
            this.m_ParentUGUI = group; //我们在子类中存了同样的玩意，用意是避免频繁的装箱
            this.DisplayView(displayMessageArgs);
        }

        public override void OnLeaveGroup(UIPageGroup group)
        {
            base.OnLeaveGroup(group);
            this.m_ParentUGUI = null;
        }

        public void SetXBehaviourWrapperReflectionProvider(IWrapperReflectionProvider wrapperReflectionProvider) 
            => this.m_XBehaviourWrapperReflectionProvider = wrapperReflectionProvider;


        public void SetName(string name) 
            => m_Name = name;

        public void SetTransform(Transform transform) 
            => m_Transform = transform;

        public override void ClosePage(params object?[] closeMessageArgs)
        {
            if (m_Closed || m_Closing)
                return;

            m_Closing = true;

            InvokeOnClosedActions();

            //延迟，可能由于UI关闭动画等原因需要延迟执行真正的关闭操作
            TimeSpan delayTime = this.CloseDelayTime;
            if (delayTime != TimeSpan.Zero)
            {
                Observable.Timer(delayTime)
                    .Subscribe(_ => { DoClosePage(closeMessageArgs); })
                    .AddTo(this.m_Transform);
            }
            else
            {
                DoClosePage(closeMessageArgs);
            }
        }

        private void DoClosePage(object?[]? closeMessageArgs)
        {
            //开始执行实际的关闭页面工作

            //首先把自己从Group中移除
            m_Parent?.Remove(this);
            //发送个事件
            SendUIClosedMessage(closeMessageArgs);

            //销毁View
            if (m_uGuiPageView != null)
            {
                m_uGuiPageView.Destroy(); //Todo: 销毁延迟
            }
            m_uGuiPageView = null;
            m_Transform = null;

            m_OnDisplayedActions.Clear();
            m_OnClosedActions?.Clear();
            m_OnHiddenActions?.Clear();
            m_OnShowedActions?.Clear();
            m_OnActiveActions?.Clear();

            //关闭操作结束
            m_Closing = false;
            m_Closed = true;
        }



        #region UI消息

        public override bool SendUIDisplayMessage(object?[]? args)
        {
            if (base.SendUIDisplayMessage(args))
                return true;
            //如果基类方法里没成功的话，我们自己进行更多尝试
            if(m_uGuiPageView != null)
            {
                return m_uGuiPageView.SendUIDisplayMessage(args);
            }

            return false;
        }

        #endregion


    }


#nullable restore
}
