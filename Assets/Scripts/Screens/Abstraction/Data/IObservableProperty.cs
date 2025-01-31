using System;
using System.Collections.Generic;

namespace Project.Screens{
    public interface IObservableProperty<T>{
        public T Value {get;}

        void AddObserver(IPropertyObserver<T> observer);
        void RemoveObserver(IPropertyObserver<T> observer);
    }

    public interface IPropertyObserver<T>{
        void OnValueChanged(T value);
    }

    public sealed class ObservableProperty<T> : IObservableProperty<T>
    {
        private event System.Action<T> m_callback;
        private T m_value;
        public T Value => m_value;

        public void AddObserver(IPropertyObserver<T> observer)
        {
            m_callback += observer.OnValueChanged;
        }

        public void RemoveObserver(IPropertyObserver<T> observer)
        {
            m_callback -= observer.OnValueChanged;
        }

        public void SetValue(T value){
            m_value = value;
            m_callback?.Invoke(value);
        }
    }

    public static class ObservablePropertyUtil{
        internal static class DisposableObserverPool<T>{
            private static readonly Queue<DisposableObserver<T>> Pool = new Queue<DisposableObserver<T>>();

            internal static System.IDisposable Get(IObservableProperty<T> observable, System.Action<T> callback){
                DisposableObserver<T> instance; 
                if(Pool.Count > 0){
                    instance = Pool.Dequeue();
                }else{
                    instance = new DisposableObserver<T>();
                }

                instance.Set(observable, callback);
                return instance;
            }

            public static void Release(IPropertyObserver<T> observer){
                if(observer is DisposableObserver<T> disposableObserver){
                    Pool.Enqueue(disposableObserver);
                }
            }
        }
        public static System.IDisposable AddObserver<T>(this IObservableProperty<T> observable, System.Action<T> callback){
            return DisposableObserverPool<T>.Get(observable, callback);
        }

        public static void AddTo(this System.IDisposable disposable, HuyDu_UISystem.IDisposableContainer container){
            container.Add(disposable);
        }

        public static void AddTo(this System.IDisposable disposable, UnityEngine.GameObject obj){
            if(false == obj.TryGetComponent(out HuyDu_UISystem.IDisposableContainer container)){
                container = obj.AddComponent<HuyDu_UISystem.GameObjectDisposable>();
            }

            disposable.AddTo(container);
        }

        private class DisposableObserver<T> : IPropertyObserver<T>, System.IDisposable
        {
            private System.Action<T> m_callback;
            private IObservableProperty<T> m_observable;
            public void Dispose()
            {
                m_observable?.RemoveObserver(this);
                DisposableObserverPool<T>.Release(this);
            }

            public void OnValueChanged(T value) => m_callback?.Invoke(value);

            internal void Set(IObservableProperty<T> observable, Action<T> callback)
            {
                m_observable = observable;
                m_callback = callback;
                m_observable.AddObserver(this);
            }
        }
    }
}