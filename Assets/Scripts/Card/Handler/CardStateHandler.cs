using UnityEngine;
using Zenject;

namespace Card
{
    public enum CardState
    {
        None         = 0,
        Lock         = 1,
        MoveToHolder = 2,
        InHolder     = 3,
        Clearing     = 4,
        Clear        = 5,
        Pulling      = 6,
        Rollback     = 7,
        Shake        = 8,
        Random       = 9
    }

    public class CardStateHandler
    {
        [Inject] private readonly CardView cardView;

        public CardState state { get; private set; } = CardState.None;

        public void ChangeCardState(CardState state)
        {
            this.state = state;

            switch (state)
            {
                case CardState.None:
                    cardView.SetColor(Color.white);
                    break;
                case CardState.Lock:
                    cardView.SetColor(new Color(0.5f, 0.5f, 0.5f, 1));
                    break;
                case CardState.Clear:
                    cardView.SetActive(false);
                    break;
            }
        }
    }
}