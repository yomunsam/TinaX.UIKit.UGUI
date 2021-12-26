using TinaX.UIKit.UGUI.ConfigAssets;
using TinaX.UIKit.UGUI.MultipleDisplay;
using UnityEngine;

namespace TinaX.UIKit.UGUI.UICamera
{
    public interface IUICameraManager
    {
        void AddUICamera(UICameraConfigAsset configuration, GameObject camerasRootGameObject, DisplayIndex displayIndex = DisplayIndex.Display1);
        void AddUICamera(Camera uiCamera, DisplayIndex displayIndex = DisplayIndex.Display1);
        Camera GetCamera(DisplayIndex displayIndex = DisplayIndex.Display1);
    }
}
