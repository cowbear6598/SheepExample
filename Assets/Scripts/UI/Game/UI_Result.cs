using AnimeTask;
using Game;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI.Game
{
    public class UI_Result : MonoBehaviour
    {
        [Inject] private readonly SignalBus     signalBus;
        [Inject] private readonly ISceneService sceneService;
        [Inject] private readonly ISoundService soundService;

        private CanvasGroup canvasGroup;

        [SerializeField] private RectTransform   bgTrans;
        [SerializeField] private GameObject[]    titleObjs;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            signalBus.Subscribe<OnGameOver>(OnGameOver);
        }

        private void OnDisable()
        {
            signalBus.Unsubscribe<OnGameOver>(OnGameOver);
        }
        
        public void Button_Back()
        {
            soundService.DoPlaySound(clip_Click);
            
            sceneService.DoLoadScene(0, false);
        }
        
        private void OnGameOver(OnGameOver e)
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = true;

            titleObjs[e.IsVictory ? 1 : 0].SetActive(true);
            timeText.text  = $"{(e.time / 60 / 60):00}:{(e.time / 60 % 60):00}:{(e.time % 60):00}";
            scoreText.text = e.score.ToString();
            
            Easing.Create<Linear>(1, 0.1f).ToColorA(canvasGroup);
            Easing.Create<Linear>(Vector3.one, 0.1f).ToLocalScale(bgTrans);
        }
    }
}