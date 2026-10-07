using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public class EnemyManager : NetworkBehaviour
    {
        [SerializeField] private int _waveCapacity = 50;
        [SerializeField] private Vector2 _spawnInterval;
        
        private NetworkObject _portal;
        private GameCycle _gameCycle;

        private float _timer;
        private EnemyCharacterSpawner _spawner;

        private int _enemySpawned;
        
        public int WaveCapacity => _waveCapacity;

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
            
            if(_enemySpawned == _waveCapacity)
                return;

            _timer -= Runner.DeltaTime;
            if (_timer <= 0)
            {
                _timer = Random.Range(_spawnInterval.x, _spawnInterval.y);
                NetworkObject enemy = _spawner.SpawnCharacter();
                enemy.GetBehaviour<MoveToTargetComponent>().SetTarget(_portal.transform);
                _enemySpawned++;
            }
        }
    }
}