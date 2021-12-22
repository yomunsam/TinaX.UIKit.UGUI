using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Page.Navigator;
using TinaX.UIKit.UGUI.Page;
using TinaX.XComponent.Warpper.ReflectionProvider;

namespace TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders
{
#nullable enable
    /// <summary>
    /// 通过页面导航器打开UGUI的构建器
    /// </summary>
    public class PageNavigatorOpenUGUIBuilder
    {
        private readonly IPageNavigator<UGUIPage, OpenUGUIArgs> m_Navigator;
        private readonly OpenUGUIArgs m_OpenUGuiArgs;

        public PageNavigatorOpenUGUIBuilder(IPageNavigator<UGUIPage, OpenUGUIArgs> navigator, string pageUri)
        {
            this.m_Navigator = navigator;
            m_OpenUGuiArgs = new OpenUGUIArgs(pageUri);
        }

        public PageNavigatorOpenUGUIBuilder SetPageUri(string pageUri)
        {
            this.m_OpenUGuiArgs.PageUri = pageUri;
            return this;
        }

        public PageNavigatorOpenUGUIBuilder SetController(UGUIPageController controller)
        {
            this.m_OpenUGuiArgs.PageController = controller;
            return this;
        }

        public PageNavigatorOpenUGUIBuilder SetDisplayMessageArgs(params object[] args)
        {
            this.m_OpenUGuiArgs.UIDisplayArgs = args;
            return this;
        }

        /// <summary>
        /// 设置xBehaviour包装器的反射提供者
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public PageNavigatorOpenUGUIBuilder SetXBehaviourWrapperReflectionProvider(IWrapperReflectionProvider provider)
        {
            this.m_OpenUGuiArgs.XBehaviourWrapperReflectionProvider = provider;
            return this;
        }

        /// <summary>
        /// 启用UI背景遮罩
        /// </summary>
        /// <param name="useMask"></param>
        /// <returns></returns>
        public PageNavigatorOpenUGUIBuilder SetUseBackgroundMask(bool useMask = true)
        {
            this.m_OpenUGuiArgs.UseBackgroundMask = useMask;
            return this;
        }


        public UniTask<UGUIPage> OpenUIAsync(CancellationToken cancellationToken = default)
        {
            return m_Navigator.OpenUIAsync(m_OpenUGuiArgs, cancellationToken);
        }
    }
#nullable restore
}
