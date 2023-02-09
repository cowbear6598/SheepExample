using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
    public enum GameState
    {
        Initialize = 0,
        BuildLevel = 1,
        InGame     = 2,
        Option     = 3,
        GameOver   = 4
    }

    public class GameStateHandler
    {
        [Inject] private readonly SignalBus signalBus;

        public GameState state { get; private set; } = GameState.Initialize;

        public void ChangeState(GameState state)
        {
            signalBus.Fire(new OnGameChangeState(this.state, state));

            this.state = state;
            
            Debug.Log($"game state: {this.state}");
        }
    }
}