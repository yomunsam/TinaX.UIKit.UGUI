using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.MultipleDisplay;
using TinaX.UIKit.UGUI.Page;
using TinaX.XComponent.Warpper.ReflectionProvider;
using UnityEngine;

#nullable enable

namespace TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders
{
    public class UIKitOpenUIBuilder
    {
        private readonly IUIKit m_UIKit;
        private readonly OpenUGUIArgs m_OpenUGuiArgs;

        public UIKitOpenUIBuilder(IUIKit uiKit, string pageUri)
        {
            this.m_UIKit = uiKit;
            m_OpenUGuiArgs = new OpenUGUIArgs(pageUri);
        }

        public UIKitOpenUIBuilder SetPageUri(string pageUri)
        {
            this.m_OpenUGuiArgs.PageUri = pageUri;
            return this;
        }

        public UIKitOpenUIBuilder SetController(UGUIPageController controller)
        {
            this.m_OpenUGuiArgs.PageController = controller;
            return this;
        }

        public UIKitOpenUIBuilder SetDisplayMessageArgs(params object?[] args)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.DisplayMessageArgs = args;
            return this;
        }

        /// <summary>
        /// 设置xBehaviour包装器的反射提供者
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public UIKitOpenUIBuilder SetXBehaviourWrapperReflectionProvider(IWrapperReflectionProvider provider)
        {
            this.m_OpenUGuiArgs.XBehaviourWrapperReflectionProvider = provider;
            return this;
        }

        /// <summary>
        /// 设置UI背景遮罩
        /// </summary>
        /// <param name="useMask"></param>
        /// <returns></returns>
        public UIKitOpenUIBuilder SetUseBackgroundMask(bool useMask = true)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.UseBackgroundMask = useMask;
            return this;
        }

        /// <summary>
        /// 设置UI背景遮罩
        /// </summary>
        /// <param name="useMask">是否使用遮罩</param>
        /// <param name="closeByMask">点击遮罩可关闭UI（点击空白处关闭）</param>
        /// <param name="maskColor">指定背景遮罩颜色</param>
        /// <returns></returns>
        public UIKitOpenUIBuilder SetUseBackgroundMask(bool useMask, bool closeByMask, Color? maskColor = null)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.UseBackgroundMask = useMask;
            m_OpenUGuiArgs.PushToGroupArgs.CloseByMask = closeByMask;
            m_OpenUGuiArgs.PushToGroupArgs.BackgroundMaskColor = maskColor;
            return this;
        }

        /// <summary>
        /// UI页可否在点击遮罩时关闭（点击空白处关闭）
        /// </summary>
        /// <param name="closeByMask"></param>
        /// <returns></returns>
        public UIKitOpenUIBuilder SetCloseByBackgroundMask(bool closeByMask = true)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.CloseByMask = closeByMask;
            return this;
        }

        /// <summary>
        /// 设置UI遮罩颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public UIKitOpenUIBuilder SetBackgroundMaskColor(Color color)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.BackgroundMaskColor = color;
            return this;
        }

        public UIKitOpenUIBuilder SetDisplayedAnimation(string simpleAnimationName)
        {
            m_OpenUGuiArgs.DisplayedSimpleAnimationName = simpleAnimationName;
            return this;
        }

        public UIKitOpenUIBuilder SetClosedAnimation(string simpleAnimationName)
        {
            m_OpenUGuiArgs.DisplayedSimpleAnimationName = simpleAnimationName;
            return this;
        }

        public UIKitOpenUIBuilder SetDisplay(DisplayIndex displayIndex)
        {
            m_OpenUGuiArgs.DisplayIndex = displayIndex;
            return this;
        }

        

        public UniTask<IUGUIPage> OpenUIAsync(CancellationToken cancellationToken = default)
        {
            return UIKitServiceExtensionsUGUI.OpenUGUIAsync(m_UIKit, m_OpenUGuiArgs, cancellationToken);
        }

    }
}
