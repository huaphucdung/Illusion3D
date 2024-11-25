namespace Project.Bootstrapper{
    public abstract class MonoInitializer : UnityEngine.MonoBehaviour, IInitializer
    {
        private void OnDestroy() => Dispose();
        public abstract void Dispose();

        public abstract void Initialize(IDependencyContainer dependencyContainer);
    }
}