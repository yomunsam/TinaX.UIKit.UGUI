using System;
using TinaX.Options;
using TinaX.UIKit.Builder;
using TinaX.UIKit.UGUI;
using TinaX.UIKit.UGUI.Options;

namespace TinaX.Services
{
    public static class UIKitOptionsExtensions
    {
        public static UIKitBuilder AddUGUI(this UIKitBuilder uikitBuilder)
        {
            uikitBuilder.AddUIKitProvider(new UIKitUGUIProvider());
            uikitBuilder.Services.AddOptions();
            uikitBuilder.Services.Configure<UIKitUGUIOptions>(options =>
            {

            });
            return uikitBuilder;
        }

        public static UIKitBuilder AddUGUI(this UIKitBuilder uikitBuilder, Action<UIKitUGUIOptions> options)
        {
            uikitBuilder.AddUIKitProvider(new UIKitUGUIProvider());
            uikitBuilder.Services.AddOptions();
            uikitBuilder.Services.Configure<UIKitUGUIOptions>(options);
            return uikitBuilder;
        }

    }
}
