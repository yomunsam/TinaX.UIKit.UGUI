using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TinaX.Options;
using TinaX.Services;
using TinaX.Systems.Pipeline;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.View;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Services
{
    public class UIKitUGUIService : IUIKitUGUI, IUIKitUGUIInternalService
    {
        
        private readonly IAssetService m_AssetService;
        private readonly IXCore m_Core;
        private readonly UIKitUGUIOptions m_Options;

        private readonly XPipeline<IGetUGuiPageAsyncHandler> m_GetUGUIPageAsyncPipeline;

        public UIKitUGUIService(IAssetService assetService,
            IOptions<UIKitUGUIOptions> options,
            IXCore core )
        {
            this.m_AssetService = assetService;
            this.m_Core = core;
            m_Options = options.Value;

            m_GetUGUIPageAsyncPipeline = m_Options.GetUGUIPageAsyncPipeline;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
#if TINAX_DEV
            Debug.LogFormat("[{0}]开始启动", UIKitUGUIConsts.ProviderName);
#endif
            await Task.CompletedTask;
        }


        public UniTask<UGUIPage> GetUIPageAsync(string pageUri, bool loadViewPrefab = true, CancellationToken cancellationToken = default)
        {
            var options = new GetUGUIPageOptions(pageUri);
            return this.GetUIPageAsync(options, cancellationToken);
        }

        public async UniTask<UGUIPage> GetUIPageAsync(GetUGUIPageOptions options, CancellationToken cancellationToken = default)
        {
            //走Pipeline
            var context = new GetUGuiPageContext(m_Core.Services, m_AssetService);
            var payload = new GetUGuiPagePayload(options);

            await m_GetUGUIPageAsyncPipeline.StartAsync(async handler =>
            {
                await handler.GetPageAsync(context, payload, cancellationToken);
                return !context.BreakPipeline; //返回true则继续队列往下
            });

            return payload.UIPage;
        }

    }
}
