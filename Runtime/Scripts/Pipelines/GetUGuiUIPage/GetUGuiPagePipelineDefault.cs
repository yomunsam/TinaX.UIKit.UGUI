using System;
using TinaX.Systems.Pipeline;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage
{
    public static class GetUGuiPagePipelineDefault
    {
        public static XPipeline<IGetUGuiPageAsyncHandler> CreateAsyncDefault()
        {
            var pipeline = new XPipeline<IGetUGuiPageAsyncHandler>();
            AppendDefaultAsyncPipeline(ref pipeline);
            return pipeline;
        }

        /// <summary>
        /// 追加默认的Pipeline
        /// 如果给的是空的Pipeline，就相当于设置Pipeline
        /// </summary>
        /// <param name="pipeline"></param>
        public static void AppendDefaultAsyncPipeline(ref XPipeline<IGetUGuiPageAsyncHandler> pipeline)
        {
            if (pipeline == null)
                throw new ArgumentNullException(nameof(pipeline));

            //处理View资产加载路径
            pipeline.AddLast(new ProcessViewAssetLoadPathAsyncHandler());

            //创建一个View提供者
            pipeline.AddLast(new MakeViewProviderAsyncHandler());

            //创建UIPage
            pipeline.AddLast(new MakeUIPageAsyncHandler());

            //加载View Prefab
            pipeline.AddLast(new LoadViewAssetAsyncHandler());
        }
    }
}
