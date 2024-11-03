using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Utilities{
    public interface IAssetGetter<TObject> : System.IDisposable where TObject : Object
    {
        Task<TObject> GetAsync();
    }

    public readonly struct AddressableAssetGetter<TObject> : IAssetGetter<TObject>, System.IEquatable<AddressableAssetGetter<TObject>> where TObject : Object
    {
        public readonly string AssetAddress;
        public AddressableAssetGetter(string assetAddress){
            if(string.IsNullOrEmpty(assetAddress)) throw new System.ArgumentNullException(nameof(assetAddress));
            AssetAddress = assetAddress;
        }

        public void Dispose()
        {
            AddressableRefHelper.ReleaseAsset(this);
        }

        public Task<TObject> GetAsync()
        {
            return AddressableRefHelper.GetAssetAsync<TObject>(this);
        }

        public bool Equals(AddressableAssetGetter<TObject> other){
            return AssetAddress.Equals(other.AssetAddress);
        }
    }
}