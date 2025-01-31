using System.Collections;
using UnityEngine.Assertions;
using UnityScreenNavigator.Runtime.Core.Page;

namespace HuyDu_UISystem
{
    public abstract class Page<TView, TState> : Page
        where TView : AppView<TState>
        where TState : AppViewState
    {
        public TView root;
        private bool _isInitialized;
        private TState _state;

        protected virtual ViewInitializationTiming RootInitializationTiming =>
            ViewInitializationTiming.BeforeFirstEnter;

        public void Setup(TState state)
        {
            _state = state;
        }
        public override IEnumerator Initialize()
        {
            Assert.IsNotNull(root);

            yield return base.Initialize();

            if (RootInitializationTiming == ViewInitializationTiming.Initialize && !_isInitialized)
            {
                yield return root.Initialize(_state);
                _isInitialized = true;
            }
        }

        public override IEnumerator WillPushEnter()
        {
            Assert.IsNotNull(root);

            yield return base.WillPushEnter();

            if (RootInitializationTiming == ViewInitializationTiming.BeforeFirstEnter && !_isInitialized)
            {
                yield return root.Initialize(_state);
                _isInitialized = true;
            }
        }
    }
}