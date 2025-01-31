using System;

namespace HuyDu_UISystem{
    public abstract class AppViewState : System.IDisposable
    {
        private bool m_isDisposed;
        public void Dispose()
        {
            if(m_isDisposed){
                throw new ObjectDisposedException(nameof(AppViewState));
            }

            DisposeInternal();

            m_isDisposed = true;
        }

        protected abstract void DisposeInternal();
    }
}