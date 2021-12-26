using System;
using System.Collections.Generic;
using UnityEngine;

namespace TinaX.UIKit.UGUI.ConfigAssets
{
    public class UIKitUGUIConfigAsset : ScriptableObject
    {
        public UIKitUGUIConfigAsset()
        {
            UICameraConfigAssets = new List<UICameraConfigAsset>();
        }

        /// <summary>
        /// 自动创建UICamera
        /// </summary>
        public bool CreateUICameraAutomatically;
        //public UICameraConfigAsset UICameraConfigAsset;

        public List<UICameraConfigAsset> UICameraConfigAssets;

        /// <summary>
        /// 自动创建 EventSystem
        /// </summary>
        public bool CreateUIEventSystemAutomatically;

        public UGUICanvasScalerConfig CanvasScalerConfig;
    }
}
