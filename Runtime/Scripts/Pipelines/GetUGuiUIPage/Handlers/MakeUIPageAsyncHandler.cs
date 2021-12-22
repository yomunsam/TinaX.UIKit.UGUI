using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Exceptions;
using TinaX.UIKit.UGUI.Page.Navigator;

namespace TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage.Handlers
{
    public class MakeUIPageAsyncHandler : IGetUGuiPageAsyncHandler
    {
        public string HandlerName => HandlerNameConsts.MakeUIPage;

        public UniTask GetPageAsync(GetUGuiPageContext context, GetUGuiPagePayload payload, CancellationToken cancellationToken)
        {
            if (payload.ViewProvider == null)
                throw new XException("ViewProvider cannot be null");

            if(payload.Args.PageController != null && !(payload.Args.PageController is UGUIPageController))
            {
                throw new XException("PageController must be \"UGUIPageController\"");
            }
            var ugui_controller = payload.Args.PageController as UGUIPageController;
            payload.UIPage = new Page.UGUIPage(payload.Args.PageUri, payload.ViewProvider!, ugui_controller, payload.Args.XBehaviourWrapperReflectionProvider);

            if(ugui_controller != null)
            {
                //Controller导航器
                var navigator = new UGUIPageNavigator(payload.UIPage, context.UIKit, context.UIKitUGUI);
                ugui_controller.Navigation = navigator;
            }

            return UniTask.CompletedTask;
        }
    }
}
