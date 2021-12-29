using System.Collections.Generic;

namespace TinaX.UIKit.UGUI.Animation
{
    public class SimpleAnimationManager
    {
        private readonly Dictionary<string, PlaySimpleAnimation> m_PlaySimpleAnimations;

        public SimpleAnimationManager(IDictionary<string, PlaySimpleAnimation> delegates)
        {
            m_PlaySimpleAnimations = new Dictionary<string, PlaySimpleAnimation>(delegates);
        }

        public bool TryGet(string name, out PlaySimpleAnimation playDelegate)
            => m_PlaySimpleAnimations.TryGetValue(name, out playDelegate);

    }
}
