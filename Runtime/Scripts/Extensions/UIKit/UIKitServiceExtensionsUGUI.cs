using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Helper;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit
{
#nullable enable
    /// <summary>
    /// 对 UIKitService （IUIKit接口）的扩展方法
    /// </summary>
    public static class UIKitServiceExtensionsUGUI
    {
        //private static string _Scheme = $"{UIKitUGUIConsts.SchemeName}://";
        //private static string _SchemeLower = _Scheme.ToLower();

        public static UniTask<UGUIPage> OpenUGUIAsync(this IUIKit uikit, string pageUri, UGUIPageController controller, CancellationToken cancellationToken = default)
            => uikit.OpenUGUIAsync(new OpenUGUIArgs(PageUriHelper.AddSchemeIfNot(pageUri)) { PageController = controller }, cancellationToken);

        public static UniTask<UGUIPage> OpenUGUIAsync(this IUIKit uikit, string pageUri, CancellationToken cancellationToken = default)
            => uikit.OpenUGUIAsync(new OpenUGUIArgs(PageUriHelper.AddSchemeIfNot(pageUri)), cancellationToken);

        public static async UniTask<UGUIPage> OpenUGUIAsync(this IUIKit uikit, OpenUGUIArgs args, CancellationToken cancellationToken = default)
        {
            var uguikit = uikit.GetUIKitUGUI();
            var page = await uguikit.GetUIPageAsync(args, cancellationToken);
            uguikit.PushScreenUI(page, args.PushToGroupArgs?.DisplayMessageArgs);
            return page;
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
