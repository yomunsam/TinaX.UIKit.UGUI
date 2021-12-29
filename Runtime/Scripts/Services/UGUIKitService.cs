using System.Linq;
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
using TinaX.UIKit.UGUI.Animation;
using TinaX.UIKit.UGUI.ConfigAssets;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.MultipleDisplay;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.Group;
using TinaX.UIKit.UGUI.Pipelines.GetUGuiUIPage;
using TinaX.UIKit.UGUI.UICamera;
using TinaX.UIKit.UGUI.UIRoot;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Services
{
#nullable enable
    public class UGUIKitService : IUGUIKit, IUGUIKitInternalService
    {
        
        private readonly IAssetService m_AssetService;
        private readonly IConfigAssetService m_ConfigAssetService;
        private readonly IXCore m_Core;
        private readonly IUIKit m_UIKit;
        private readonly UIKitUGUIOptions m_Options;
        private readonly UICameraManager m_UICameraManager;

        private readonly XPipeline<IGetUGuiPageAsyncHandler> m_GetUGUIPageAsyncPipeline;
        private readonly SimpleAnimationManager m_SimpleAnimationManager;

        //------------构造方法---------------------------------------------------------------------------------------

        public UGUIKitService(IAssetService assetService,
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
            m_UICameraManager = new UICameraManager();
            m_SimpleAnimationManager = new SimpleAnimationManager(m_Options.PlaySimpleAnimationGateways);
        }

        //------------内部字段---------------------------------------------------------------------------------------

        private bool m_Initialized;
        private UIKitUGUIConfigAsset? m_ConfigAsset;
        private GameObject? m_UIKitUGUIRootGameObject;
        private UIRootManager? m_UIRootManager;
        private GameObject? m_UICameraRootGameObject;

        //------------公开属性---------------------------------------------------------------------------------------------

        public SimpleAnimationManager SimpleAnimationManager => m_SimpleAnimationManager;

        //------------公开方法--------------------------------------------------------------------------------------------

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
            m_UICameraRootGameObject = m_UIKitUGUIRootGameObject.FindOrCreateGameObject("UICameras");

            if (m_ConfigAsset.CreateUICameraAutomatically)
            {
                var defaultConfig = ScriptableObject.CreateInstance<UICameraConfigAsset>();

                if (m_Options.EnableMultipleDisplay)
                {
                    //如果可以有多个Display的情况
                    for (int i = 0; i < Display.displays.Length; i++)
                    {
                        if(i > DisplayIndexRange.MaximumIndex)
                        {
                            Debug.LogError("The number of displays exceeds the upper limit by 8");
                            break;
                        }
                        UICameraConfigAsset config;
                        if(m_ConfigAsset.UICameraConfigAssets.Count > i)
                        {
                            config = m_ConfigAsset.UICameraConfigAssets[i];
                        }
                        else
                        {
                            if (m_ConfigAsset.UICameraConfigAssets.Count > 0)
                                config = m_ConfigAsset.UICameraConfigAssets[0];
                            else
                                config = defaultConfig;
                        }

                        if (config == null) //配置里面有可能会配一个空的数组项
                            config = defaultConfig;

                        m_UICameraManager.AddUICamera(config, m_UICameraRootGameObject, (DisplayIndex)i);
                    }
#if UNITY_EDITOR
                    //Unity Editor中就只会识别出一个显示器，于是通过一个设置强行开多个
                    if (m_Options.ForceDisplayNumInEditor != null && m_Options.ForceDisplayNumInEditor > 1)
                    {
                        for(int i = 1; i < m_Options.ForceDisplayNumInEditor.Value; i++)
                        {
                            UICameraConfigAsset config;
                            if (m_ConfigAsset.UICameraConfigAssets.Count > i)
                            {
                                config = m_ConfigAsset.UICameraConfigAssets[i];
                            }
                            else
                            {
                                if (m_ConfigAsset.UICameraConfigAssets.Count > 0)
                                    config = m_ConfigAsset.UICameraConfigAssets[0];
                                else
                                    config = defaultConfig;
                            }

                            if (config == null) //配置里面有可能会配一个空的数组项
                                config = defaultConfig;

                            m_UICameraManager.AddUICamera(config, m_UICameraRootGameObject, (DisplayIndex)i);
                        }
                    }
#endif
                }
                else
                {
                    //不管如何只能有一个Display
                    if (m_ConfigAsset.UICameraConfigAssets.Count > 0)
                        m_UICameraManager.AddUICamera(m_ConfigAsset.UICameraConfigAssets[0] ?? defaultConfig, m_UICameraRootGameObject);
                    else
                        m_UICameraManager.AddUICamera(defaultConfig, m_UICameraRootGameObject);
                }
            }
            

            //UIRoot
            m_UIRootManager = new UIRootManager(m_Core.Services.Get<IUIKit>(), m_UIKitUGUIRootGameObject, m_ConfigAsset.CanvasScalerConfig);
            //根据UICamera创建UIRoot
            var uiCameras = m_UICameraManager.UICameras;
            if(uiCameras.Count > 0) 
            {
                //可以有多个Display并且有UICamera的话
                var uiCameraEnumerator = uiCameras.GetEnumerator();
                while (uiCameraEnumerator.MoveNext())
                {
                    m_UIRootManager.EnsureUIRoot(uiCameraEnumerator.Current.Value); //sortingLayer 为 0 是Unity自带的，先只创建这一个，其他的惰性创建
                }
            }
            else
            {
                //至少目前没有任何一个UICamera，我们给弄一个没有UICamera的UIRoot出来
                m_UIRootManager.EnsureUIRoot(null);
            }

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
        /// <param name="displayIndex">你想把UI显示到哪一页屏幕上?（多Display时）不指定的话，会跑到默认的地方</param>
        /// <param name="pushArgs">相关参数</param>
        /// <exception cref="XException"></exception>
        public void PushScreenUI(UGUIPage page, DisplayIndex? displayIndex = null, PushUGUIPageArgs? pushArgs = null)
        {
            if (!m_Initialized)
                throw new XException($"{UIKitUGUIConsts.ProviderName} not ready.");
            //获取sortingLayer
            int sortingLayer = 0; //Todo,后面再写，应该是从UIPage里获取
            //UICamera
            Camera? uiCamera;
            if (displayIndex == null)
            {
                uiCamera = m_UICameraManager.GetCamera(); //尝试获取默认Camera
            }
            else
            {
                uiCamera = m_UICameraManager.GetCamera(displayIndex.Value);
            }

            //获取Canvas
            var uiKitCanvas = m_UIRootManager!.GetUIKitCanvasOrCreate(uiCamera, sortingLayer);
            uiKitCanvas.RootGroupUGUI.GetLastChildUGUIGroup().Push(page, pushArgs);
        }

        private UniTask<UIKitUGUIConfigAsset> LoadConfigAssetAsync(string loadPath, CancellationToken cancellationToken)
        {
            return m_ConfigAssetService.GetConfigAsync<UIKitUGUIConfigAsset>(loadPath, cancellationToken);
        }
    }
#nullable restore
}
