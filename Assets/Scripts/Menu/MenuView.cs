using System;
using Cysharp.Threading.Tasks;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Menu
{
    public class MenuView : MonoBehaviour
    {
        [Inject] private readonly ISoundService      soundService;
        [Inject] private readonly ISceneService      sceneService;
        
        [Inject] private readonly MenuStateHandler   stateHandler;

        [SerializeField] private AssetReferenceT<AudioClip> clip_BGM;

        private async void Start()
        {
            Application.targetFrameRate = 30;

            soundService.DoPlayBGM(clip_BGM);

            stateHandler.ChangeState(MenuState.Title);

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            
            sceneService.DoFadeOut();
        }
    }
}