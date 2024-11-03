using Project.Utilities;
using UnityEngine;

namespace Project.Screens
{
    public sealed class LevelSelectionItemViewState : HuyDu_UISystem.AppViewState
    {
        public IAssetGetter<Sprite> LevelThumbnail;
        public string LevelName;

        public bool IsUnlocked;

        protected override void DisposeInternal()
        {
            LevelThumbnail.Dispose();
        }
    }
}