using System;
using Zenject;

namespace Game
{
    public interface IGameService
    {
        void DoShuffleLevel();
        void DoLevelClear();

        void DoSubscribeScore(Action<int> action);
        void DoAddScore();
        
        void DoSubscribeTime(Action<int> action);

        void DoGameOver(bool IsVictory);

        void DoChangeGameState(GameState state);
        GameState GetGameState();
    }

    public class GameService : IGameService
    {
        [Inject] private readonly GameLevelHandler levelHandler;
        [Inject] private readonly GameScoreHandler scoreHandler;
        [Inject] private readonly GameTimeHandler  timeHandler;
        [Inject] private readonly GameOverHandler  overHandler;
        [Inject] private readonly GameStateHandler stateHandler;
        
        public void DoShuffleLevel() => levelHandler.ShuffleLevel();
        public void DoLevelClear() => levelHandler.LevelClear();

        public void DoSubscribeScore(Action<int> action) => scoreHandler.Subscribe(action);
        public void DoAddScore() => scoreHandler.AddScore();
        
        public void DoSubscribeTime(Action<int> action) => timeHandler.Subscribe(action);

        public void DoGameOver(bool IsVictory) => overHandler.GameOver(IsVictory);

        public void DoChangeGameState(GameState state) => stateHandler.ChangeState(state);
        public GameState GetGameState() => stateHandler.state;
    }
}