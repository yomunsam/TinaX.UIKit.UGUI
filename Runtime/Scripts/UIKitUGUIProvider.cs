using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TinaX.Container;
using TinaX.Core.Behaviours;
using TinaX.Systems.Pipeline;
using TinaX.UIKit.Pipelines.GetUIPage;
using TinaX.UIKit.Providers.UIKitProvider;
using TinaX.UIKit.UGUI.Consts;
using TinaX.UIKit.UGUI.Pipelines.GetUIPage;
using TinaX.UIKit.UGUI.Services;

namespace TinaX.UIKit.UGUI
{
    public class UIKitUGUIProvider : IUIKitProvider
    {
        private readonly List<IAwakeBehaviour> m_AwakeBehaviours;
        private readonly List<IStartBehaviour> m_StartBehaviours;

        public UIKitUGUIProvider(List<IAwakeBehaviour> awakeBehaviours, List<IStartBehaviour> startBehaviours)
        {
            this.m_AwakeBehaviours = awakeBehaviours;
            this.m_StartBehaviours = startBehaviours;
        }

        public string ProviderName => UIKitUGUIConsts.ProviderName;

        public void ConfigureServices(IServiceContainer services) //注册服务
        {
            services.Singleton<IUGUIKit, UGUIKitService>().SetAlias<IUGUIKitInternalService>();
        }

        public void ConfigureBehaviours(IBehaviourManager behaviour, IServiceContainer services)
        {
            for(int i = 0; i < m_AwakeBehaviours.Count; i++)
            {
                behaviour.RegisterAwake(m_AwakeBehaviours[i]);
            }

            for (int i = 0; i < m_StartBehaviours.Count; i++)
            {
                behaviour.RegisterStart(m_StartBehaviours[i]);
            }
        }

        public async UniTask StartAsync(IServiceContainer services, CancellationToken cancellationToken = default) //启动服务
        {
            var uGuiUIKit = services.Get<IUGUIKitInternalService>();
            await uGuiUIKit.StartAsync(cancellationToken);
        }

        public void ConfigureGetUIPagePipeline(ref XPipeline<IGetUIPageAsyncHandler> pipeline, ref IServiceContainer services) //配置“GetUIPage”管线
        {
            pipeline.AddLast(new GetUIPageHandler());
        }
    }
}
