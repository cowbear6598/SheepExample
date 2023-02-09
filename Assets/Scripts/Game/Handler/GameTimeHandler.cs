using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameTimeHandler : IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus signalBus;

        private ReactiveProperty<int> elapsedTime = new(0);

        private const int maxElapsedTime = 60 * 60;

        private CompositeDisposable countdownTimer;

        private long pauseTime;

        public void Initialize()
        {
            signalBus.Subscribe<OnGameChangeState>(OnGameChangeState);
        }

        public int GetElapsedTime() => elapsedTime.Value;

        public void Subscribe(Action<int> action)
        {
            elapsedTime.Subscribe(action);
        }

        private void OnGameChangeState(OnGameChangeState e)
        {
            switch (e.state)
            {
                case GameState.InGame:
                    if (countdownTimer == null)
                    {
                        countdownTimer = new CompositeDisposable();
                        Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                                  .Subscribe(l => elapsedTime.Value++)
                                  .AddTo(countdownTimer);
                    }

                    break;
                case GameState.GameOver:
                    countdownTimer?.Dispose();
                    break;
            }
        }

        public void Pause(bool IsPause)
        {
            if (IsPause)
            {
                pauseTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            else
            {
                int pauseElapsedTime = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() - pauseTime);

                elapsedTime.Value = Mathf.Clamp(elapsedTime.Value + pauseElapsedTime, 0, maxElapsedTime);
            }
        }

        public void Dispose()
        {
            signalBus.Unsubscribe<OnGameChangeState>(OnGameChangeState);

            elapsedTime?.Dispose();
            countdownTimer?.Dispose();
        }
    }
}