using Menu;
using UI;
using Zenject;

namespace Installers
{
    public class MenuInstaller : MonoInstaller<MenuInstaller>
    {
        public override void InstallBindings()
        {
            InstallMenu();
            InstallSetting();
        }

        private void InstallMenu()
        {
            Container.Bind<MenuStateHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuService>().AsSingle();
        }

        private void InstallSetting()
        {
            Container.Bind<SettingHandler>().AsSingle();
        }
    }
}