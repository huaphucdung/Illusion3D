using UnityEngine;
using Zenject;

public class BlockGroupInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // Bind BlockGroup from the current GameObject To Walkable is child of this GameObject
        Container.Bind<BlockGroup>().FromComponentOn(gameObject).AsSingle().WhenInjectedInto<Walkable>();
    }
}