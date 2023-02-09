using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameView : MonoBehaviour
    {
        [Inject] private readonly ISoundService   soundService;
        [Inject] private readonly GameTimeHandler timeHandler;

        [SerializeField] private Transform   backgroundImgTrans;

        [SerializeField] private AssetReferenceT<AudioClip>[] clip_BGM;
        [SerializeField] private AssetReferenceT<AudioClip>[] clip_Over;

        [SerializeField] private ParticleSystem winParticle;

        private void Start()
        {
            Application.targetFrameRate = 60;

            soundService.DoPlayBGM(clip_BGM[Random.Range(0, clip_BGM.Length)]);

            ResizeBackground();
        }

        private void ResizeBackground()
        {
            float height = 2436f;
            float width  = 1125f;

            float multiply = height / Screen.height;

            float currentWidth = Screen.width * multiply;

            if (currentWidth > width)
            {
                var resizeScale = Vector3.one * (currentWidth / width);
                resizeScale.z                 = 1;
                backgroundImgTrans.localScale = resizeScale;
            }
        }
        
        public void PlayLevelPassSound() => soundService.DoPlaySound(clip_Over[1]);

        public void PlayGameOverSound(bool IsVictory) => soundService.DoPlaySound(clip_Over[IsVictory ? 1 : 0]);

        public void PlayWinParticle() => winParticle.Play();

        private void OnApplicationPause(bool IsPause) => timeHandler.Pause(IsPause);
    }
}