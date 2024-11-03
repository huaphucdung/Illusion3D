using HuyDu_UISystem;

namespace Project.Screens
{
    public sealed class LevelSelectionPage : Page<LevelSelectionView, LevelSelectionViewState>{
        public override void DidPushEnter()
        {
            root.ValidateUnlockState();
        }
    }
}