using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Pipelines.GetUIPage;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Services;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Pipelines.GetUIPage
{
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

        private IUIKitUGUI m_UIKit_UGUI_Service;

        public async UniTask GetPageAsync(string pageUri, GetUIPageContext context, CancellationToken cancellationToken)
        {
            Debug.Log("有走到这里");
            if (m_UIKit_UGUI_Service == null)
                m_UIKit_UGUI_Service = context.Services.Get<IUIKitUGUI>();

            if(!pageUri.ToLower().StartsWith(_uiKit_UGUI_Scheme))
                return;

            var page = await m_UIKit_UGUI_Service.GetUIPageAsync(pageUri, true, cancellationToken);
            context.UIPageReuslt = page;
        }
    }
}
