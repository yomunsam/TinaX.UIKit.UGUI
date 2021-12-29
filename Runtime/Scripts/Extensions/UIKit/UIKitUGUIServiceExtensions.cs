using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit
{
#nullable enable
    public static class UIKitUGUIServiceExtensions
    {
        /// <summary>
        /// 在默认的屏幕空间 异步打开UI（UGUI）
        /// </summary>
        /// <param name="uguikit"></param>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<IUGUIPage> OpenUIAsync(this IUGUIKit uguikit, OpenUGUIArgs args, CancellationToken cancellationToken = default) //这里应该是IUIKitUGUI接口打开UI的总方法
        {
            var page = await uguikit.GetUIPageAsync(args, cancellationToken); //加载UI Page
            uguikit.PushScreenUI(page, args.DisplayIndex, args.PushToGroupArgs); //推到默认的屏幕空间
            return page;
        }

        /// <summary>
        /// 创建一个打开UI的构建器
        /// </summary>
        public static UGUIKitOpenUIBuilder CreateOpenUI(this IUGUIKit uguikit, string pageUri)
            => new UGUIKitOpenUIBuilder(uguikit, pageUri);
    }
#nullable restore
}
