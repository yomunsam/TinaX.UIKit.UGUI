using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Core.Behaviours;
using TinaX.UIKit.Providers.UIKitProvider;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit.UGUI
{
    public class UIKitUGUIProvider : IUIKitProvider
    {

        public UIKitUGUIProvider()
        {
        }

        public string ProviderName => UIKitUGUIConsts.ProviderName;

        public void ConfigureServices(IServiceContainer services) //注册服务
        {
            services.Singleton<IUIKitUGUI, UIKitUGUIService>().SetAlias<IUIKitUGUIInternalService>();
        }

        public void ConfigureBehaviours(IBehaviourManager behaviour, IServiceContainer services)
        {
        }

        public async UniTask StartAsync(IServiceContainer services, CancellationToken cancellationToken = default) //启动服务
        {
            var uGuiUIKit = services.Get<IUIKitUGUIInternalService>();
            await uGuiUIKit.StartAsync(cancellationToken);
        }
    }
}
