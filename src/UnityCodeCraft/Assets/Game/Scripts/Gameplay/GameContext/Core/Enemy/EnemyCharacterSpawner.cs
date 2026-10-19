using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class EnemyCharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        
        private SpawnPointService _spawnPointService;
        private DiContainer _container;

        [Inject]
        public void Construct(
            [Inject(Id = Tags.Enemy)] SpawnPointService spawnPointService,
            DiContainer container
        )
        {
            _container = container;
            _spawnPointService = spawnPointService;
        }

        public NetworkObject SpawnCharacter()
        {
            Transform spawnPoint = _spawnPointService.GetRandomSpawnPoint();
            NetworkObject character = Runner.Spawn(
                _enemyConfig.Prefab,
                spawnPoint.position,
                spawnPoint.rotation);
            
            _container.Inject(character);

            if(character.TryGetBehaviour(out MoneyBag bag))
                bag.RewardAmount = Random.Range(_enemyConfig.Reward.x, _enemyConfig.Reward.y + 1);


            return character;
        }
    }
}