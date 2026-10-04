using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public class EnemyManager : NetworkBehaviour
    {
        [SerializeField] private float _spawnInterval = 2f;
        
        private NetworkObject _portal;
        private GameCycle _gameCycle;

        private float _timer;
        private EnemyCharacterSpawner _spawner;

        [Inject]
        public void Construct(
            [Inject(Id = Tags.Portal)] NetworkObject portal,
            GameCycle gameCycle,
            EnemyCharacterSpawner spawner)
        {
            _spawner = spawner;
            _gameCycle = gameCycle;
            _portal = portal;
        }

        public override void FixedUpdateNetwork()
        {
            if(_gameCycle.State != GameState.Run)
                return;

            _timer -= Runner.DeltaTime;
            if (_timer <= 0)
            {
                _timer = _spawnInterval;
                NetworkObject enemy = _spawner.SpawnCharacter();
                
                enemy.GetBehaviour<MoveToTargetComponent>().SetTarget(_portal.transform);
            }
        }
    }
}