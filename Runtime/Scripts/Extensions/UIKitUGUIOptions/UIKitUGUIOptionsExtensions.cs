using TinaX.UIKit.UGUI.Animation;
using TinaX.UIKit.UGUI.Options;

namespace TinaX.UIKit
{
    public  static class UIKitUGUIOptionsExtensions
    {
        /// <summary>
        /// 添加简单动画
        /// </summary>
        /// <param name="options"></param>
        /// <param name="playDelegate"></param>
        /// <returns></returns>
        public static UIKitUGUIOptions AddSimpleAnimation(this UIKitUGUIOptions options ,string name, PlaySimpleAnimation playDelegate)
        {
            if (!options.PlaySimpleAnimationGateways.ContainsKey(name))
                options.PlaySimpleAnimationGateways.Add(name, playDelegate);
            return options;
        }
    }
}
