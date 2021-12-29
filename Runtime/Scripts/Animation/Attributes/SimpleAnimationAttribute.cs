using System;

namespace TinaX.UIKit.UGUI.Animation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class SimpleAnimationAttribute : System.Attribute
    {
        public SimpleAnimationAttribute(string animationName, AnimationType type)
        {
            AnimationName = animationName;
            Type = type;
        }


        public string AnimationName { get; set; }
        public AnimationType Type { get; set; }

        public enum AnimationType : byte
        {
            Displayed = 0,
            Closed = 1,
        }
    }
}
