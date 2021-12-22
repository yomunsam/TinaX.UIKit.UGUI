using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit
{
#nullable enable
    /// <summary>
    /// 对 UIKitService （IUIKit接口）的扩展方法
    /// </summary>
    public static class UIKitServiceExtensions
    {
        private static string _Scheme = $"{UIKitUGUIConsts.SchemeName}://";
        private static string _SchemeLower = _Scheme.ToLower();

        public static async UniTask<UGUIPage> OpenUGUIAsync(this IUIKit uikit, string pageUri, UGUIPageController controller, CancellationToken cancellationToken = default)
        {
            var args = new GetUIPageArgs(pageUri.ToLower().StartsWith(_SchemeLower) ? pageUri : $"{_Scheme}{pageUri}")
            {
                PageController = controller,
            };
            var page = await uikit.GetUIPageAsync(args, cancellationToken);
            var uguiPage = page as UGUIPage;
            if (uguiPage == null)
                throw new XException($"UIPage \"{page}\"({page.GetType().FullName}) not valid UGUI Page");
            var uguikit = uikit.GetUIKitUGUI();
            uguikit.PushScreenUI(uguiPage);
            return uguiPage;
        }

        public static async UniTask<UGUIPage> OpenUGUIAsync(this IUIKit uikit, string pageUri, CancellationToken cancellationToken = default)
        {
            var args = new GetUIPageArgs(pageUri.ToLower().StartsWith(_SchemeLower) ? pageUri : $"{_Scheme}{pageUri}")
            {
            };
            var page = await uikit.GetUIPageAsync(args, cancellationToken);
            var uguiPage = page as UGUIPage;
            if (uguiPage == null)
                throw new XException($"UIPage \"{page}\"({page.GetType().FullName}) not valid UGUI Page");
            var uguikit = uikit.GetUIKitUGUI();
            uguikit.PushScreenUI(uguiPage);
            return uguiPage;
        }


        /// <summary>
        /// Get UIKit(uGUI)
        /// </summary>
        /// <param name="uikit"></param>
        /// <returns></returns>
        public static IUIKitUGUI GetUIKitUGUI(this IUIKit uikit)
            => uikit.Services.Get<IUIKitUGUI>();

    }
#nullable restore
}
