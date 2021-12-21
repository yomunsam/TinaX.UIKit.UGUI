using TinaX.Systems.Pipeline;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;

namespace TinaX.UIKit.UGUI.Options
{
    public class UIKitUGUIOptions
    {
        public string ConfigAssetLoadPath { get; set; } = UIKitUGUIConsts.DefaultConfigAssetName;

        public readonly XPipeline<IGetUGuiPageAsyncHandler> GetUGUIPageAsyncPipeline = GetUGuiPagePipelineDefault.CreateAsyncDefault();
    }
}
