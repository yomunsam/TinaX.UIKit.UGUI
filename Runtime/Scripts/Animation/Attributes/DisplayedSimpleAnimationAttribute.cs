using System;

namespace TinaX.UIKit.UGUI.Animation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DisplayedSimpleAnimationAttribute : SimpleAnimationAttribute
    {
        public DisplayedSimpleAnimationAttribute(string name) : base(name, AnimationType.Displayed) { }
    }
}
