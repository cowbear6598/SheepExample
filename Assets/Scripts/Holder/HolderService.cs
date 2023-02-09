using Card;
using Holder;
using Zenject;

namespace Holder
{
    public interface IHolderService
    {
        bool DoAddCard(ICardService cardService);

        void DoClearCheck();

        void DoPull();
        void DoRollback();
    }

    public class HolderService : IHolderService
    {
        [Inject] private readonly HolderCardHandler  cardHandler;
        [Inject] private readonly HolderClearHandler clearHandler;
        [Inject] private readonly HolderItemHandler  itemHandler;

        public bool DoAddCard(ICardService cardService) => cardHandler.AddCard(cardService);

        public void DoClearCheck() => clearHandler.ClearCheck();

        public void DoPull() => itemHandler.Pull();
        public void DoRollback() => itemHandler.Rollback();
    }
}