using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameFinishController : NetworkBehaviour, IKillHandler
    {
        private GameCycle _gameCycle;
        private EnemySpawnController _enemySpawnController;

        private int _enemyKilled;

        [Inject]
        public void Construct(
            GameCycle gameCycle,
            EnemySpawnController enemySpawnController)
        {
            _enemySpawnController = enemySpawnController;
            _gameCycle = gameCycle;
        }

        public void Handle(KillArgs args)
        {
            NetworkObject victim = Runner.FindObject(args.Victim);
            if(victim == null)
                return;

            if (victim.GetBehaviour<Character>() || victim.GetBehaviour<Portal>())
            {
                Debug.Log("<color=red>GAME LOSE</color>");
                _gameCycle.LoseGame();
                return;
            }

            if (victim.GetBehaviour<Enemy>())
            {
                _enemyKilled++;
                if(_enemyKilled != _enemySpawnController.WaveCapacity)
                    return;
                
                _gameCycle.WinGame();
                Debug.Log("<color=green>GAME WON</color>");
            }
        }
    }
}