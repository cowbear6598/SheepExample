using Zenject;

namespace Game
{
    public class GameOverHandler
    {
        [Inject] private readonly SignalBus        signalBus;
        [Inject] private readonly GameView         view;
        [Inject] private readonly GameTimeHandler  timeHandler;
        [Inject] private readonly GameScoreHandler scoreHandler;
        [Inject] private readonly GameStateHandler stateHandler;

        public void GameOver(bool IsVictory)
        {
            stateHandler.ChangeState(GameState.GameOver);
            view.PlayGameOverSound(IsVictory);

            var score       = scoreHandler.GetScore();
            var elapsedTime = timeHandler.GetElapsedTime();

            signalBus.Fire(new OnGameOver(IsVictory, elapsedTime, score));
        }
    }
}