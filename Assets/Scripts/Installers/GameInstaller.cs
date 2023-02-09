using Card;
using Game;
using Holder;
using InputSystem;
using TimeSystem;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            InstallTime();
            InstallGame();
            InstallController();
            InstallHolder();
            InstallCard();
        }

        private void InstallCard()
        {
            Container.BindInterfacesAndSelfTo<CardRegistry>().AsSingle();
        }

        private void InstallTime()
        {
            Container.BindInterfacesAndSelfTo<TimeService>().AsSingle();
        }

        private void InstallGame()
        {
            Container.BindInterfacesAndSelfTo<GameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameLevelHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameScoreHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameTimeHandler>().AsSingle();
            Container.Bind<GameStateHandler>().AsSingle();
            Container.Bind<GameOverHandler>().AsSingle();
        }

        private void InstallHolder()
        {
            Container.BindInterfacesAndSelfTo<HolderService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HolderCardHandler>().AsSingle();
            Container.Bind<HolderClearHandler>().AsSingle();
            Container.Bind<HolderItemHandler>().AsSingle();
        }

        private void InstallController()
        {
            Container.BindInterfacesAndSelfTo<PCInput>().AsSingle();
            Container.BindInterfacesTo<PlayerController>().AsSingle();
        }
    }
}