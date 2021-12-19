using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.UIKit.UGUI.Page.View;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers
{
#nullable enable

    /// <summary>
    /// 构建 View提供者
    /// </summary>
    public class MakeViewProviderAsyncHandler : IGetUGuiPageAsyncHandler
    {
        public string HandlerName => HandlerNameConsts.MakeViewProvider;

        public UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            //在这一步之前我们应该已经得到了loadPath
            if (payload.ViewAssetLoadPath == null)
                throw new XException($"{nameof(payload.ViewAssetLoadPath)} cannot be null");
            payload.ViewProvider = new UGUIPageViewProvider(payload.ViewAssetLoadPath!, context.AssetService);

            return UniTask.CompletedTask;
        }
    }
#nullable restore
}
