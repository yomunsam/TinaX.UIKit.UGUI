using TinaX.UIKit.Page;
using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit.UGUI.Page
{
    public interface IUGUIGroup : IGroup
    {
        void Push(UGUIPage page, PushUGUIPageArgs pushArgs = null);
    }
}
