using System;
using AnimeTask;
using Menu;
using SoapUtils.SoundSystem;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI
{
    public class UI_Title : MonoBehaviour
    {
        [Inject] private readonly ISoundService soundService;
        [Inject] private readonly IMenuService  menuService;
        [Inject] private readonly SignalBus     signalBus;

        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private TextMeshProUGUI tapText;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        private CompositeDisposable shineRepeater = new();
        
        private void OnEnable()
        {
            signalBus.Subscribe<OnMenuChangeState>(OnMenuChangeState);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnMenuChangeState>(OnMenuChangeState);
        }

        private async void TapTextShine(long l)
        {
            await Easing.Create<InQuint>(Color.clear, 1f).ToColor(tapText);
            await Easing.Create<OutQuint>(Color.white, 1f).ToColor(tapText);
        }

        public void Button_Start()
        {
            if (menuService.GetState() != MenuState.Title) return;

            soundService.DoPlaySound(clip_Click);
            
            menuService.DoChangeState(MenuState.Menu);
        }

        private void SetAppear(bool IsOn)
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = IsOn;
            Easing.Create<Linear>(IsOn ? 1 : 0, 0.25f).ToColorA(canvasGroup);
        }

        private void OnMenuChangeState(OnMenuChangeState e)
        {
            if (e.state == MenuState.Title)
            {
                shineRepeater = new CompositeDisposable();

                Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
                          .Subscribe(TapTextShine)
                          .AddTo(shineRepeater);

                SetAppear(true);
            }
            else if (e.preState is MenuState.Title)
            {
                shineRepeater.Dispose();

                SetAppear(false);
            }
        }
    }
}