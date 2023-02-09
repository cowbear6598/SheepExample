using System;
using AnimeTask;
using Game;
using SoapUtils.NotifySystem;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Game
{
    public class UI_Game : MonoBehaviour
    {
        [Inject] private readonly SignalBus      signalBus;
        [Inject] private readonly INotifyService notifyService;
        [Inject] private readonly ISoundService  soundService;
        [Inject] private readonly IGameService   gameService;

        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeText;

        [SerializeField] private RectTransform topTrans;
        [SerializeField] private RectTransform itemTrans;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        private void OnEnable()
        {
            signalBus.Subscribe<OnGameChangeState>(OnGameChangeState);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnGameChangeState>(OnGameChangeState);
        }

        private void Start()
        {
            gameService.DoSubscribeScore(UpdateScore);
            gameService.DoSubscribeTime(UpdateTime);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private void UpdateTime(int time)
        {
            timeText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
        }

        public void Button_GiveUp()
        {
            soundService.DoPlaySound(clip_Click);
            
            if (gameService.GetGameState() != GameState.InGame) return;

            gameService.DoChangeGameState(GameState.Option);
            notifyService.DoNotify("確定要放棄遊戲嗎？", () => gameService.DoGameOver(false), () => gameService.DoChangeGameState(GameState.InGame));
        }

        private void OnGameChangeState(OnGameChangeState e)
        {
            if (e.state == GameState.GameOver)
            {
                var duration = 1f;
                
                Easing.Create<OutBack>(200, duration).ToAnchoredPositionY(topTrans);
                Easing.Create<OutBack>(-180, duration).ToAnchoredPositionY(itemTrans);
            }
        }
    }
}