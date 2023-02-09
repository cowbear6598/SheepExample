using AnimeTask;
using SoapUtils.PrefsSystem;
using SoapUtils.SoundSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UI_Setting : MonoBehaviour
    {
        [Inject] private ISoundService  soundService;
        [Inject] private IPrefsService  prefsService;
        [Inject] private SettingHandler settingHandler;

        [SerializeField] private CanvasGroup   canvasGroup;
        [SerializeField] private RectTransform bgPos;

        [SerializeField] private Slider          bgmSlider;
        [SerializeField] private Image           bgmIconImg;
        [SerializeField] private TextMeshProUGUI bgmVolumeText;

        [SerializeField] private Slider          effectSlider;
        [SerializeField] private Image           effectIconImg;
        [SerializeField] private TextMeshProUGUI effectVolumeText;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Confirm;
        [SerializeField] private AssetReferenceT<AudioClip> clip_Cancel;

        private void Start()
        {
            bgmSlider.value = (prefsService.Exist("BGM_Volume")) ? prefsService.GetFloat("BGM_Volume") : 1;
            SetBGMVolume(bgmSlider.value);

            effectSlider.value = (prefsService.Exist("Effect_Volume")) ? prefsService.GetFloat("Effect_Volume") : 1;
            SetEffectVolume(effectSlider.value);
        }

        public void Slider_BGM(float volume)
        {
            SetBGMVolume(volume);
        }

        private void SetBGMVolume(float volume)
        {
            settingHandler.SetBGMVolume(volume);

            bgmVolumeText.text = ((int)((volume - bgmSlider.minValue) / (bgmSlider.maxValue - bgmSlider.minValue) * 100)).ToString();
        }

        public void Slider_Effect(float volume)
        {
            SetEffectVolume(volume);
        }

        private void SetEffectVolume(float volume)
        {
            settingHandler.SetEffectVolume(volume);

            effectVolumeText.text = ((int)((volume - effectSlider.minValue) / (effectSlider.maxValue - effectSlider.minValue) * 100)).ToString();
        }
        
        public void Button_Open()
        {
            soundService.DoPlaySound(clip_Confirm);

            SetAppear(true);
        }

        public void Button_Close()
        {
            soundService.DoPlaySound(clip_Cancel);

            SetAppear(false);
        }

        private void SetAppear(bool IsOn)
        {
            var tweenTime = 0.15f;

            canvasGroup.interactable = canvasGroup.blocksRaycasts = IsOn;

            Easing.Create<Linear>(IsOn ? 1 : 0, tweenTime).ToColorA(canvasGroup);
            Easing.Create<Linear>(IsOn ? Vector3.one : Vector3.zero, tweenTime).ToLocalScale(bgPos);
        }
    }
}