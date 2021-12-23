using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Page;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.Page.View;
using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.Page.Group;
using TinaX.UIKit.UGUI.Page.View;
using TinaX.UIKit.UGUI.Services;
using TinaX.XComponent.Warpper.ReflectionProvider;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Page
{
#nullable enable

    public class UGUIPage : UIPageBase
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

        public UGUIPageController? UGUIController => m_UGuiPageController;

        public IWrapperReflectionProvider? XBehaviourWrapperReflectionProvider => m_XBehaviourWrapperReflectionProvider;

        public UGUIPageGroup? ParentUGUI => m_ParentUGUI;

        public UIKitUGUICanvas? UGUICanvas => m_ParentUGUI?.UGUICanvas;

        public override int PageSize => 10; //UGUI每页Order占用10，以方便放一些东西

        /// <summary>
        /// 该UIPage是否使用遮罩
        /// 这个属性注意仅由Group管理
        /// </summary>
        public bool UseMask { get; set; } 
        public bool CloseByMask { get; set; }
        public Color? MaskColor { get; set; }

        public virtual float RemoveMaskDelayTime => 0; //Todo: UI动画等原因需要延迟

        //------------公开方法---------------------------------------------------------------------------------------------------------------------------

        public override async UniTask ReadyViewAsync(CancellationToken cancellationToken = default)
        {
            if(m_Content == null || m_uGuiViewProvider == null)
            {
                m_uGuiPageView = await m_uGuiViewProvider!.GetPageViewGenericAsync(this, cancellationToken);
                m_Content = m_uGuiPageView;
            }
        }

        public override void DisplayView(object?[]? args)
        {
            m_uGuiPageView?.Display(args);
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

        public override void ClosePage(params object?[]? closeMessageArgs)
        {
            //计算延迟，如UI关闭动画，需要等UI动画结束之后再处理遮罩等等
            //Todo ...

            //首先把自己从Group中移除
            m_Parent?.Remove(this);

            //执行销毁
            this.DestroyPage();
        }

        public override void DestroyPage()
        {
            //throw new System.NotImplementedException();
        }
    }


#nullable restore
}
