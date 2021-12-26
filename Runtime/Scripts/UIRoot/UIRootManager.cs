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
        public const int EmptyDisplayIndex = -1; //没有指定UICamera的时候，用这个index
        private readonly IUIKit m_UIKit;
        private readonly GameObject m_UIKitUGUIRootGameObject;
        private readonly UGUICanvasScalerConfig m_DefaultCanvasScalerConfig;
        private readonly Dictionary<int, Dictionary<int, GameObject>> m_UIRootGameObjectDict = new Dictionary<int, Dictionary<int, GameObject>>(1); //sortingLayerId -> displayIndex -> GameObject , 如果没有UICamera，则默认displayIndex是-1
        private readonly Dictionary<int, Dictionary<int, UIKitUGUICanvasComponentBase>> m_UIKitUGUICanvasComponentDict = new Dictionary<int, Dictionary<int, UIKitUGUICanvasComponentBase>>(1);

        public UIRootManager(IUIKit uikit, 
            GameObject uiKitUGUIRootGameObject,
            UGUICanvasScalerConfig defaultCanvasScalerConfig)
        {
            this.m_UIKit = uikit;
            this.m_UIKitUGUIRootGameObject = uiKitUGUIRootGameObject;
            this.m_DefaultCanvasScalerConfig = defaultCanvasScalerConfig;
        }

        private GameObject? m_UIRootsGameObject; //存放各种UIRoot的基础空GameObject


        public void EnsureUIRoot(Camera? uiCamera, int sortingLayerId = 0, UGUICanvasScalerConfig? canvasScalerConfig = null)
        {
            if (SortingLayer.IsValid(sortingLayerId))
            {
                int displayIndex = uiCamera?.targetDisplay ?? EmptyDisplayIndex;

                if(!m_UIRootGameObjectDict.TryGetValue(sortingLayerId, out var uiRootGameObjects))
                {
                    uiRootGameObjects = new Dictionary<int, GameObject>(1);
                    m_UIRootGameObjectDict.Add(sortingLayerId, uiRootGameObjects);
                }
                if(!uiRootGameObjects.TryGetValue(displayIndex, out var uiRootGameObject))
                {
                    m_UIRootsGameObject = m_UIKitUGUIRootGameObject.FindOrCreateGameObject("UIRoots");

                    uiRootGameObject = m_UIRootsGameObject.FindOrCreateGameObject($"UIRoot_d{uiCamera?.targetDisplay ?? EmptyDisplayIndex}_s{sortingLayerId}")
                        .SetLayerRecursive(5)
                        .SetLocalPosition(Vector3.zero);
                    uiRootGameObjects.Add(uiCamera?.targetDisplay ?? EmptyDisplayIndex, uiRootGameObject);

                    var uGUICanvas = uiRootGameObject.GetComponentOrAdd<UnityEngine.Canvas>();
                    uGUICanvas.sortingLayerID = sortingLayerId;

                    //UI Camera
                    if (uiCamera == null)
                    {
                        uGUICanvas.worldCamera = null;
                        uGUICanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    }
                    else
                    {
                        uGUICanvas.renderMode = RenderMode.ScreenSpaceCamera;
                        uGUICanvas.worldCamera = uiCamera;
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
                    var uiKitCanvas = uiKitCanvasComponent.UIKitCanvas!;

                    uiKitCanvas!.SortingLayerId = sortingLayerId;


                    if (!m_UIKitUGUICanvasComponentDict.TryGetValue(sortingLayerId, out var components))
                    {
                        components = new Dictionary<int, UIKitUGUICanvasComponentBase>(1);
                        m_UIKitUGUICanvasComponentDict.Add(sortingLayerId, components);
                    }

                    if (!components.ContainsKey(displayIndex))
                    {
                        components.Add(displayIndex, uiKitCanvasComponent);
                        m_UIKit.RegisterUIKitCanvas(uiKitCanvas);
                    }
                }

                
            }
        }

        /// <summary>
        /// 获取UIKit Canvas，如果没有则创建
        /// </summary>
        /// <param name="uiCamera">如果这个UIKit Canvas有一个UI相机，则传入它，没有就null</param>
        /// <param name="sortingLayerId"></param>
        /// <param name="canvasScalerConfig">如果需要创建的话，这里可以设置CanvasScaler，通常不用传，会用默认的（也就是直接从ConfigAsset里读的那个）</param>
        /// <returns></returns>
        public UIKitUGUICanvas GetUIKitCanvasOrCreate(Camera? uiCamera, int sortingLayerId = 0, UGUICanvasScalerConfig? canvasScalerConfig = null)
        {
            if (!m_UIKitUGUICanvasComponentDict.TryGetValue(sortingLayerId, out var components))
            {
                components = new Dictionary<int, UIKitUGUICanvasComponentBase>(1);
                m_UIKitUGUICanvasComponentDict.Add(sortingLayerId, components);
            }

            int displayIndex = uiCamera?.targetDisplay ?? EmptyDisplayIndex;
            if (components.TryGetValue(displayIndex, out var uiKitCanvas))
                return uiKitCanvas.UIKitCanvas!;
            else
            {
                EnsureUIRoot(uiCamera, sortingLayerId, canvasScalerConfig);
                return components[displayIndex].UIKitCanvas!;
            }
        }
    
    }
#nullable restore
}
