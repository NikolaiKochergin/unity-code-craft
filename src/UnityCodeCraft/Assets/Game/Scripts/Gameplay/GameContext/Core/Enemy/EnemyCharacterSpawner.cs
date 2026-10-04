using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class EnemyCharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        
        private SpawnPointService _spawnPointService;

        [Inject]
        public void Construct(
            [Inject(Id = Tags.Enemy)] SpawnPointService spawnPointService
        )
        {
            _spawnPointService = spawnPointService;
        }

        public NetworkObject SpawnCharacter()
        {
            Transform spawnPoint = _spawnPointService.GetRandomSpawnPoint();
            NetworkObject character = Runner.Spawn(
                _enemyConfig.Prefab,
                spawnPoint.position,
                spawnPoint.rotation);

            return character;
        }
    }
}