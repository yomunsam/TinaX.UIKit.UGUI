using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.Options;
using TinaX.UIKit.UGUI.Page;

namespace TinaX.UIKit.UGUI.Services
{
    public interface IUIKitUGUI
    {
        UniTask<UGUIPage> GetUIPageAsync(string pageUri, bool loadViewPrefab = true, CancellationToken cancellationToken = default);
        UniTask<UGUIPage> GetUIPageAsync(GetUGUIPageOptions options, CancellationToken cancellationToken = default);

        /// <summary>
        /// 把UI压到UGUI屏幕空间 的默认简化方法
        /// </summary>
        /// <param name="page"></param>
        void PushScreenUI(UGUIPage page);
    }
}
