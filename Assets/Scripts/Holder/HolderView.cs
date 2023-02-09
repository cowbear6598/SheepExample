using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Holder
{
    public class HolderView : MonoBehaviour
    {
        [Inject] private readonly ISoundService soundService;
        
        [SerializeField] private Transform firstTrans;
        [SerializeField] private Transform pullTrans;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Add;
        [SerializeField] private AssetReferenceT<AudioClip> clip_AddFail;
        [SerializeField] private AssetReferenceT<AudioClip> clip_Clear;

        public Vector2 GetFirstPosition() => firstTrans.position;
        public Vector2 GetPullPosition() => pullTrans.position;

        public void PlayAddSound() => soundService.DoPlaySound(clip_Add);
        public void PlayAddFailSound() => soundService.DoPlaySound(clip_AddFail);
        public void PlayClearSound() => soundService.DoPlaySound(clip_Clear);
    }
}