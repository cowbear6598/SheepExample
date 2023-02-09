using Card;
using UnityEngine;
using Zenject;

namespace Card
{
    public class CardInstaller : MonoInstaller<CardInstaller>
    {
        [SerializeField] private CardLockSetting lockSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CardService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CardMoveHandler>().AsSingle();
            Container.Bind<CardStateHandler>().AsSingle();
            Container.Bind<CardTypeHandler>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CardLockHandler>()
                     .AsSingle()
                     .WithArguments(lockSettings.unlockCount, lockSettings.lockCardViews);
        }
    }
}