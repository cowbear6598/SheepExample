using Zenject;

namespace Holder
{
    public class HolderItemHandler
    {
        [Inject] private readonly HolderView        view;
        [Inject] private readonly HolderCardHandler cardHandler;

        public void Pull()
        {
            if (cardHandler.currentCount == 0) return;

            int totalPullCount = 3;

            var cards = cardHandler.cards;

            for (int i = 0; i < totalPullCount; i++)
            {
                if (cards[i] == null)
                    return;

                var pullPosition = view.GetPullPosition();
                pullPosition.x += i * 1.1f;

                cards[i].DoPullTo(pullPosition);
                cards[i] = null;

                cardHandler.CurrentCountAdd(-1);
            }

            for (int i = 3; i < cardHandler.cards.Length; i++)
            {
                if (cards[i] == null) return;

                cardHandler.OffsetCard(i - 3, i);
            }
        }

        public void Rollback()
        {
            if (cardHandler.currentCount == 0) return;

            var cards          = cardHandler.cards;
            var lastCard       = cardHandler.lastCard;
            var lastPlaceIndex = cardHandler.lastPlaceIndex;
            var totalCount     = cardHandler.GetTotalCount();

            if (lastCard == null)
            {
                var lastCardIndex = cardHandler.currentCount - 1; 
                
                cards[lastCardIndex].DoRollback();
                cards[lastCardIndex] = null;
                
                cardHandler.CurrentCountAdd(-1);
            }
            else
            {
                lastCard.DoRollback();
                cards[lastPlaceIndex] = null;
                
                cardHandler.CurrentCountAdd(-1);

                for (int i = lastPlaceIndex; i < totalCount; i++)
                {
                    if (i + 1 >= totalCount || cards[i + 1] == null)
                        return;

                    cardHandler.OffsetCard(i, i + 1);
                }
            }
        }
    }
}