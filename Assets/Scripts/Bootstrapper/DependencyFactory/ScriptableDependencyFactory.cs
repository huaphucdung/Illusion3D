namespace Project.Bootstrapper{
    public abstract class ScriptableDependencyFactory : UnityEngine.ScriptableObject, IDependencyFactory{
        public abstract void RegisterDependencies(DependencyContainer container);
    }
}