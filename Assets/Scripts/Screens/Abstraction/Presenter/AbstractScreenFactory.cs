using System;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
namespace Project.Screens
{
    public abstract class AbstractPageFactory<TPage, TPresenter>
        where TPresenter : HuyDu_UISystem.PagePresenter<TPage>
        where TPage : Page
    {
        private readonly string m_pageId;

        public AbstractPageFactory(string pageId) => m_pageId = pageId;

        protected abstract TPresenter CreatePresenter(TPage page);
        public void PushScreenTo(PageContainer container, bool playAnimation = true, bool stack = true){
            container.Push<TPage>(m_pageId, playAnimation, stack, onLoad: OnScreenLoaded);
        }

        private void OnScreenLoaded((string pageId, TPage page) tuple){
            TPresenter presenter = CreatePresenter(tuple.page);
            OnPresenterCreated(presenter);
        }

        protected virtual void OnPresenterCreated(TPresenter presenter){
            presenter.Initialize();
        }
    }

    public abstract class AbstractModalFactory<TModal, TPresenter>
        where TPresenter : HuyDu_UISystem.ModalPresenter<TModal>
        where TModal : Modal{
            private readonly string m_modalId;

            public AbstractModalFactory(string modalId) => m_modalId = modalId;
            protected abstract TPresenter CreatePresenter(TModal modal);
            public void PushModalTo(ModalContainer container, bool playAnimation = true){
                container.Push<TModal>(m_modalId, playAnimation, onLoad: OnModalLoaded);
            }

        private void OnModalLoaded((string modalId, TModal modal) tuple)
        {
            TPresenter presenter = CreatePresenter(tuple.modal);
            OnPresenterCreated(presenter);
        }

        protected virtual void OnPresenterCreated(TPresenter presenter){
            presenter.Initialize();
        }
    }
}