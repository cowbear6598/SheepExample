using System.Linq;
using Card;
using Game;
using Zenject;

namespace Holder
{
    public class HolderClearHandler
    {
        [Inject] private readonly IGameService      gameService;
        [Inject] private readonly HolderCardHandler cardHandler;
        [Inject] private readonly HolderView        view;

        public void ClearCheck()
        {
            var matchCount   = 0;
            var lastCardType = CardType.None;
            var cards        = cardHandler.cards;

            for (int i = 0; i < cards.Length; i++)
            {
                var currentCard = cards[i];

                if (currentCard == null) return;

                var CanMatch = currentCard.GetCardType()  == lastCardType &&
                               currentCard.GetCardState() == CardState.InHolder;

                if (CanMatch)
                {
                    matchCount++;

                    if (matchCount == 3)
                    {
                        view.PlayClearSound();

                        gameService.DoAddScore();

                        // 三消
                        for (int j = i; j > i - 3; j--)
                        {
                            cardHandler.RemoveCard(j);
                        }

                        // 調整位置
                        for (int j = i; j < cards.Length; j++)
                        {
                            var IsValid = j + 1 < cards.Length && cards[j + 1] != null;

                            if (IsValid)
                            {
                                cardHandler.OffsetCard(j - 2, j + 1);
                            }
                        }

                        return;
                    }
                }
                else
                {
                    lastCardType = currentCard.GetCardType();
                    matchCount   = 1;
                }
            }

            CheckGameOver();
        }

        private void CheckGameOver()
        {
            var cards = cardHandler.cards;

            bool IsGameOver = cards.All((card) => card                != null &&
                                                  card.GetCardState() == CardState.InHolder);

            if (!IsGameOver) return;

            gameService.DoGameOver(false);
        }
    }
}