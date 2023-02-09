using System;
using Game;
using UnityEngine;

namespace Card
{
    public enum CardType
    {
        Dinosaur1,
        Dinosaur2,
        Dinosaur3,
        Dinosaur4,
        Dinosaur5,
        Dinosaur6,
        Dinosaur7,
        Dinosaur8,
        Dinosaur9,
        Dinosaur10,
        Dinosaur11,
        Dinosaur12,
        Dinosaur13,
        Dinosaur14,
        Dinosaur15,
        Dinosaur16,
        None = 99
    }

    public class CardTypeHandler
    {
        private CardType cardType = CardType.Dinosaur1;

        private readonly Settings settings;
        private readonly CardView cardView;

        public CardTypeHandler(Settings settings, CardView cardView)
        {
            this.settings = settings;
            this.cardView = cardView;
        }

        public void SetCardType(CardType cardType)
        {
            this.cardType = cardType;

            int spriteIndex = (int)this.cardType;

            cardView.SetSprite(settings.cardSprites[spriteIndex]);
        }
        public CardType GetCardType() => cardType;

        [Serializable]
        public class Settings
        {
            public Sprite[] cardSprites;
        }
    }
}