using System;
using Fusion;

namespace Game
{
    public sealed class GameCycle : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(InvokeStateChanged))]
        public GameState State { get; private set; }
        
        public event Action OnGameStateChanged;
        public event Action OnGameStarted;
        public event Action OnGameFinished;

        public void StartGame()
        {
            if(!HasStateAuthority)
                return;
            
            if(State != GameState.Idle)
                return;

            State = GameState.Run;
        }

        public void LoseGame()
        {
            if(!HasStateAuthority)
                return;
            
            if(State != GameState.Run)
                return;
            
            State = GameState.Lose;
        }

        public void WinGame()
        {
            if(!HasStateAuthority)
                return;
            
            if(State != GameState.Run)
                return;
            
            State = GameState.Win;
        }

        private void InvokeStateChanged()
        {
            OnGameStateChanged?.Invoke();

            switch (State)
            {
                case GameState.Idle:
                    break;
                case GameState.Run:
                    OnGameStarted?.Invoke();
                    break;
                case GameState.Lose:
                case GameState.Win:
                    OnGameFinished?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}