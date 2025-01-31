using System;
using System.Collections;
using System.Collections.Generic;
using UnityScreenNavigator.Runtime.Core.Modal;

namespace HuyDu_UISystem
{
    public abstract class ModalPresenter<TModal, TRootView, TRootViewState> : ModalPresenter<TModal>, IDisposableContainer
    where TModal : Modal<TRootView, TRootViewState>
    where TRootView : AppView<TRootViewState>
    where TRootViewState : AppViewState, new()
    {
        private readonly List<IDisposable> m_disposables;
        private TRootViewState m_state;

        protected ModalPresenter(TModal view) : base(view)
        {
            m_disposables = QuickListPool<IDisposable>.Get();
        }

        protected sealed override void Initialize(TModal view)
        {
            base.Initialize(view);
        }

        protected sealed override IEnumerator ViewDidLoad(TModal view)
        {
            var state = new TRootViewState();
            m_state = state;
            m_disposables.Add(state);
            view.Setup(state);
            return ViewDidLoad(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPushEnter(TModal view)
        {
            return ViewWillPushEnter(view, m_state);
        }

        protected sealed override void ViewDidPushEnter(TModal view)
        {
            ViewDidPushEnter(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPushExit(TModal view)
        {
            return ViewWillPushExit(view, m_state);
        }

        protected sealed override void ViewDidPushExit(TModal view)
        {
            ViewDidPushExit(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPopEnter(TModal view)
        {
            return ViewWillPopEnter(view, m_state);
        }

        protected sealed override void ViewDidPopEnter(TModal view)
        {
            ViewDidPopEnter(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPopExit(TModal view)
        {
            return ViewWillPopExit(view, m_state);
        }

        protected sealed override void ViewDidPopExit(TModal view)
        {
            ViewDidPopExit(view, m_state);
        }

        protected override IEnumerator ViewWillDestroy(TModal view)
        {
            return ViewWillDestroy(view, m_state);
        }

        protected virtual IEnumerator ViewDidLoad(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual IEnumerator ViewWillPushEnter(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPushEnter(TModal view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPushExit(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPushExit(TModal view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPopEnter(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPopEnter(TModal view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPopExit(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPopExit(TModal view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillDestroy(TModal view, TRootViewState viewState)
        {
            yield break;
        }

        protected sealed override void Dispose(TModal view)
        {
            base.Dispose(view);
            foreach (var disposable in m_disposables)
                disposable.Dispose();
            QuickListPool<IDisposable>.Release(m_disposables);
        }

        void IDisposableContainer.Add(IDisposable disposable)
        {
            m_disposables.Add(disposable);
        }

    }

    public abstract class ModalPresenter<TModal> : Presenter<TModal>, IModalPresenter where TModal : Modal
    {
        protected ModalPresenter(TModal view) : base(view)
        {
            m_view = view;
        }

        private readonly TModal m_view;
        IEnumerator IModalLifecycleEvent.Initialize()
        {
            return ViewDidLoad(m_view);
        }
        IEnumerator IModalLifecycleEvent.WillPushEnter()
        {
            return ViewWillPushEnter(m_view);
        }

        void IModalLifecycleEvent.DidPushEnter()
        {
            ViewDidPushEnter(m_view);
        }

        IEnumerator IModalLifecycleEvent.WillPushExit()
        {
            return ViewWillPushExit(m_view);
        }

        void IModalLifecycleEvent.DidPushExit()
        {
            ViewDidPushExit(m_view);
        }

        IEnumerator IModalLifecycleEvent.WillPopEnter()
        {
            return ViewWillPopEnter(m_view);
        }

        void IModalLifecycleEvent.DidPopEnter()
        {
            ViewDidPopEnter(m_view);
        }

        IEnumerator IModalLifecycleEvent.WillPopExit()
        {
            return ViewWillPopExit(m_view);
        }

        void IModalLifecycleEvent.DidPopExit()
        {
            ViewDidPopExit(m_view);
        }

        IEnumerator IModalLifecycleEvent.Cleanup()
        {
            return ViewWillDestroy(m_view);
        }
        protected virtual IEnumerator ViewDidLoad(TModal view)
        {
            yield break;
        }
        protected virtual IEnumerator ViewWillPushEnter(TModal view)
        {
            yield break;
        }

        protected virtual void ViewDidPushEnter(TModal view)
        {
        }

        protected virtual IEnumerator ViewWillPushExit(TModal view)
        {
            yield break;
        }

        protected virtual void ViewDidPushExit(TModal view)
        {
        }

        protected virtual IEnumerator ViewWillPopEnter(TModal view)
        {
            yield break;
        }

        protected virtual void ViewDidPopEnter(TModal view)
        {
        }

        protected virtual IEnumerator ViewWillPopExit(TModal view)
        {
            yield break;
        }

        protected virtual void ViewDidPopExit(TModal view)
        {
        }

        protected virtual IEnumerator ViewWillDestroy(TModal view)
        {
            yield break;
        }

        protected override void Initialize(TModal view)
        {
            // The lifecycle event of the view will be added with priority 0.
            // Presenters should be processed after the view so set the priority to 1.
            view.AddLifecycleEvent(this, 1);
        }

        protected override void Dispose(TModal view)
        {
            view.RemoveLifecycleEvent(this);
        }
    }
}