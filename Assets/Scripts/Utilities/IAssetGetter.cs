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

    public readonly struct AddressableAssetGetter<TObject> : IAssetGetter<TObject> where TObject : Object
    {
        private readonly string m_assetAddress;
        public AddressableAssetGetter(string assetAddress) => m_assetAddress = assetAddress;

        public void Dispose()
        {
            Addressables.Release(m_assetAddress);
        }

        public Task<TObject> GetAsync()
        {
            return Addressables.LoadAssetAsync<TObject>(m_assetAddress).Task;
        }
    }
}