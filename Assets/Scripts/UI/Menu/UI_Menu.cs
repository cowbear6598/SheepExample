using AnimeTask;
using Menu;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI
{
    public class UI_Menu : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        [Inject] private readonly SignalBus          signalBus;
        [Inject] private readonly ISoundService      soundService;
        [Inject] private readonly ISceneService      sceneService;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Start;

        private bool IsChangingMode;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnMenuChangeState>(OnMenuChangeState);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnMenuChangeState>(OnMenuChangeState);
        }

        public void Button_Start()
        {
            soundService.DoPlaySound(clip_Start);
            sceneService.DoLoadScene(1, false);
        }
        
        private async void SetAppear(bool IsOn)
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = IsOn;

            await Easing.Create<Linear>(IsOn ? 1 : 0, 0.25f).ToColorA(canvasGroup);
        }
        
        private void OnMenuChangeState(OnMenuChangeState e)
        {
            if (e.state == MenuState.Menu && e.preState is MenuState.Title)
            {
                SetAppear(true);
            }
            else if ((e.preState == MenuState.Menu && e.state != MenuState.ToGame) || 
                     (e.preState == MenuState.ToGame ))
            {
                SetAppear(false);
            }
        }
    }
}