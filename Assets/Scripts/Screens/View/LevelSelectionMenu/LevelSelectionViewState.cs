using System.Collections.Generic;
using HuyDu_UISystem;

namespace Project.Screens{
    public sealed class LevelSelectionViewState : AppViewState
    {
        private readonly List<LevelSelectionItemViewState> m_itemStates = new List<LevelSelectionItemViewState>();
        public IReadOnlyList<LevelSelectionItemViewState> ItemStates => m_itemStates;
        public void Clear(){
            m_itemStates.Clear();
        }
        public void AddItemState(LevelSelectionItemViewState state) {
            m_itemStates.Add(state);
        }
        protected override void DisposeInternal()
        {
            foreach (var itemState in m_itemStates)
            {
                itemState.Dispose();
            }
        }
    }
}