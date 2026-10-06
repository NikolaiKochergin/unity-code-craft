using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class GameFinishController : NetworkBehaviour
    {
        private GameCycle _gameCycle;
        private KillNotificator _killNotificator;

        [Inject]
        public void Construct(
            GameCycle gameCycle,
            KillNotificator killNotificator)
        {
            _killNotificator = killNotificator;
            _gameCycle = gameCycle;
        }

        public override void Spawned() => 
            _killNotificator.OnKilled += OnKilled;

        public override void Despawned(NetworkRunner _, bool __)
        {
            _killNotificator.OnKilled -= OnKilled;
        }

        private void OnKilled(KillArgs args)
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
            
            // _gameCycle.WinGame();
            // Debug.Log("<color=green>GAME WON</color>");
        }
    }
}