using System;
using UniRx;
using Zenject;

namespace Game
{
    public class GameScoreHandler
    {
        [Inject] private readonly Settings  settings;

        private ReactiveProperty<int> score = new(0);

        public int GetScore() => score.Value;

        public void Subscribe(Action<int> action)
        {
            score.Subscribe(action);
        }

        public void AddScore()
        {
            score.Value += settings.score;
        }

        [Serializable]
        public class Settings
        {
            public int score;
        }
    }
}