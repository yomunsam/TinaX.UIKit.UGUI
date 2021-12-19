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
    }
}
