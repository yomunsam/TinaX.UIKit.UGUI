using TinaX.UIKit.Builder;
using TinaX.UIKit.UGUI;

namespace TinaX.Services
{
    public static class UIKitOptionsExtensions
    {
        public static UIKitBuilder AddUGUI(this UIKitBuilder uikitBuilder)
        {
            uikitBuilder.AddUIKitProvider(new UIKitUGUIProvider());
            return uikitBuilder;
        }

    }
}
