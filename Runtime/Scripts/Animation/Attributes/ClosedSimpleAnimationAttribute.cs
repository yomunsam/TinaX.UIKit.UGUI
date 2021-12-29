using System;

namespace TinaX.UIKit.UGUI.Animation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ClosedSimpleAnimationAttribute : SimpleAnimationAttribute
    {
        public ClosedSimpleAnimationAttribute(string name) : base(name, AnimationType.Closed) { }
    }
}
