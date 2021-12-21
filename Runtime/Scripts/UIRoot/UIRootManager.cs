using System.Collections.Generic;
using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.ConfigAssets;
using UnityEngine;

namespace TinaX.UIKit.UGUI.UIRoot
{
#nullable enable
    /// <summary>
    /// 管理屏幕空间UI的UIRoot
    /// </summary>
    public class UIRootManager
    {
        private readonly IUIKit m_UIKit;
        private readonly GameObject m_UIKitUGUIRootGameObject;
        private readonly UGUICanvasScalerConfig m_DefaultCanvasScalerConfig;
        private readonly Dictionary<int, GameObject> m_UIRootGameObjectDict = new Dictionary<int, GameObject>(1);
        private readonly Dictionary<int, UIKitUGUICanvasComponentBase> m_UIKitUGUICanvasComponentDict = new Dictionary<int, UIKitUGUICanvasComponentBase>(1);

        public UIRootManager(IUIKit uikit, 
            GameObject uiKitUGUIRootGameObject,
            UGUICanvasScalerConfig defaultCanvasScalerConfig,
            Camera? uiCamera)
        {
            this.m_UIKit = uikit;
            this.m_UIKitUGUIRootGameObject = uiKitUGUIRootGameObject;
            this.m_DefaultCanvasScalerConfig = defaultCanvasScalerConfig;
            this.m_UICamera = uiCamera;
        }

        private Camera? m_UICamera;


        public void EnsureUIRoot(int sortingLayerId = 0, UGUICanvasScalerConfig? canvasScalerConfig = null)
        {
            if (SortingLayer.IsValid(sortingLayerId))
            {
                if(!m_UIRootGameObjectDict.TryGetValue(sortingLayerId, out var uiRootGameObject))
                {
                    uiRootGameObject = m_UIKitUGUIRootGameObject.FindOrCreateGameObject($"UIRoot_{sortingLayerId}")
                        .SetLayerRecursive(5)
                        .SetLocalPosition(Vector3.zero);
                    m_UIRootGameObjectDict.Add(sortingLayerId, uiRootGameObject);
                }

                var uGUICanvas = uiRootGameObject.GetComponentOrAdd<UnityEngine.Canvas>();
                uGUICanvas.sortingLayerID = sortingLayerId;

                //UI Camera
                if(m_UICamera == null)
                {
                    uGUICanvas.worldCamera = null;
                    uGUICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                }
                else
                {
                    uGUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
                    uGUICanvas.worldCamera = m_UICamera;
                }

                //CanvasScaler
                if (canvasScalerConfig == null)
                    canvasScalerConfig = m_DefaultCanvasScalerConfig;
                var canvasScaler = uiRootGameObject.GetComponentOrAdd<UnityEngine.UI.CanvasScaler>();
                canvasScaler.uiScaleMode = canvasScalerConfig.UICanvasScalerMode;
                canvasScaler.scaleFactor = canvasScalerConfig.UIScaleFactor;
                canvasScaler.referencePixelsPerUnit = canvasScalerConfig.ReferencePixelsPerUnit;

                canvasScaler.referenceResolution = canvasScalerConfig.ReferenceResolution;
                canvasScaler.screenMatchMode = canvasScalerConfig.ScreenMatchMode;
                canvasScaler.matchWidthOrHeight = canvasScalerConfig.CanvasScalerMatchWidthOrHeight;

                canvasScaler.physicalUnit = canvasScalerConfig.PhySicalUnit;
                canvasScaler.fallbackScreenDPI = canvasScalerConfig.FallbackScreenDPI;
                canvasScaler.defaultSpriteDPI = canvasScalerConfig.DefaultSpriteDPI;

                //UIKit Canvas Component 
                var uiKitCanvasComponent = uiRootGameObject.GetComponentOrAdd<UIKitUGUICanvasComponentBase>();
                uiKitCanvasComponent.EnsureUIKitCanvas();
                var uiKitCanvas = uiKitCanvasComponent.UIKitCanvas;

                if(!m_UIKitUGUICanvasComponentDict.ContainsKey(sortingLayerId))
                {
                    m_UIKitUGUICanvasComponentDict.Add(sortingLayerId, uiKitCanvasComponent);
                }
                m_UIKit.RegisterUIKitCanvas(uiKitCanvas);
            }
        }
    
        public UIKitUGUICanvas GetUIKitCanvasOrCreate(int sortingLayerId = 0, UGUICanvasScalerConfig? canvasScalerConfig = null)
        {
            if (m_UIKitUGUICanvasComponentDict.TryGetValue(sortingLayerId, out var uiKitCanvas))
                return uiKitCanvas.UIKitCanvas!;
            else
            {
                EnsureUIRoot(sortingLayerId, canvasScalerConfig);
                return m_UIKitUGUICanvasComponentDict[sortingLayerId].UIKitCanvas!;
            }
        }
    
    }
#nullable restore
}
