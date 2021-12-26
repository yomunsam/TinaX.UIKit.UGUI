using TinaX.UIKit.Page.Navigator;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Builder.OpenUGUIBuilders;
using TinaX.UIKit.UGUI.Page;

namespace TinaX.UIKit
{
    public static class UGUIPageNavigatorExtensions
    {
        /// <summary>
        /// 创建一个打开UI的构建器
        /// </summary>
        /// <param name="navigator"></param>
        /// <param name="pageUri"></param>
        /// <returns></returns>
        public static UGUIPageNavigatorOpenUIBuilder CreateOpenUI(this IPageNavigator<IUGUIPage, OpenUGUIArgs> navigator, string pageUri)
            => new UGUIPageNavigatorOpenUIBuilder(navigator, pageUri);
    }
}
