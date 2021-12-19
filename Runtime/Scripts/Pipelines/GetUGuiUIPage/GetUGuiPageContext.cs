using TinaX.Container;
using TinaX.Services;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage
{
#nullable enable

    /// <summary>
    /// 获取 uGUI UIPage 管道的上下文
    /// </summary>
    public class GetUGuiPageContext
    {
        public GetUGuiPageContext(IServiceContainer services, IAssetService assetService)
        {
            this.Services = services;
            this.AssetService = assetService;
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

    }
#nullable restore

}
