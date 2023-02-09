using System;
using System.Collections.Generic;
using System.Linq;
using AnimeTask;
using Card;
using Cysharp.Threading.Tasks;
using SoapUtils.SceneSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameLevelHandler : IInitializable, IDisposable
    {
        [Inject] private readonly ISceneService sceneService;

        [Inject] private readonly DiContainer      container;
        [Inject] private readonly CardRegistry     cardRegistry;
        [Inject] private readonly GameOverHandler  overHandler;
        [Inject] private readonly GameStateHandler stateHandler;
        [Inject] private readonly GameView         view;
        [Inject] private          Settings         settings;

        private int currentLevel = 0;

        private GameObject            levelObj;
        private LevelView             levelView;
        private Transform             levelGroupTrans;
        private LevelView.LevelData[] levelData;

        public async void Initialize()
        {
            currentLevel = Random.Range(0, settings.levelAssets.Length);
                            
            levelObj = await Addressables.LoadAssetAsync<GameObject>(settings.levelAssets[currentLevel]).Task;

            levelGroupTrans = container.InstantiatePrefab(levelObj).transform;
            levelView       = levelGroupTrans.GetComponent<LevelView>();
            levelData       = levelView.levelData;

            for (int level = 0; level < levelView.totalLevel; level++)
            {
                Shuffle(level);
            }

            await UniTask.Delay(1000);

            sceneService.DoFadeOut();
            BuildLevel(currentLevel);
        }

        public void ShuffleLevel() => Shuffle(currentLevel, true);

        private void Shuffle(int level, bool IsItem = false)
        {
            System.Random random = new System.Random();

            var allCards            = cardRegistry.AllCards(level).OrderBy(_ => random.Next()).ToList();
            var matchCardDictionary = GetMatchingCard(allCards);

            int      matchCardIndex = 0;
            int      spawnCount     = 0;
            CardType spawnCardType  = (CardType)Random.Range(0, 16);

            for (int j = 0; j < allCards.Count; j++)
            {
                ICardService cardService = allCards[j].Value;

                if (cardService.GetCardState() is not (CardState.None or CardState.Lock))
                    continue;

                if (IsItem)
                    cardService.DoRandomMove();

                if (matchCardIndex < matchCardDictionary.Count)
                {
                    var matchCardPair = matchCardDictionary.ElementAt(matchCardIndex);

                    spawnCardType = matchCardPair.Key;
                    matchCardDictionary[spawnCardType]++;

                    if (matchCardDictionary[spawnCardType] == 3)
                        matchCardIndex++;
                }
                else
                {
                    spawnCount++;
                }

                cardService.SetCardType(spawnCardType);

                if (spawnCount == 3)
                {
                    spawnCardType = (CardType)Random.Range(0, 16);
                    spawnCount    = 0;
                }
            }
        }

        private Dictionary<CardType, int> GetMatchingCard(IReadOnlyList<KeyValuePair<int, ICardService>> allCards)
        {
            var cardDictionary = new Dictionary<CardType, int>();

            // 先抓取 Match 堆裡的 Type
            for (int i = 0; i < allCards.Count; i++)
            {
                ICardService cardService = allCards[i].Value;

                if (cardService.GetCardState() is (CardState.MoveToHolder or CardState.InHolder))
                {
                    CardType cardType = cardService.GetCardType();

                    if (cardDictionary.ContainsKey(cardType))
                    {
                        cardDictionary[cardType]++;

                        // 數量三代表正準備 Match 移除此陣列
                        if (cardDictionary[cardType] == 3)
                            cardDictionary.Remove(cardType);
                    }
                    else
                    {
                        cardDictionary.Add(cardType, 1);
                    }
                }
            }

            return cardDictionary;
        }

        public void LevelClear()
        {
            if (currentLevel == levelView.totalLevel - 1)
            {
                overHandler.GameOver(true);
            }
            else
            {
                currentLevel++;

                BuildLevel(currentLevel);

                view.PlayLevelPassSound();
            }
        }

        private async void BuildLevel(int level)
        {
            stateHandler.ChangeState(GameState.BuildLevel);

            var duration = 1f;
            var interval = 0.35f;

            levelGroupTrans.localPosition = new Vector3(-15 * level, 0, 0);

            for (int i = 0; i < levelData.Length; i++)
            {
                levelData[i].groupObj.SetActive(false);
            }

            for (int i = 0; i < levelData.Length; i++)
            {
                if (level == i)
                {
                    levelData[i].groupObj.SetActive(true);

                    for (int j = 0; j < levelData[i].layerTrans.Length; j++)
                    {
                        var layerTrans     = levelData[i].layerTrans[j];
                        var targetPosition = new Vector3(0, 0, layerTrans.position.z);

                        Easing.Create<OutQuint>(targetPosition, duration).ToLocalPosition(layerTrans);

                        if (j != levelData[i].layerTrans.Length - 1)
                            await UniTask.Delay(TimeSpan.FromSeconds(interval));
                    }
                }
            }

            stateHandler.ChangeState(GameState.InGame);
        }

        public void Dispose()
        {
            Addressables.Release(levelObj);
        }

        [Serializable]
        public class Settings
        {
            public AssetReference[] levelAssets;
        }
    }
}