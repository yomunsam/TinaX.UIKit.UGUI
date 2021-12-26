using System.Threading;
using Cysharp.Threading.Tasks;

namespace TinaX.UIKit.UGUI.Services
{
    /// <summary>
    /// UIKit (uGUI) 内部服务 接口 （好拗口）
    /// </summary>
    internal interface IUGUIKitInternalService
    {
        UniTask StartAsync(CancellationToken cancellationToken = default);
    }
}
