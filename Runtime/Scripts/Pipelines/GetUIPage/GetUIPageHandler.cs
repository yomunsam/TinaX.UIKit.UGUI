using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Pipelines.GetUIPage;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Helper;
using TinaX.UIKit.UGUI.Services;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Pipelines.GetUIPage
{
#nullable enable
    public class GetUIPageHandler : IGetUIPageAsyncHandler
    {
        public GetUIPageHandler()
        {
        }

        public string HandlerName => UIKitUGUIConsts.GetUIPagePipelineHandlerName;

        private IUIKitUGUI? m_UIKit_UGUI_Service;

        

        public async UniTask GetPageAsync(GetUIPageContext context, GetUIPagePayload payload, CancellationToken cancellationToken)
        {
#if TINAX_DEV
            Debug.Log("UIKit uGUI - Get Page: " + payload.PageUri);
#endif
            if (m_UIKit_UGUI_Service == null)
                m_UIKit_UGUI_Service = context.Services.Get<IUIKitUGUI>();

            if (!PageUriHelper.IsMatch(payload.PageUri))
                return;
            var getUGUIPageArgs = new GetUGUIPageArgs(payload.PageUri)
            {
                PageController = payload.PageController
            };
            var page = await m_UIKit_UGUI_Service.GetUIPageAsync(getUGUIPageArgs);
            payload.UIPage = page;

            //加载完成，终止管线
            context.Break();
        }
    }
#nullable restore
}
