using System;
using TinaX.UIKit.Canvas;
using UnityEngine;
using TinaX;

namespace TinaX.UIKit.UGUI.Canvas
{
    [AddComponentMenu("TinaX/UIKit/UGUI/UIKit uGUI Canvas")]
    public class UIKitUGUICanvasComponent : MonoBehaviour
    {
        #region Unity Serializable Fields
        [Tooltip("Don't destroy on load")]
        public bool DontDestroy;
        #endregion

        public UIKitUGUICanvasComponent()
        {
            Canvas = new UIKitCanvas();
        }
        public UIKitCanvas Canvas { get; }


        #region Unity Magic Methods
        void Awake()
        {
            if (DontDestroy)
                this.gameObject.DontDestroyOnLoad();
        }

        #endregion
    }
}
