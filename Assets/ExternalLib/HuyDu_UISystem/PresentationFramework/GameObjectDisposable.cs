using System;

namespace HuyDu_UISystem{
    public sealed class GameObjectDisposable : UnityEngine.MonoBehaviour, IDisposableContainer
    {
        private readonly System.Collections.Generic.List<IDisposable> m_disposables = QuickListPool<IDisposable>.Get();
        public void Add(IDisposable disposable)
        {
            m_disposables.Add(disposable);
        }

        void OnDestroy(){
            foreach(var disposable in m_disposables){
                disposable.Dispose();
            }

            QuickListPool<IDisposable>.Release(m_disposables);
        }
    }
}