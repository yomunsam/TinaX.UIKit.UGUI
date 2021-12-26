using System.Collections.Generic;
using System.Linq;
using TinaX.UIKit.UGUI.ConfigAssets;
using TinaX.UIKit.UGUI.MultipleDisplay;
using UnityEngine;

namespace TinaX.UIKit.UGUI.UICamera
{
#nullable enable
    public class UICameraManager : IUICameraManager
    {

        //------------固定字段---------------------------------------------------------------------------------------
        private readonly Dictionary<DisplayIndex, Camera> m_UICameraDict = new Dictionary<DisplayIndex, Camera>(1); //Key: displayIndex

        public UICameraManager()
        {

        }

        
        public Dictionary<DisplayIndex, Camera> UICameras => m_UICameraDict;

        public void AddUICamera(UICameraConfigAsset configuration, GameObject camerasRootGameObject , DisplayIndex displayIndex = DisplayIndex.Display1)
        {
            if (displayIndex < 0)
                return;

            var uiCamera = camerasRootGameObject.FindOrCreateGameObject($"UICamera_{displayIndex}")
                .GetComponentOrAdd<Camera>();

            uiCamera.clearFlags = configuration.clearFlags;
            uiCamera.backgroundColor = configuration.backgroundColor;
            uiCamera.cullingMask = configuration.cullingMask;
            uiCamera.orthographic = configuration.orthographic;
            uiCamera.orthographicSize = configuration.orthographicSize;
            uiCamera.nearClipPlane = configuration.nearClipPlane;
            uiCamera.farClipPlane = configuration.farClipPlane;
            uiCamera.depth = configuration.depth;
            uiCamera.renderingPath = configuration.renderingPath;
            uiCamera.targetTexture = configuration.targetTexture;
            uiCamera.useOcclusionCulling = configuration.useOcclusionCulling;
            uiCamera.allowHDR = configuration.allowHDR;
            uiCamera.allowMSAA = configuration.allowMSAA;

            AddUICamera(uiCamera, displayIndex);
        }

        public void AddUICamera(Camera uiCamera, DisplayIndex displayIndex = DisplayIndex.Display1)
        {
            if (displayIndex < 0)
                return;
            if (m_UICameraDict.ContainsKey(displayIndex))
                m_UICameraDict.Remove(displayIndex);
            uiCamera.targetDisplay = (int)displayIndex;
            m_UICameraDict.Add(displayIndex, uiCamera);
        }

        public Camera? GetCamera(DisplayIndex displayIndex = DisplayIndex.Display1)
        {
            if(displayIndex < 0)
                return null;

            if (m_UICameraDict.TryGetValue(displayIndex, out var camera))
                return camera;
            else
                return null;
        }


    }
}
