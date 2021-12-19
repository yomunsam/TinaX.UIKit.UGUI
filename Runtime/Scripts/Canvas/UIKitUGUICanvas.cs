using TinaX.UIKit.Canvas;
using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit.UGUI.Canvas
{
    public class UIKitUGUICanvas : UIKitCanvas
    {
        public UIKitUGUICanvas() : base() { }
        public UIKitUGUICanvas(UGUIPageGroup mainGroup) : base(mainGroup) { }
        public UIKitUGUICanvas(UGUIPageGroup mainGroup , string name):base(mainGroup, name) { }
    }
}
