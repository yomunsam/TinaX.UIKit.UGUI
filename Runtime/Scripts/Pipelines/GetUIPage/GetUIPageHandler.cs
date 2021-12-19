using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Pipelines.GetUIPage;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Services;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Pipelines.GetUIPage
{
#nullable enable
    public class GetUIPageHandler : IGetUIPageAsyncHandler
    {
        private readonly string _uiKit_UGUI_Scheme;
        private readonly int _uiKit_UGUI_Scheme_Length;
        public GetUIPageHandler()
        {
            _uiKit_UGUI_Scheme = $"{UIKitUGUIConsts.SchemeName.ToLower()}://";
            _uiKit_UGUI_Scheme_Length = _uiKit_UGUI_Scheme.Length;
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

            if (!payload.PageUriLower.StartsWith(_uiKit_UGUI_Scheme))
                return;
            var getUIOptions = new GetUGUIPageOptions(payload.PageUri)
            {
                PageController = payload.PageController
            };
            var page = await m_UIKit_UGUI_Service.GetUIPageAsync(getUIOptions);
            payload.UIPage = page;

            //加载完成，终止管线
            context.Break();
        }
    }
#nullable restore
}
