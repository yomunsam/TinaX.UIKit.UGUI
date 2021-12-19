using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers
{
    public class MakeUIPageAsyncHandler : IGetUGuiPageAsyncHandler
    {
        public string HandlerName => HandlerNameConsts.MakeUIPage;

        public UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            if (payload.ViewProvider == null)
                throw new XException("ViewProvider cannot be null");

            if(payload.Options.PageController != null && !(payload.Options.PageController is UGUIPageController))
            {
                throw new XException("PageController must be \"UGUIPageController\"");
            }
            var ugui_controller = payload.Options.PageController as UGUIPageController;
            payload.UIPage = new Page.UGUIPage(payload.Options.PageUri, payload.ViewProvider!, ugui_controller);
            return UniTask.CompletedTask;
        }
    }
}
