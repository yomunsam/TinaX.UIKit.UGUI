using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TinaX.UIKit.UGUI.Consts;
using UnityEngine;

namespace TinaX.UIKit.UGUI.Services
{
    public class UIKitUGUIService : IUIKitUGUI, IUIKitUGUIInternalService
    {
        public UIKitUGUIService()
        {

        }

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
#if TINAX_DEV
            Debug.LogFormat("[{0}]开始启动", UIKitUGUIConsts.ProviderName);
#endif
            await Task.CompletedTask;
        }

    }
}
