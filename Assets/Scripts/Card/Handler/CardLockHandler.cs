using System;
using Zenject;

namespace Card
{
    public class CardLockHandler : IInitializable
    {
        [Inject] private readonly CardStateHandler stateHandler;
        [Inject] private readonly CardRegistry     registry;

        private int        unlockCount;
        private CardView[] lockedCardViews;

        public CardView[] GetLockedCards() => lockedCardViews;

        public CardLockHandler(int unlockCount, CardView[] lockedCardViews)
        {
            this.unlockCount     = unlockCount;
            this.lockedCardViews = lockedCardViews;
        }

        public void Initialize()
        {
            if (unlockCount > 0)
                stateHandler.ChangeCardState(CardState.Lock);
        }

        public void Unlock()
        {
            unlockCount--;

            if (unlockCount == 0)
                stateHandler.ChangeCardState(CardState.None);
        }

        public void Lock()
        {
            unlockCount++;

            if (stateHandler.state != CardState.Lock)
                stateHandler.ChangeCardState(CardState.Lock);
        }

        public void LockOtherCards()
        {
            var lockedCards = GetLockedCards();

            for (int i = 0; i < lockedCards.Length; i++)
            {
                var id    = lockedCards[i].id;
                var level = lockedCards[i].level;

                ICardService cardService = registry.Find(level, id);

                cardService?.GetLockHandler().Lock();
            }
        }

        public void UnlockOtherCards()
        {
            var lockedCards = GetLockedCards();

            for (int i = 0; i < lockedCards.Length; i++)
            {
                var id    = lockedCards[i].id;
                var level = lockedCards[i].level;
                
                ICardService cardService = registry.Find(level, id);

                cardService?.GetLockHandler().Unlock();
            }
        }

        public void Clear()
        {
            unlockCount     = 0;
            lockedCardViews = Array.Empty<CardView>();
        }
    }
}