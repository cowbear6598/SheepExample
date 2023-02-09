using System;
using Game;
using Holder;
using SoapUtils.NotifySystem;
using SoapUtils.SceneSystem;
using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace UI.Game
{
    public enum ItemType
    {
        Pull     = 0,
        Rollback = 1,
        Shuffle  = 2
    }

    public class UI_Item : MonoBehaviour
    {
        [Inject] private readonly ISoundService    soundService;
        [Inject] private readonly ISceneService    sceneService;
        [Inject] private readonly INotifyService   notifyService;
        [Inject] private readonly IGameService     gameService;
        [Inject] private readonly IHolderService   holderService;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;
        [SerializeField] private AssetReferenceT<AudioClip> clip_Random;

        [Serializable]
        private class ItemBtn
        {
            [SerializeField] private Button btn;
            [SerializeField] private Image  btnImg;
            [SerializeField] private Image  iconImg;

            public void Use(Sprite sprite)
            {
                btn.interactable = false;
                
                btnImg.sprite = sprite;
                
                iconImg.color = Color.white;
            }
        }

        [SerializeField] private ItemBtn pullItem;
        [SerializeField] private ItemBtn rollbackItem;
        [SerializeField] private ItemBtn shuffleItem;

        [SerializeField] private Sprite[] btnSprites;

        public void Button_Use(int index)
        {
            soundService.DoPlaySound(clip_Click);
            
            if (gameService.GetGameState() != GameState.InGame) return;

            gameService.DoChangeGameState(GameState.Option);

            string message;

            switch ((ItemType)index)
            {
                case ItemType.Pull:
                    message = $"將前三個恐龍放回場上。";
                    break;
                case ItemType.Rollback:
                    message = $"放回上一個恐龍，若已被消除則將最後一個恐龍放回場上。";
                    break;
                case ItemType.Shuffle:
                    message = $"將遺留在場上的恐龍重組。";
                    break;
                default:
                    return;
            }

            notifyService.DoNotify(message, () => Use(index), () => gameService.DoChangeGameState(GameState.InGame));
        }

        private void Use(int index)
        {
            gameService.DoChangeGameState(GameState.InGame);

            switch ((ItemType)index)
            {
                case ItemType.Pull:
                    holderService.DoPull();
                    pullItem.Use(btnSprites[0]);
                    break;
                case ItemType.Rollback:
                    holderService.DoRollback();
                    rollbackItem.Use(btnSprites[0]);
                    break;
                case ItemType.Shuffle:
                    gameService.DoShuffleLevel();
                    shuffleItem.Use(btnSprites[0]);
                    soundService.DoPlaySound(clip_Random);
                    break;
            }
        }
    }
}