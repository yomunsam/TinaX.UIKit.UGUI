using TinaX.UIKit.Page.Navigator;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders;
using TinaX.UIKit.UGUI.Page;

namespace TinaX.UIKit
{
    public static class UGUIPageNavigatorExtensions
    {
        public static PageNavigatorOpenUGUIBuilder CreateOpenUI(this IPageNavigator<UGUIPage, OpenUGUIArgs> navigator, string pageUri)
            => new PageNavigatorOpenUGUIBuilder(navigator, pageUri);
    }
}
