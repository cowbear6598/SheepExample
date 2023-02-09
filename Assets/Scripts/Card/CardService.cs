using Card;
using UnityEngine;
using Zenject;

namespace Card
{
    public interface ICardService
    {
        void SetCardType(CardType cardType);
        CardType GetCardType();

        CardState GetCardState();

        CardLockHandler GetLockHandler();
        void DoUnlockOtherCards();

        void DoHolderOffset(float padding);
        void DoMoveToHolder(Vector2 position);
        void DoClear();
        void DoPullTo(Vector2 position);
        void DoRollback();
        void DoRandomMove();
    }

    public class CardService : ICardService
    {
        [Inject] private readonly CardTypeHandler  typeHandler;
        [Inject] private readonly CardStateHandler stateHandler;
        [Inject] private readonly CardLockHandler  lockHandler;
        [Inject] private readonly CardMoveHandler  moveHandler;

        public void SetCardType(CardType cardType) => typeHandler.SetCardType(cardType);
        public CardType GetCardType() => typeHandler.GetCardType();

        public CardState GetCardState() => stateHandler.state;
        public CardLockHandler GetLockHandler() => lockHandler;

        public void DoUnlockOtherCards() => lockHandler.UnlockOtherCards();

        public void DoHolderOffset(float padding) => moveHandler.HolderOffset(padding);
        public void DoMoveToHolder(Vector2 position) => moveHandler.MoveToHolder(position);
        public void DoRandomMove() => moveHandler.RandomMove();
        public void DoClear() => moveHandler.Clear();
        public void DoPullTo(Vector2 position) => moveHandler.PullTo(position);
        public void DoRollback() => moveHandler.Rollback();
    }
}