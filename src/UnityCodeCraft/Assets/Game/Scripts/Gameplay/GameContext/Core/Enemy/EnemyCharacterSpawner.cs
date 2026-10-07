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

            if(character.TryGetBehaviour(out MoneyBag bag))
                bag.RewardAmount = Random.Range(_enemyConfig.Reward.x, _enemyConfig.Reward.y + 1);

            Weapon weapon = character.GetComponentInChildren<Weapon>();
            if (weapon)
            {
                weapon.SetDamage(_enemyConfig.Damage);
                weapon.SetCooldown(_enemyConfig.PlayerDamageCooldown);
            }

            return character;
        }
    }
}