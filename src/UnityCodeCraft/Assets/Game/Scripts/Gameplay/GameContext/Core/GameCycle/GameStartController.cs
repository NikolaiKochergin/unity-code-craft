using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameStartController : NetworkBehaviour
    {
        [SerializeField] private int _startPlayerCount = 2;
        
        private PlayerManager _playerManager;
        private GameCycle _gameCycle;

        [Inject]
        public void Construct(PlayerManager playerManager, GameCycle gameCycle)
        {
            _gameCycle = gameCycle;
            _playerManager = playerManager;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameCycle.State == GameState.Idle && _playerManager.PlayerCount >= _startPlayerCount)
            {
                _gameCycle.StartGame();
                Debug.Log("<color=green>START GAME</color>");
            }
        }
    }
}