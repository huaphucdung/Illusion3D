using System;
using System.Collections;
using HuyDu_UISystem;
using UnityEngine;

namespace Project.Screens{
    public sealed class LevelSelectionView : AppView<LevelSelectionViewState>
    {
        [SerializeField] RectTransform itemContainer;
        [SerializeField] private LevelSelectionItemView itemViewPrefab;
        private LevelSelectionItemView[] m_itemViews;
        protected override IEnumerator InitializeInternal(LevelSelectionViewState state)
        {
            int count = state.ItemStates.Count;
            m_itemViews = new LevelSelectionItemView[count];
            for(int i = 0; i < count; ++i){                
                LevelSelectionItemView itemView = Instantiate(itemViewPrefab, itemContainer);
                m_itemViews[i] = itemView;
                StartCoroutine(itemView.Initialize(state.ItemStates[i]));
            }
            yield break;
        }
    }
}