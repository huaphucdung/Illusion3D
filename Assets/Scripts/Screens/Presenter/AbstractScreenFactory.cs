using UnityScreenNavigator.Runtime.Core.Page;
namespace Project.Screens
{
    public abstract class AbstractScreenFactory<TPage, TPresenter>
        where TPresenter : HuyDu_UISystem.PagePresenter<TPage>
        where TPage : Page
    {
        private readonly string m_pageId;

        public AbstractScreenFactory(string pageId) => m_pageId = pageId;

        protected abstract TPresenter CreatePresenter(TPage page);
        public void PushScreenTo(PageContainer container, bool playAnimation = true, bool stack = true){
            container.Push<TPage>(m_pageId, playAnimation, stack, onLoad: OnScreenLoaded);
        }

        private void OnScreenLoaded((string pageId, TPage page) tuple){
            TPresenter presenter = CreatePresenter(tuple.page);
            presenter.Initialize();
        }
    }
}