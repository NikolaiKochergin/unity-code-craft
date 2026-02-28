using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Ship _player;

        [SerializeField] private EnemyPool _pool;
        [SerializeField] private BulletSystem _bulletSystem;
        [SerializeField] private EnemySpawnCooldown _spawnCooldown;
        [SerializeField] private EnemyPositions _enemyPositions;
        
        public int DestroyedEnemies { get; private set; }

        public event Action OnDestroyedEnemiesChanged;

        private void FixedUpdate()
        {
            if (!_spawnCooldown.ReadyToSpawn || !_player.IsAlive)
                return;
            
            SpawnEnemy();

            _spawnCooldown.ResetSpawnCooldown();
        }

        private void SpawnEnemy()
        {
            Enemy enemy = _pool.Get();
            enemy.transform.position = _enemyPositions.NextSpawnPosition();
            enemy.Setup(_bulletSystem, _player, _enemyPositions.NextDestination());
            enemy.OnDied += OnEnemyDied;
            enemy.gameObject.SetActive(true);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            DestroyedEnemies++;
            OnDestroyedEnemiesChanged?.Invoke();
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            _pool.Return(enemy);
        }
    }
}