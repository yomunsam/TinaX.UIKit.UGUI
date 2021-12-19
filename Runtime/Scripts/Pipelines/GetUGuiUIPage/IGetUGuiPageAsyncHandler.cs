using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage
{
    public interface IGetUGuiPageAsyncHandler
    {
        string HandlerName { get; }

        UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken);
    }
}
