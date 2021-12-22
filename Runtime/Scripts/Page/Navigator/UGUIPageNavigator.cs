using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.Args;
using TinaX.UIKit.Page.Navigator;
using TinaX.UIKit.UGUI.Helper;
using TinaX.UIKit.UGUI.Packages.io.nekonya.tinax.uikit.ugui.Runtime.Scripts.Services.Args;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit.UGUI.Page.Navigator
{
    public class UGUIPageNavigator : PageNavigator, IPageNavigator<UGUIPage, OpenUGUIArgs>
    {
        public UGUIPageNavigator(UGUIPage page, IUIKit uikit, IUIKitUGUI uiKitUGUI) : base(page, uikit)
        {

        }

        public UniTask<UGUIPage> OpenUIAsync(OpenUGUIArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessArgsBeforeGetPage(ref OpenUIArgs args)
        {
            if(!PageUriHelper.IsMatch(args.PageUri))
            {
                if (PageUriHelper.IsRelativePath(args.PageUri))
                    args.PageUri = PageUriHelper.SpliceRelativePath(PageUriHelper.GetParent(m_Page.PageUri), args.PageUri); //处理相对路径
            }
        }
    }
}
