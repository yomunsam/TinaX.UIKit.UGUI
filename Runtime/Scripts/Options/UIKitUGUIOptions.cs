using TinaX.Systems.Pipeline;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;

namespace TinaX.UIKit.UGUI.Options
{
    public class UIKitUGUIOptions
    {
        public string ConfigAssetLoadPath { get; set; } = UIKitUGUIConsts.DefaultConfigAssetName;

        public readonly XPipeline<IGetUGuiPageAsyncHandler> GetUGUIPageAsyncPipeline = GetUGuiPagePipelineDefault.CreateAsyncDefault();

        /// <summary>
        /// 启用多个Display（如可用）
        /// </summary>
        public bool EnableMultipleDisplay { get; set; } = false;

        /// <summary>
        /// 在编辑器中强制认为有多个显示器
        /// </summary>
        public int? ForceDisplayNumInEditor { get; set; }
    }
}
