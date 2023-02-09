using System;
using AnimeTask;
using Cysharp.Threading.Tasks;
using Holder;
using TimeSystem;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Card
{
    public class CardMoveHandler : IInitializable, ITickable
    {
        [Inject] private readonly ITimeService     timeService;
        [Inject] private readonly IHolderService   holderService;
        [Inject] private readonly CardView         view;
        [Inject] private readonly CardStateHandler stateHandler;
        [Inject] private readonly CardLockHandler  lockHandler;
        [Inject] private readonly CardRegistry     registry;

        private Vector2    originPosition;
        private Vector2    targetPosition;


        public void Initialize()
        {
            originPosition = targetPosition = view.GetPosition();
        }

        public void HolderOffset(float x)
        {
            targetPosition.x += x;

            stateHandler.ChangeCardState(CardState.MoveToHolder);
        }

        public void MoveToHolder(Vector2 position)
        {
            targetPosition = position;

            stateHandler.ChangeCardState(CardState.MoveToHolder);

            view.SetRenderSort(SortingLayer.NameToID("Card"), 1);
        }

        public async void Shake()
        {
            stateHandler.ChangeCardState(CardState.Shake);

            await Easing.Create<Linear>(originPosition + new Vector2(0.1f, 0), 0.05f).ToLocalPosition(view);
            await Easing.Create<Linear>(originPosition + new Vector2(-0.1f, 0), 0.05f).ToLocalPosition(view);
            await Easing.Create<Linear>(originPosition, 0.05f).ToLocalPosition(view);

            stateHandler.ChangeCardState(CardState.None);
        }

        public async void RandomMove()
        {
            CardState originState = stateHandler.state;

            stateHandler.ChangeCardState(CardState.Random);

            Vector2 direction = (view.GetWorldPosition() - Vector2.zero).normalized;

            await Easing.Create<Linear>(originPosition + direction, 0.1f).ToLocalPosition(view);
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            await Easing.Create<Linear>(originPosition, 0.1f).ToLocalPosition(view);

            stateHandler.ChangeCardState(originState);
        }

        public async void Clear()
        {
            stateHandler.ChangeCardState(CardState.Clearing);

            registry.RemoveCard(view.level, view.id);

            view.SetParent(null);
            view.SetRenderSort(SortingLayer.NameToID("Foreground"), 99);

            await UniTask.WhenAll(
                Moving.Gravity(new Vector2(Random.Range(-5, 5), Random.Range(5f, 10)), Vector2.down * 60f, 1.5f).ToLocalPosition(view),
                Easing.Create<Linear>(Vector3.zero, 1.5f).ToLocalScale(view)
            );

            stateHandler.ChangeCardState(CardState.Clear);
        }

        public void Tick()
        {
            switch (stateHandler.state)
            {
                case CardState.MoveToHolder:
                case CardState.Pulling:
                case CardState.Rollback:
                {
                    if (Vector2.Distance(view.GetPosition(), targetPosition) > Vector2.kEpsilon)
                    {
                        Vector2 movePosition = Vector2.MoveTowards(view.GetPosition(), targetPosition, 30 * timeService.GetDeltaTime());
                        view.SetPosition(movePosition);
                    }
                    else
                    {
                        if (stateHandler.state == CardState.MoveToHolder)
                        {
                            stateHandler.ChangeCardState(CardState.InHolder);
                            holderService.DoClearCheck();
                        }
                        else if (stateHandler.state is CardState.Pulling or CardState.Rollback)
                        {
                            stateHandler.ChangeCardState(CardState.None);
                        }
                    }
                }
                    break;
            }
        }

        public void PullTo(Vector2 targetPosition)
        {
            lockHandler.Clear();

            originPosition = this.targetPosition = targetPosition;

            stateHandler.ChangeCardState(CardState.Pulling);

            view.SetRenderSort(SortingLayer.NameToID("Card"), 0);
        }

        public void Rollback()
        {
            lockHandler.LockOtherCards();

            targetPosition = originPosition;
            stateHandler.ChangeCardState(CardState.Rollback);

            view.SetRenderSort(SortingLayer.NameToID("Card"), 0);
        }
    }
}