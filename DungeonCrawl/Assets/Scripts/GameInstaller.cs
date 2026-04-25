using Systems.Dungeon;
using Zenject;

namespace DefaultNamespace
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            RegisterService<IDungeonManager, DungeonManager>();
        }

        private void RegisterService<TService, TImplementation>() where TImplementation : TService
        {
            Container.Bind<TService>().To<TImplementation>().FromComponentsInHierarchy().AsSingle();
        }
    }
}