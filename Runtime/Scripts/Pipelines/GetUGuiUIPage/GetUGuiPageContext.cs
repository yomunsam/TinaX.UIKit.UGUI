using TinaX.Container;
using TinaX.Services;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage
{
#nullable enable

    /// <summary>
    /// 获取 uGUI UIPage 管道的上下文
    /// </summary>
    public class GetUGuiPageContext
    {
        public GetUGuiPageContext(IServiceContainer services, IAssetService assetService, IUIKit uikit, IUIKitUGUI uikitUGUI)
        {
            this.Services = services;
            this.AssetService = assetService;
            this.UIKit = uikit;
            this.UIKitUGUI = uikitUGUI;
        }

        /// <summary>
        /// 是否终断Pipeline的标记
        /// </summary>
        public bool BreakPipeline { get; set; } = false;

        /// <summary>
        /// 终断Pipeline流程
        /// </summary>
        public void Break() => BreakPipeline = true;


        public IServiceContainer Services { get; set; }

        public IAssetService AssetService { get; set; }

        public IUIKit UIKit { get; set; }
        public IUIKitUGUI UIKitUGUI { get; set; }

    }
#nullable restore

}
