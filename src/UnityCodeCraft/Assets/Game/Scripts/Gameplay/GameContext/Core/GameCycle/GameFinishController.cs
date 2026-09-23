using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameFinishController : NetworkBehaviour
    {
        private readonly Dictionary<NetworkObject, HealthComponent> _playerTeamUnits = new();
        
        private GameCycle _gameCycle;

        [Inject]
        public void Construct(GameCycle gameCycle)
        {
            _gameCycle = gameCycle;
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameCycle.State != GameState.Run) 
                return;

            if (IsGameWon())
            {
                _gameCycle.WinGame();
                Debug.Log("<color=green>GAME WON</color>");
            }
            else if (IsGameLost())
            {
                _gameCycle.LoseGame();
                Debug.Log("<color=red>GAME LOSE</color>");
            }
        }

        public void AddPlayerTeamUnit(NetworkObject unit) => 
            _playerTeamUnits.Add(unit, unit.GetBehaviour<HealthComponent>());

        public void RemovePlayerTeamUnit(NetworkObject unit) => 
            _playerTeamUnits.Remove(unit);

        private bool IsGameWon()
        {

            
            return false;
        }

        private bool IsGameLost()
        {
            foreach (HealthComponent unit in _playerTeamUnits.Values)
                if(unit.IsDead)
                    return true;
            
            return false;
        }
    }
}