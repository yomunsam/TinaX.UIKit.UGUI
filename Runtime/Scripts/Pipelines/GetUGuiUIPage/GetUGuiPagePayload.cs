using TinaX.UIKit.Page.View;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.View;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage
{
#nullable enable
    public class GetUGuiPagePayload
    {
        public GetUGuiPagePayload(GetUGUIPageOptions options)
        {
            Options = options;
            PageUriLower = options.PageUri.ToLower();
        }

        public string PageUriLower { get; set; }

        public GetUGUIPageOptions Options { get; }

        /// <summary>
        /// 加载 uGUI View(也就是Prefab)的资产加载路径
        /// </summary>
        public string? ViewAssetLoadPath { get; set; }

        public IPageViewProvider<UGUIPageView, UGUIPage>? ViewProvider;

        public UGUIPage? UIPage { get; set; }

    }
#nullable restore
}
