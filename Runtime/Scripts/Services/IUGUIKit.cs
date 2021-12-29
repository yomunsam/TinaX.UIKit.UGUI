using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Animation;
using TinaX.UIKit.UGUI.MultipleDisplay;
using TinaX.UIKit.UGUI.Page;
using TinaX.UIKit.UGUI.Page.Group;

namespace TinaX.UIKit
{
#nullable enable
    public interface IUGUIKit
    {
        SimpleAnimationManager SimpleAnimationManager { get; }

        UniTask<UGUIPage> GetUIPageAsync(string pageUri, bool loadViewPrefab = true, CancellationToken cancellationToken = default);
        UniTask<UGUIPage> GetUIPageAsync(GetUGUIPageArgs args, CancellationToken cancellationToken = default);


        
        /// <summary>
        /// 把UI压到UGUI屏幕空间 的默认简化方法
        /// </summary>
        /// <param name="page">UI Page</param>
        /// <param name="displayIndex">你想把UI显示到哪一页屏幕上?（多Display时）不指定的话，会跑到默认的地方</param>
        /// <param name="pushArgs">相关参数</param>
        void PushScreenUI(UGUIPage page, DisplayIndex? displayIndex = null, PushUGUIPageArgs? pushArgs = null);
    }
#nullable restore
}
