using System.Collections;
using HuyDu_UISystem;

namespace Project.Screens{
    public sealed class LevelSelectionMenuPresenter : PagePresenter<LevelSelectionPage, LevelSelectionView, LevelSelectionViewState>
    {
        public LevelSelectionMenuPresenter(LevelSelectionPage view) : base(view)
        {
        }

        protected override IEnumerator ViewDidLoad(LevelSelectionPage view, LevelSelectionViewState viewState)
        {
            return base.ViewDidLoad(view, viewState);
        }
    }
}