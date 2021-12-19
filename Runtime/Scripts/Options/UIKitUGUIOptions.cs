using TinaX.Systems.Pipeline;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;

namespace TinaX.UIKit.UGUI.Options
{
    public class UIKitUGUIOptions
    {
        public readonly XPipeline<IGetUGuiPageAsyncHandler> GetUGUIPageAsyncPipeline = GetUGuiPagePipelineDefault.CreateAsyncDefault();
    }
}
