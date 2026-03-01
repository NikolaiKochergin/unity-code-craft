using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class EnemySpawnCooldown : MonoBehaviour
    {
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private float _minSpawnCooldown = 2;
        [SerializeField] private float _maxSpawnCooldown = 3;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        private void Start() => 
            ResetSpawnCooldown();

        public void Disable() => 
            enabled = false;

        private void FixedUpdate()
        {
            if (Time.fixedTime - _spawnTime < _spawnCooldown)
                return;
            
            _enemyManager.SpawnEnemy();
            ResetSpawnCooldown();
        }

        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }
    }
}