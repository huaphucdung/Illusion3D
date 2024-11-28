using System;
using UnityEngine;

namespace Project.Bootstrapper{
    public class ApplicationRoot : MonoBehaviour{
        private static ApplicationRoot s_instance;

        [Header("Initializers"), SerializeField] MonoInitializer[] m_initializers;
        [Header("Dependency Factories"), SerializeField] ScriptableDependencyFactory[] m_dependencyFactories;

        void Awake(){
            if(s_instance == null){
                s_instance = this;
                LaunchApp();
                DontDestroyOnLoad(gameObject);
            }
            else{
                Destroy(gameObject);
            }
        }

        private void LaunchApp()
        {
            var dependencyContainer = new DependencyContainer();
            BuildDependencies(dependencyContainer);
            BootInitializers(dependencyContainer);
        }

        private void BootInitializers(DependencyContainer dependencyContainer)
        {
            foreach (IInitializer initializer in m_initializers){
                initializer.Initialize(dependencyContainer);
            }
        }

        private void BuildDependencies(DependencyContainer dependencyContainer)
        {
            IDependencyRegistry registry = dependencyContainer;
            foreach (ScriptableDependencyFactory factory in m_dependencyFactories){
                factory.RegisterDependencies(dependencyContainer);
            }
            

            registry.RegisterDependency<Persistence.IPersistenceService>(new Persistence.FileService(Application.persistentDataPath, new Persistence.JsonSerializer()));
        }
    }
}