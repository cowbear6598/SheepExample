using AnimeTask;
using Game;
using Holder;
using UnityEngine;
using Zenject;

namespace Card
{
    public class CardView : MonoBehaviour, IInitializable
    {
        public int id { get; private set; }
        public int level;

        [Inject] private readonly CardRegistry    cardRegistry;
        [Inject] private readonly CardMoveHandler moveHandler;
        [Inject] private readonly ICardService    cardService;
        [Inject] private readonly IHolderService  holderService;
        [Inject] private readonly IGameService    gameService;

        [SerializeField] private SpriteRenderer[] render;

        public void Initialize()
        {
            id = gameObject.GetInstanceID();

            cardRegistry.AddCard(level, id, cardService);
        }

        public void OnRaycastHit()
        {
            if (gameService.GetGameState() != GameState.InGame)
            {
                Debug.LogWarning($"cannot raycast, game: {gameService.GetGameState()}");
                return;
            }
            
            if (cardService.GetCardState() != CardState.None)
            {
                Debug.LogWarning($"cannot raycast, state: {cardService.GetCardState()}");
                return;
            }

            if (!holderService.DoAddCard(cardService))
            {
                moveHandler.Shake();
            }
        }

        public void SetPosition(Vector2 targetPosition) => transform.localPosition = targetPosition;
        public Vector2 GetPosition() => transform.localPosition;
        public Vector2 GetWorldPosition() => transform.position;

        public void SetRotation(Quaternion rot) => transform.rotation = rot; 
        public Quaternion GetRotation() => transform.rotation;

        public void SetParent(Transform parent) => transform.SetParent(parent);

        public void SetSprite(Sprite sprite) => render[1].sprite = sprite;

        public void SetRenderSort(int layerID, int order)
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].sortingLayerID = layerID;
                render[i].sortingOrder   = order;
            }
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < render.Length; i++)
            {
                render[i].color = color;
            }
        }

        public void SetActive(bool IsOn) => gameObject.SetActive(IsOn);
    }
}