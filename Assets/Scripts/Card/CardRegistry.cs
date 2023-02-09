using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using Zenject;

namespace Card
{
    public class CardRegistry : IInitializable
    {
        [Inject] private readonly IGameService gameService;

        private Dictionary<int, ICardService>[] cardDictionaryArray;

        public IEnumerable<KeyValuePair<int, ICardService>> AllCards(int level) => cardDictionaryArray[level].ToList();

        public void Initialize()
        {
            cardDictionaryArray = new Dictionary<int, ICardService>[5];

            for (int i = 0; i < cardDictionaryArray.Length; i++)
            {
                cardDictionaryArray[i] = new Dictionary<int, ICardService>();
            }
        }

        public ICardService Find(int level, int id)
        {
            if (cardDictionaryArray[level].ContainsKey(id))
                return cardDictionaryArray[level][id];

            Debug.Log("cannot find card");

            return null;
        }

        public void AddCard(int level, int id, ICardService cardService)
        {
            if (cardDictionaryArray[level].ContainsKey(id))
            {
                Debug.Log("already has this card");
                return;
            }

            cardDictionaryArray[level].Add(id, cardService);
        }

        public void RemoveCard(int level, int id)
        {
            if (!cardDictionaryArray[level].ContainsKey(id))
            {
                Debug.Log("cannot find this card");
                return;
            }

            cardDictionaryArray[level].Remove(id);

            if (cardDictionaryArray[level].Count == 0)
            {
                Debug.Log("level clear");
                gameService.DoLevelClear();
            }
        }
    }
}