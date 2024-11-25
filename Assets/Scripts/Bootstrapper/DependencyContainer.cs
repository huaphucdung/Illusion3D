using System;
using System.Collections.Generic;

namespace Project.Bootstrapper{
    public interface IDependencyContainer{
        TInterface GetDependency<TInterface>() where TInterface : class;
        bool TryGetDependency<TInterface>(out TInterface dependency) where TInterface : class;
        bool HasDependency<TInterface>() where TInterface : class;
    }
    public interface IDependencyRegistry{
        bool RegisterDependency<TInterface>(TInterface dependency) where TInterface : class;
    }
    public sealed class DependencyContainer : IDependencyContainer, IDependencyRegistry{
        private readonly Dictionary<Type, object> m_container = new Dictionary<Type, object>();

        TInterface IDependencyContainer.GetDependency<TInterface>() where TInterface : class{
            return m_container[typeof(TInterface)] as TInterface;
        }
        bool IDependencyContainer.TryGetDependency<TInterface>(out TInterface dependency) where TInterface : class{
            bool hasDependency = m_container.TryGetValue(typeof(TInterface), out object obj);
            dependency = obj as TInterface;
            return hasDependency;
        } 

        bool IDependencyRegistry.RegisterDependency<TInterface>(TInterface dependency) where TInterface : class{
            return m_container.TryAdd(typeof(TInterface), dependency);
        }

        bool IDependencyContainer.HasDependency<TInterface>() where TInterface : class{
            return m_container.ContainsKey(typeof(TInterface));
        }
    }
}