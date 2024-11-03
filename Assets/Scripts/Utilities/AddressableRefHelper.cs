using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Utilities{

    public static class AddressableRefHelper{
        private static readonly Dictionary<string, AssetRefCounter> s_referenceCount = new();

        public static Task<TObject> GetAssetAsync<TObject>(AssetReferenceT<TObject> reference)
        where TObject : UnityEngine.Object
        {
            return GetAsset<TObject>(reference.AssetGUID);
        }

        public static void ReleaseAsset<TObject>(AssetReferenceT<TObject> reference)
        where TObject : UnityEngine.Object
        {
            ReleaseAsset(reference);
        }

        public static void ReleaseAsset(AssetReference reference) => ReleaseAsset(reference.AssetGUID);

        public static Task<TObject> GetAssetAsync<TObject>(in AddressableAssetGetter<TObject> getter)
        where TObject : UnityEngine.Object
        {
            return GetAsset<TObject>(getter.AssetAddress);
        }

        public static void ReleaseAsset<TObject>(in AddressableAssetGetter<TObject> assetGetter)
        where TObject : UnityEngine.Object
        {
            ReleaseAsset(assetGetter.AssetAddress);
        }

        private static Task<TObject> GetAsset<TObject>(string assetAddress)
        where TObject : UnityEngine.Object
        {
            if(!s_referenceCount.TryGetValue(assetAddress, out AssetRefCounter counter)){
                counter = new AssetRefCounter(Addressables.LoadAssetAsync<TObject>(assetAddress));
                s_referenceCount.Add(assetAddress, counter);
            }
            else{
                counter.IncreaseRef();
            }

            return counter.GetAssetAsync<TObject>();
        }

        private static void ReleaseAsset(string assetAddress){
            if(false == s_referenceCount.TryGetValue(assetAddress, out AssetRefCounter counter)){
                return; 
            }

            counter.DecreaseRef();
        }


        private class AssetRefCounter{
            readonly AsyncOperationHandle m_handle;
            private int m_referenceCount;
            public AssetRefCounter(AsyncOperationHandle handle){
                m_handle = handle;
                m_referenceCount = 1;
            }

            public void IncreaseRef() => ++m_referenceCount;
            public void DecreaseRef(){
                --m_referenceCount;
                if(0 == m_referenceCount){
                    Dispose();
                }
            }

            private void Dispose(){
                if(m_handle.IsValid()){
                    Addressables.Release(m_handle);
                }
            }

            internal Task<TObject> GetAssetAsync<TObject>() where TObject : UnityEngine.Object
            {
                AsyncOperationHandle<TObject> handleT = m_handle.Convert<TObject>();
                if(handleT.IsValid() && handleT.Status == AsyncOperationStatus.Succeeded){
                    return Task.FromResult(handleT.Result);
                }

                return handleT.Task;
            }
        }
    }
}