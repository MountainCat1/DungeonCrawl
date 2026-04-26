using DefaultNamespace.Cards;
using DefaultNamespace.Systems;
using Systems.Dungeon;
using Zenject;

namespace DefaultNamespace
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            RegisterFloatingService<ISpawnManager, SpawnManager>();
            
            RegisterService<IDungeonManager, DungeonManager>();
            RegisterService<IPlayerManager, PlayerManager>();
            RegisterService<ICardPlaySystem, CardPlaySystem>();
        }

        private void RegisterService<TService, TImplementation>() where TImplementation : TService
        {
            Container.Bind<TService>().To<TImplementation>().FromComponentsInHierarchy().AsSingle();
        }
        
        private void RegisterFloatingService<TService, TImplementation>() where TImplementation : TService
        {
            Container.Bind<TService>().To<TImplementation>().FromNew().AsSingle();
        }
    }
}