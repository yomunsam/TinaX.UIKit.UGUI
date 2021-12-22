using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers
{
#nullable enable
    /// <summary>
    /// 加载View资产（也就是prefab）
    /// </summary>
    public class LoadViewAssetAsyncHandler : IGetUGuiPageAsyncHandler
    {
        public string HandlerName => HandlerNameConsts.LoadViewAsset;

        public async UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            if(payload.Args.LoadPrefab)
            {
                if (payload.UIPage != null)
                    await payload.UIPage.ReadyViewAsync(cancellationToken);
            }
        }
    }
#nullable restore
}
