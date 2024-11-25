namespace Project.Bootstrapper{
    public interface IInitializer : System.IDisposable{
        void Initialize(IDependencyContainer dependencyContainer);
    }
}