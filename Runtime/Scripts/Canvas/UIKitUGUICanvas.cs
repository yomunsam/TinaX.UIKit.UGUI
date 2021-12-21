using TinaX.UIKit.Canvas;
using TinaX.UIKit.Page.Group;
using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit.UGUI.Canvas
{
    public class UIKitUGUICanvas : UIKitCanvas
    {
        public UIKitUGUICanvas() : base() { }
        public UIKitUGUICanvas(UGUIPageGroup rootGroup) : base(rootGroup)
        {
            m_RootGroupUGUI = rootGroup;
        }
        public UIKitUGUICanvas(UGUIPageGroup rootGroup , string name):base(rootGroup, name)
        {
            m_RootGroupUGUI = rootGroup;
        }

        protected UGUIPageGroup m_RootGroupUGUI;  

        public UGUIPageGroup RootGroupUGUI
        {
            get
            {
                if (m_RootGroupUGUI == null)
                    m_RootGroupUGUI = (UGUIPageGroup)RootGroup;
                return m_RootGroupUGUI;
            }
        }


    }
}
