using Game;

namespace Game
{
    public struct OnGameChangeState
    {
        public GameState preState;
        public GameState state;

        public OnGameChangeState(GameState preState, GameState state)
        {
            this.preState = preState;
            this.state    = state;
        }
    }

    public struct OnGameOver
    {
        public bool IsVictory;
        public int  time;
        public int  score;

        public OnGameOver(bool IsVictory, int time, int score)
        {
            this.IsVictory = IsVictory;
            this.time      = time;
            this.score     = score;
        }
    }
}