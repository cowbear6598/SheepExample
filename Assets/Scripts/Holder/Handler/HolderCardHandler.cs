using System;
using Card;
using UnityEngine;
using Zenject;

namespace Holder
{
    public class HolderCardHandler : IInitializable
    {
        [Inject] private readonly HolderView view;
        [Inject] private readonly Settings   settings;

        public int            currentCount   { get; private set; }
        public ICardService   lastCard       { get; private set; }
        public int            lastPlaceIndex { get; private set; }
        public ICardService[] cards          { get; private set; }

        public int GetTotalCount() => settings.totalCount;

        public void Initialize()
        {
            cards = new ICardService[settings.totalCount];

            currentCount = 0;
        }

        public bool AddCard(ICardService cardService)
        {
            if (currentCount == settings.totalCount)
            {
                Debug.LogWarning("card full");
                view.PlayAddFailSound();
                return false;
            }

            cardService.DoUnlockOtherCards();
            view.PlayAddSound();

            int  placeIndex = -1;
            bool IsMatched  = false;

            for (int i = 0; i < cards.Length; i++)
            {
                // 放置在最後面
                if (cards[i] == null)
                {
                    placeIndex = i;
                    cards[i]   = cardService;

                    break;
                }

                // 放置在相同卡牌後面
                if (IsMatched)
                {
                    if (cards[i].GetCardType() != cardService.GetCardType())
                    {
                        placeIndex = i;
                        OffsetCard(cardService, placeIndex);
                        break;
                    }
                }
                else
                {
                    if (cards[i].GetCardType() == cardService.GetCardType())
                        IsMatched = true;
                }
            }

            if (placeIndex == -1)
            {
                Debug.LogError("placeIndex shouldn't be -1");
                return false;
            }

            CurrentCountAdd(1);

            lastPlaceIndex = placeIndex;
            lastCard       = cardService;

            Vector2 placePosition = view.GetFirstPosition() + new Vector2(placeIndex * settings.padding, 0);
            cardService.DoMoveToHolder(placePosition);
            return true;
        }

        private void OffsetCard(ICardService cardService, int placeIndex)
        {
            var _cardServices = new ICardService[cards.Length];

            for (int i = 0; i < cards.Length; i++)
                _cardServices[i] = cards[i];

            cards[placeIndex] = cardService;

            for (int i = placeIndex; i < cards.Length; i++)
            {
                if (_cardServices[i] == null)
                    break;

                cards[i + 1] = _cardServices[i];
                cards[i + 1].DoHolderOffset(settings.padding);
            }
        }

        public void OffsetCard(int oldCardIndex, int newCardIndex)
        {
            int offsetAmount = newCardIndex - oldCardIndex;

            bool IsDecreaseCard = newCardIndex > oldCardIndex;

            cards[oldCardIndex] = cards[newCardIndex];
            cards[oldCardIndex].DoHolderOffset((offsetAmount * (IsDecreaseCard ? -1 : 1)) * settings.padding);

            cards[newCardIndex] = null;
        }

        public void CurrentCountAdd(int amount) => currentCount += amount;

        public void RemoveCard(int index)
        {
            currentCount--;

            if (cards[index] == lastCard)
                lastCard = null;
            
            cards[index].DoClear();
            cards[index] = null;
        }

        [Serializable]
        public class Settings
        {
            public int   totalCount;
            public float padding;
        }
    }
}