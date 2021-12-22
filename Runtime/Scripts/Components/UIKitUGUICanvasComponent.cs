using TinaX.UIKit.UGUI.Canvas;
using TinaX.UIKit.UGUI.Page.Group;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Components
{
#nullable enable
    [AddComponentMenu("TinaX/UIKit uGUI/UIKit uGUI Canvas")]
    public class UIKitUGUICanvasComponent : MonoBehaviour
    {
        #region Unity Serializable Fields
        [Tooltip("Don't destroy on load")]
        public bool DontDestroy;
        #endregion

        public UIKitUGUICanvasComponent()
        {
            
        }

        public UIKitUGUICanvas? Canvas { get; private set; }


        #region Unity Magic Methods
        void Awake()
        {
            Canvas = new UIKitUGUICanvas(new UGUIPageGroup(this.name), this.transform, this.name);
            if (DontDestroy)
                this.gameObject.DontDestroyOnLoad();
        }

        #endregion

        
    }

#nullable restore
}
