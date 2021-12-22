using System;
using System.Collections.Generic;
using System.ComponentModel;
using TinaX.Container;
using TinaX.Core.Behaviours;
using TinaX.Options;
using TinaX.UIKit.UGUI.Options;

namespace TinaX.UIKit.UGUI.Builder
{
    public class UIKitUGUIBuilder
    {
        private readonly List<IAwakeBehaviour> m_Awakes = new List<IAwakeBehaviour>();
        private readonly List<IStartBehaviour> m_Starts = new List<IStartBehaviour>();

        public UIKitUGUIBuilder(IServiceContainer services)
            => Services = services ?? throw new ArgumentNullException(nameof(services));

        [EditorBrowsable(EditorBrowsableState.Never)]
        public readonly IServiceContainer Services;



        public UIKitUGUIBuilder Configure(Action<UIKitUGUIOptions> configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.Services.AddOptions();
            this.Services.Configure<UIKitUGUIOptions>(configuration);
            return this;
        }

        public UIKitUGUIBuilder AddAwakeBehaviour(IAwakeBehaviour behaviour)
        {
            m_Awakes.Add(behaviour);
            return this;
        }

        public UIKitUGUIBuilder AddStartBehaviour(IStartBehaviour behaviour)
        {
            m_Starts.Add(behaviour);
            return this;
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<IAwakeBehaviour> AwakeBehaviours => m_Awakes;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public List<IStartBehaviour> StartBehaviours => m_Starts;

    }
}
