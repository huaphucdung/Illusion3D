using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Project.Screens{
    internal static class AddressableReferenceContainer<T> where T : UnityEngine.Object{
        static readonly Dictionary<string, AddressableReference> m_References = new Dictionary<string, AddressableReference>();
        public static void AddReference(IAddressableProperty<T> property){
            if(false == m_References.TryGetValue(property.Address, out var reference)){
                reference = new AddressableReference(property.Address);
                m_References.Add(property.Address, reference);
            }

            reference.AddReference(property);
        }

        public static void RemoveReference(IAddressableProperty<T> property){
            if(m_References.TryGetValue(property.Address, out var reference)){
                reference.RemoveReference(property);
                if(reference.RefCount == 0){
                    reference.Dispose();
                    m_References.Remove(property.Address);
                }
            }
        }

        private class AddressableReference{
            private readonly AsyncOperationHandle<T> m_Handle;
            readonly List<IAddressableProperty<T>> m_References = new List<IAddressableProperty<T>>();
            public int RefCount => m_References.Count;
            public AddressableReference(string address){
                m_Handle = Addressables.LoadAssetAsync<T>(address);
                m_Handle.Completed += HandleCompleted;
            }

            private void HandleCompleted(AsyncOperationHandle<T> handle)
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var reference in m_References)
                    {
                        reference.SetValue(handle.Result);
                    }
                }
            }

            public void AddReference(IAddressableProperty<T> property){
                if(m_Handle.IsDone){
                    property.SetValue(m_Handle.Result);
                }
                m_References.Add(property);
            }

            public void RemoveReference(IAddressableProperty<T> property){
                m_References.Remove(property);
            }

            public void Dispose(){
                if(m_Handle.IsValid()){
                    m_Handle.Completed -= HandleCompleted;
                    Addressables.Release(m_Handle);   
                }
            }
        }
    }
}