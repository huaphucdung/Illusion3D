using System.Collections;
using Project.Domain.MapLevel;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Project.Repository{

    public sealed class AddressableMapRepository<TMapTable> : IMapRepository where TMapTable : Object, IMapTable{
        private readonly AssetReferenceT<TMapTable> m_mapTableAsset;
        private bool m_isInitialized;
        private TMapTable m_scriptableMapTable;
        public IMapTable MapTable => m_isInitialized == false ? throw new System.Exception("MapTable is not initialized") : m_scriptableMapTable;

        public AddressableMapRepository(AssetReferenceT<TMapTable> mapTableAsset){
            m_mapTableAsset = mapTableAsset;
        }
        public IEnumerator FetchMapTable(){
            if(m_isInitialized) yield break;
            yield return m_mapTableAsset.LoadAssetAsync<TMapTable>();
            m_isInitialized = true;
            m_scriptableMapTable = m_mapTableAsset.Asset as TMapTable;
            yield return m_scriptableMapTable.Initialize();
        }

        public void Dispose(){
            m_isInitialized = false;
            m_scriptableMapTable = null;
            m_mapTableAsset.ReleaseAsset();
        }
    }
}