using System;
using System.Collections;
using System.Collections.Generic;
using UnityScreenNavigator.Runtime.Core.Page;
namespace HuyDu_UISystem{

    public abstract class PagePresenter<TPage, TRootView, TRootViewState> : PagePresenter<TPage>
        where TPage : Page<TRootView, TRootViewState>
        where TRootView : AppView<TRootViewState>
        where TRootViewState : AppViewState, new()
    {
        private readonly List<IDisposable> m_disposables = new List<IDisposable>();
        private TRootViewState m_state;

        protected PagePresenter(TPage view) : base(view)
        {
        }

        protected sealed override void Initialize(TPage view)
        {
            base.Initialize(view);
        }

        protected sealed override IEnumerator ViewDidLoad(TPage view)
        {
            yield return base.ViewDidLoad(view);
            var state = new TRootViewState();
            m_state = state;
            m_disposables.Add(state);
            view.Setup(state);
            yield return ViewDidLoad(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPushEnter(TPage view)
        {
            yield return base.ViewWillPushEnter(view);
            yield return ViewWillPushEnter(view, m_state);
        }

        protected sealed override void ViewDidPushEnter(TPage view)
        {
            base.ViewDidPushEnter(view);
            ViewDidPushEnter(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPushExit(TPage view)
        {
            yield return base.ViewWillPushExit(view);
            yield return ViewWillPushExit(view, m_state);
        }

        protected sealed override void ViewDidPushExit(TPage view)
        {
            base.ViewDidPushExit(view);
            ViewDidPushExit(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPopEnter(TPage view)
        {
            yield return base.ViewWillPopEnter(view);
            yield return ViewWillPopEnter(view, m_state);
        }

        protected sealed override void ViewDidPopEnter(TPage view)
        {
            base.ViewDidPopEnter(view);
            ViewDidPopEnter(view, m_state);
        }

        protected sealed override IEnumerator ViewWillPopExit(TPage view)
        {
            yield return base.ViewWillPopExit(view);
            yield return ViewWillPopExit(view, m_state);
        }

        protected sealed override void ViewDidPopExit(TPage view)
        {
            base.ViewDidPopExit(view);
            ViewDidPopExit(view, m_state);
        }

        protected override IEnumerator ViewWillDestroy(TPage view)
        {
            yield return base.ViewWillDestroy(view);
            yield return ViewWillDestroy(view, m_state);
        }

        protected virtual IEnumerator ViewDidLoad(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual IEnumerator ViewWillPushEnter(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPushEnter(TPage view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPushExit(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPushExit(TPage view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPopEnter(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPopEnter(TPage view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillPopExit(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected virtual void ViewDidPopExit(TPage view, TRootViewState viewState)
        {
        }

        protected virtual IEnumerator ViewWillDestroy(TPage view, TRootViewState viewState)
        {
            yield break;
        }

        protected sealed override void Dispose(TPage view)
        {
            base.Dispose(view);
            foreach (var disposable in m_disposables)
                disposable.Dispose();
        }
    }

     public abstract class PagePresenter<TPage> : Presenter<TPage>, IPagePresenter where TPage : Page
    {
        protected PagePresenter(TPage view) : base(view)
        {
            View = view;
        }

        private TPage View { get; }
        IEnumerator IPageLifecycleEvent.Initialize()
        {
            return ViewDidLoad(View);
        }
        IEnumerator IPageLifecycleEvent.WillPushEnter()
        {
            return ViewWillPushEnter(View);
        }

        void IPageLifecycleEvent.DidPushEnter()
        {
            ViewDidPushEnter(View);
        }

        IEnumerator IPageLifecycleEvent.WillPushExit()
        {
            return ViewWillPushExit(View);
        }

        void IPageLifecycleEvent.DidPushExit()
        {
            ViewDidPushExit(View);
        }

        IEnumerator IPageLifecycleEvent.WillPopEnter()
        {
            return ViewWillPopEnter(View);
        }

        void IPageLifecycleEvent.DidPopEnter()
        {
            ViewDidPopEnter(View);
        }

        IEnumerator IPageLifecycleEvent.WillPopExit()
        {
            return ViewWillPopExit(View);
        }

        void IPageLifecycleEvent.DidPopExit()
        {
            ViewDidPopExit(View);
        }

        IEnumerator IPageLifecycleEvent.Cleanup()
        {
            return ViewWillDestroy(View);
        }
        protected virtual IEnumerator ViewDidLoad(TPage view)
        {
            yield break;
        }
        protected virtual IEnumerator ViewWillPushEnter(TPage view)
        {
            yield break;
        }

        protected virtual void ViewDidPushEnter(TPage view)
        {
        }

        protected virtual IEnumerator ViewWillPushExit(TPage view)
        {
            yield break;
        }

        protected virtual void ViewDidPushExit(TPage view)
        {
        }

        protected virtual IEnumerator ViewWillPopEnter(TPage view)
        {
            yield break;
        }

        protected virtual void ViewDidPopEnter(TPage view)
        {
        }

        protected virtual IEnumerator ViewWillPopExit(TPage view)
        {
            yield break;
        }

        protected virtual void ViewDidPopExit(TPage view)
        {
        }

        protected virtual IEnumerator ViewWillDestroy(TPage view)
        {
            yield break;
        }

        protected override void Initialize(TPage view)
        {
            // The lifecycle event of the view will be added with priority 0.
            // Presenters should be processed after the view so set the priority to 1.
            view.AddLifecycleEvent(this, 1);
        }

        protected override void Dispose(TPage view)
        {
            view.RemoveLifecycleEvent(this);
        }
    }
}