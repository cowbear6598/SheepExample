using SoapUtils.PrefsSystem;
using UnityEngine;
using Zenject;

namespace UI
{
    public class SettingHandler
    {
        [Inject] private readonly IPrefsService    prefsService;
        [Inject] private readonly BasicComponent   basicComponent;

        public float SetBGMVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;

            basicComponent.audioMixer.SetFloat("bgmVolume", dB);
            
            prefsService.SetFloat("BGM_Volume", volume);

            return dB;
        }

        public float SetEffectVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;

            basicComponent.audioMixer.SetFloat("effectVolume", dB);

            prefsService.SetFloat("Effect_Volume", volume);

            return dB;
        }
    }
}