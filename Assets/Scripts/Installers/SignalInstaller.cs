using Game;
using Menu;
using Zenject;

namespace Installers
{
    public class SignalInstaller : Installer<SignalInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<OnMenuChangeState>();

            Container.DeclareSignal<OnGameChangeState>();
            Container.DeclareSignal<OnGameOver>();
        }
    }
}