using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TinaX.Core.Consts;
using TinaX.Core.Helper;
using TinaX.Core.Utils;
using TinaX.Exceptions;
using TinaX.Options;
using TinaX.Services;
using TinaX.Services.ConfigAssets;
using TinaX.Systems.Pipeline;
using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.ConfigAssets;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;
using TinaX.UIKit.UGUI.UIRoot;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Services
{
#nullable enable
    public class UIKitUGUIService : IUIKitUGUI, IUIKitUGUIInternalService
    {
        
        private readonly IAssetService m_AssetService;
        private readonly IConfigAssetService m_ConfigAssetService;
        private readonly IXCore m_Core;
        private readonly IUIKit m_UIKit;
        private readonly UIKitUGUIOptions m_Options;

        private readonly XPipeline<IGetUGuiPageAsyncHandler> m_GetUGUIPageAsyncPipeline;

        public UIKitUGUIService(IAssetService assetService,
            IConfigAssetService configAssetService,
            IOptions<UIKitUGUIOptions> options,
            IXCore core,
            IUIKit uikit)
        {
            this.m_AssetService = assetService;
            this.m_ConfigAssetService = configAssetService;
            this.m_Core = core;
            this.m_UIKit = uikit;
            m_Options = options.Value;

            m_GetUGUIPageAsyncPipeline = m_Options.GetUGUIPageAsyncPipeline;
        }

        private bool m_Initialized;
        private UIKitUGUIConfigAsset? m_ConfigAsset;
        private GameObject? m_UIKitUGUIRootGameObject;
        private UIRootManager? m_UIRootManager;
        private Camera? m_UICamera;

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
#if TINAX_DEV
            Debug.LogFormat("[{0}]开始启动", UIKitUGUIConsts.ProviderName);
#endif
            if (m_Initialized)
                return;

            //加载配置资产
            m_ConfigAsset = await LoadConfigAssetAsync(m_Options.ConfigAssetLoadPath, cancellationToken);
            if (m_ConfigAsset == null)
            {
                throw new XException($"Failed to load configuration assets \"{m_Options.ConfigAssetLoadPath}\" ");
            }

            //准备 UIKit uGUI GameObjects
            #region UIKit uGUI GameObjects
            //Root GameObject
            var tinax_gameObject = GameObject.Find(TinaXConst.FrameworkBaseGameObjectName);
            if(tinax_gameObject == null)
            {
                tinax_gameObject = new GameObject(TinaXConst.FrameworkBaseGameObjectName)
                    .DontDestroy();
            }
            m_UIKitUGUIRootGameObject = tinax_gameObject.FindOrCreateGameObject("UIKit uGUI")
                .SetPosition(new Vector3(-99999, -99999, -99999)); //这句话在新版本里好像不起作用了

            //UI Camera
            if (m_ConfigAsset.CreateUICameraAutomatically)
            {
                var cameraConfiguration = m_ConfigAsset.UICameraConfigAsset ?? new UICameraConfigAsset();
                m_UICamera = m_UIKitUGUIRootGameObject.FindOrCreateGameObject("UICamera")
                    .GetComponentOrAdd<Camera>();

                m_UICamera.clearFlags = cameraConfiguration.clearFlags;
                m_UICamera.backgroundColor = cameraConfiguration.backgroundColor;
                m_UICamera.cullingMask = cameraConfiguration.cullingMask;
                m_UICamera.orthographic = cameraConfiguration.orthographic;
                m_UICamera.orthographicSize = cameraConfiguration.orthographicSize;
                m_UICamera.nearClipPlane = cameraConfiguration.nearClipPlane;
                m_UICamera.farClipPlane = cameraConfiguration.farClipPlane;
                m_UICamera.depth = cameraConfiguration.depth;
                m_UICamera.renderingPath = cameraConfiguration.renderingPath;
                m_UICamera.targetTexture = cameraConfiguration.targetTexture;
                m_UICamera.useOcclusionCulling = cameraConfiguration.useOcclusionCulling;
                m_UICamera.allowHDR = cameraConfiguration.allowHDR;
                m_UICamera.allowMSAA = cameraConfiguration.allowMSAA;
            }

            //UIRoot
            m_UIRootManager = new UIRootManager(m_Core.Services.Get<IUIKit>(), m_UIKitUGUIRootGameObject, m_ConfigAsset.CanvasScalerConfig, m_UICamera);
            m_UIRootManager.EnsureUIRoot();

            //InputSystem EventSystem
            if (m_ConfigAsset.CreateUIEventSystemAutomatically)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                var eventSystemGameObject = GameObjectHelper.FindOrCreateGameObject("EventSystem").DontDestroy();
                var unityEventSystem = eventSystemGameObject.GetComponentOrAdd<UnityEngine.EventSystems.EventSystem>();
                unityEventSystem.sendNavigationEvents = true;
                unityEventSystem.pixelDragThreshold = 10;

                _ = eventSystemGameObject.GetComponentOrAdd<UnityEngine.EventSystems.StandaloneInputModule>();

                if (LocalizationUtil.IsHans())
                    Debug.Log($"[{UIKitUGUIConsts.ProviderName}]EventSystem被自动创建", eventSystemGameObject);
                else
                    Debug.Log($"[{UIKitUGUIConsts.ProviderName}]EventSystem is automatically created", eventSystemGameObject);
#endif
            }

            #endregion



            await Task.CompletedTask;
            m_Initialized = true;
        }


        public UniTask<UGUIPage> GetUIPageAsync(string pageUri, bool loadViewPrefab = true, CancellationToken cancellationToken = default)
        {
            var options = new GetUGUIPageArgs(pageUri);
            return this.GetUIPageAsync(options, cancellationToken);
        }

        public async UniTask<UGUIPage> GetUIPageAsync(GetUGUIPageArgs args, CancellationToken cancellationToken = default)
        {
            //走Pipeline
            var context = new GetUGuiPageContext(m_Core.Services, m_AssetService, m_UIKit, this);
            var payload = new GetUGuiPagePayload(args);

            await m_GetUGUIPageAsyncPipeline.StartAsync(async handler =>
            {
                await handler.GetPageAsync(context, payload, cancellationToken);
                return !context.BreakPipeline; //返回true则继续队列往下
            });

            return payload.UIPage!;
        }

        /// <summary>
        /// 把UI压到UGUI屏幕空间
        /// </summary>
        /// <param name="page"></param>
        /// <param name="displayMessageArgs">传递给UI的启动参数</param>
        /// <returns></returns>
        /// <exception cref="XException"></exception>
        public void PushScreenUI(UGUIPage page, object[]? displayMessageArgs = null)
        {
            if (!m_Initialized)
                throw new XException($"{UIKitUGUIConsts.ProviderName} not ready.");
            //获取Canvas
            var uiKitCanvas = m_UIRootManager!.GetUIKitCanvasOrCreate(); //Todo:SortingLayer分层
            uiKitCanvas.RootGroupUGUI.GetLastChildUGUIGroup().Push(page, displayMessageArgs);
        }


        private UniTask<UIKitUGUIConfigAsset> LoadConfigAssetAsync(string loadPath, CancellationToken cancellationToken)
        {
            return m_ConfigAssetService.GetConfigAsync<UIKitUGUIConfigAsset>(loadPath, cancellationToken);
        }
    }
#nullable restore
}
