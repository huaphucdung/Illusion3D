namespace HuyDu_UISystem{
    public abstract class AppView<TState> : UnityEngine.MonoBehaviour where TState : AppViewState {
        private bool m_isInitialized;
        public System.Collections.IEnumerator Initialize(TState state){
            if(m_isInitialized) yield break;

            m_isInitialized = true;

            yield return InitializeInternal(state);
        }

        protected abstract System.Collections.IEnumerator InitializeInternal(TState state);
    }
}