using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TinaX.Services;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.View;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Services
{
    public class UIKitUGUIService : IUIKitUGUI, IUIKitUGUIInternalService
    {
        private readonly string _uiKit_UGUI_Scheme;
        private readonly int _uiKit_UGUI_Scheme_Length;
        private readonly IAssetService m_AssetService;

        public UIKitUGUIService(IAssetService assetService)
        {
            _uiKit_UGUI_Scheme = $"{UIKitUGUIConsts.SchemeName.ToLower()}://";
            _uiKit_UGUI_Scheme_Length = _uiKit_UGUI_Scheme.Length;
            this.m_AssetService = assetService;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
#if TINAX_DEV
            Debug.LogFormat("[{0}]开始启动", UIKitUGUIConsts.ProviderName);
#endif
            await Task.CompletedTask;
        }


        public async UniTask<UGUIPage> GetUIPageAsync(string pageUri, bool loadViewPrefab = true, CancellationToken cancellationToken = default)
        {
            //Todo: 这里是超简化写法，先把UIKit的大框架打起来再重写UGUI这边
            var viewLoadPath = pageUri.ToLower().StartsWith(_uiKit_UGUI_Scheme) ? pageUri.Substring(_uiKit_UGUI_Scheme_Length, pageUri.Length - _uiKit_UGUI_Scheme_Length) : pageUri;
            if (!viewLoadPath.ToLower().EndsWith(".prefab"))
                viewLoadPath += ".prefab";

            var viewProvider = new UGUIPageViewProvider(viewLoadPath, m_AssetService);
            var page = new UGUIPage(viewProvider);

            if (loadViewPrefab)
                await page.ReadyViewAsync();
            return page;
        }

    }
}
