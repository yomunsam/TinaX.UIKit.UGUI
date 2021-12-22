using System;
using TinaX.Options;
using TinaX.UIKit.Builder;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Builder;
using TinaX.UIKit.UGUI.Options;

namespace TinaX.Services
{
    public static class UIKitBuilderExtensions
    {
        public static UIKitBuilder AddUGUI(this UIKitBuilder uikitBuilder)
        {
            uikitBuilder.AddUGUI(builder => { });
            return uikitBuilder;
        }

        public static UIKitBuilder AddUGUI(this UIKitBuilder uikitBuilder, Action<UIKitUGUIBuilder> uguiBuilder)
        {
            //---------------------------------------------------------------------------------
            //因为这个Builder不复杂，所有我们就直接在这儿顺便实现builder模式里的Director，不另外写个class了
            var builder = new UIKitUGUIBuilder(uikitBuilder.Services);
            uguiBuilder?.Invoke(builder);

            //Options
            if(!uikitBuilder.Services.TryGet<IOptions<UIKitUGUIOptions>>(out _))
            {
                uikitBuilder.Services.AddOptions();
                uikitBuilder.Services.Configure<UIKitUGUIOptions>(options => { });
            }

            //UIKit Provider
            var provider = new UIKitUGUIProvider(builder.AwakeBehaviours, builder.StartBehaviours);
            uikitBuilder.AddUIKitProvider(provider);

            return uikitBuilder;
        }

    }
}
