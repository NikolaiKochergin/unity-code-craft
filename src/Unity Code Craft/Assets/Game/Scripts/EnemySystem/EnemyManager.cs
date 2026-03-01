using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyPool _pool;
        [SerializeField] private EnemyPositions _enemyPositions;
        
        public int DestroyedEnemies { get; private set; }

        public event Action OnDestroyedEnemiesChanged;

        public void SpawnEnemy()
        {
            Enemy enemy = _pool.Get();
            enemy.SetPosition(_enemyPositions.NextSpawnPosition());
            enemy.SetDestination(_enemyPositions.NextDestination());
            enemy.OnDied += OnEnemyDied;
            enemy.Enable();
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.OnDied -= OnEnemyDied;
            StartCoroutine(DespawnInNextFrame(enemy));
            
            DestroyedEnemies++;
            OnDestroyedEnemiesChanged?.Invoke();
        }

        private IEnumerator DespawnInNextFrame(Enemy enemy)
        {
            yield return null;
            enemy.Disable();
            _pool.Return(enemy);
        }
    }
}