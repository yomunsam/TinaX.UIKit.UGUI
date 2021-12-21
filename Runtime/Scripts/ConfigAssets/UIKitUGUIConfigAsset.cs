using UnityEngine;

namespace TinaX.UIKit.UGUI.ConfigAssets
{
    public class UIKitUGUIConfigAsset : ScriptableObject
    {
        /// <summary>
        /// 自动创建UICamera
        /// </summary>
        public bool CreateUICameraAutomatically;
        public UICameraConfigAsset UICameraConfigAsset;
        /// <summary>
        /// 自动创建 EventSystem
        /// </summary>
        public bool CreateUIEventSystemAutomatically;

        public UGUICanvasScalerConfig CanvasScalerConfig;
    }
}
