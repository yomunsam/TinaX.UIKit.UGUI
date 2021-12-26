using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.MultipleDisplay;
using TinaX.UIKit.UGUI.Page;
using TinaX.XComponent.Warpper.ReflectionProvider;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders
{
#nullable enable

    /// <summary>
    /// 通过 IUIKitUGUI 接口打开UGUI的构建器
    /// </summary>
    public class UGUIKitOpenUIBuilder
    {

        private readonly OpenUGUIArgs m_OpenUGuiArgs;
        private readonly IUGUIKit m_UGUIKit;

        public UGUIKitOpenUIBuilder(IUGUIKit uiKitUGUI, string pageUri)
        {
            m_OpenUGuiArgs = new OpenUGUIArgs(pageUri);
            this.m_UGUIKit = uiKitUGUI;
        }

        public UGUIKitOpenUIBuilder SetPageUri(string pageUri)
        {
            this.m_OpenUGuiArgs.PageUri = pageUri;
            return this;
        }

        public UGUIKitOpenUIBuilder SetController(UGUIPageController controller)
        {
            this.m_OpenUGuiArgs.PageController = controller;
            return this;
        }

        public UGUIKitOpenUIBuilder SetDisplayMessageArgs(params object?[] args)
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
        public UGUIKitOpenUIBuilder SetXBehaviourWrapperReflectionProvider(IWrapperReflectionProvider provider)
        {
            this.m_OpenUGuiArgs.XBehaviourWrapperReflectionProvider = provider;
            return this;
        }

        /// <summary>
        /// 设置UI背景遮罩
        /// </summary>
        /// <param name="useMask"></param>
        /// <returns></returns>
        public UGUIKitOpenUIBuilder SetUseBackgroundMask(bool useMask = true)
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
        public UGUIKitOpenUIBuilder SetUseBackgroundMask(bool useMask, bool closeByMask, Color? maskColor = null)
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
        public UGUIKitOpenUIBuilder SetCloseByBackgroundMask(bool closeByMask = true)
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
        public UGUIKitOpenUIBuilder SetBackgroundMaskColor(Color color)
        {
            if (m_OpenUGuiArgs.PushToGroupArgs == null)
                m_OpenUGuiArgs.PushToGroupArgs = new Page.Group.PushUGUIPageArgs();
            m_OpenUGuiArgs.PushToGroupArgs.BackgroundMaskColor = color;
            return this;
        }

        public UGUIKitOpenUIBuilder SetDisplay(DisplayIndex displayIndex)
        {
            m_OpenUGuiArgs.DisplayIndex = displayIndex;
            return this;
        }

        public async UniTask<IUGUIPage> OpenUIAsync(CancellationToken cancellationToken = default)
        {
            return await m_UGUIKit.OpenUIAsync(m_OpenUGuiArgs, cancellationToken);
        }

    }
}
