using System;
using System.Collections.Generic;
using SoapUtils.NotifySystem;
using SoapUtils.SoundSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace UI
{
    public class UI_Rank : MonoBehaviour
    {
        [Inject] private readonly SignalBus          signalBus;
        [Inject] private readonly ISoundService      soundService;
        [Inject] private readonly INotifyService     notifyService;

        [SerializeField] private AssetReferenceT<AudioClip> clip_Click;

        [SerializeField] private UI_Rank_Scroller scroller;

        private int locationIndex = -1;

        private void Start()
        {
            locationIndex = 10;

            List<RankData> ranks = new List<RankData>();

            for (int i = 0; i < 50; i++)
            {
                ranks.Add(new RankData {
                    uid = i,
                    name = $"test_{i}",
                    elapsed_time = i,
                    score = 50 - i
                });
            }

            scroller.UpdateData(ranks);
            scroller.JumpTo(0);
        }
        
        public void Button_Location()
        {
            soundService.DoPlaySound(clip_Click);

            if (locationIndex == -1)
            {
                notifyService.DoNotify("您目前在排名外", () => { });
                return;
            }

            scroller.ScrollTo(locationIndex);
        }
    }

    [Serializable]
    public struct RankData
    {
        public int    uid;
        public string name;
        public int    elapsed_time;
        public int    score;
    }
}