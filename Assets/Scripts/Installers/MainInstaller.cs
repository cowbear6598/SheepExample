using System;
using CameraSystem;
using SoapUtils.NotifySystem;
using SoapUtils.PrefsSystem;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Installers
{
    public class MainInstaller : MonoInstaller<MainInstaller>
    {
        [SerializeField] private BasicComponent basicComponent;
        
        public override void InstallBindings()
        {
            SignalInstaller.Install(Container);

            Container.BindInstance(basicComponent).IfNotBound();

            InstallCameraSystem();
            
            InstallScene();
            InstallNotify();
            InstallSound();
            InstallPrefs();
        }

        private void InstallPrefs()
        {
            Container.BindInterfacesAndSelfTo<PrefsService>().AsSingle();
        }

        private void InstallCameraSystem()
        {
            Container.BindInterfacesAndSelfTo<CameraView>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallScene()
        {
            Container.Bind<LoadHandler>().AsSingle();
            Container.Bind<StateHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
            
            Container.Bind<SceneView>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallNotify()
        {
            Container.Bind<NotifyView>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<NotifyService>().AsSingle();
        }

        private void InstallSound()
        {
            Container.Bind<SoundView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<EffectHandler>().AsSingle();
            Container.Bind<BGMHandler>().AsSingle();
            Container.Bind<LoopHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
        }
    }
}

[Serializable]
public class BasicComponent
{
    public AudioMixer audioMixer;
}