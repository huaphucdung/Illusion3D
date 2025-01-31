using System.Collections.Generic;

namespace Project.Screens{
    internal interface IAddressableProperty<T> where T : UnityEngine.Object{
        string Address { get; }
        void SetValue(T value);
    }
    internal sealed class AddressableObservableProperty<T> : IObservableProperty<T>, IAddressableProperty<T> where T : UnityEngine.Object
    {
        public T Value {get; private set;}

        public string Address {get; private set;}
        private event System.Action<T> OnValueChanged;

        public AddressableObservableProperty(string address){
            Address = address;
        }

        public void AddObserver(IPropertyObserver<T> observer)
        {
            if(Value != null) observer.OnValueChanged(Value);
            OnValueChanged += observer.OnValueChanged;
        }

        public void RemoveObserver(IPropertyObserver<T> observer)
        {
            OnValueChanged -= observer.OnValueChanged;
        }

        public void SetValue(T value)
        {
            Value = value;
            OnValueChanged?.Invoke(value);
        }
    }
}