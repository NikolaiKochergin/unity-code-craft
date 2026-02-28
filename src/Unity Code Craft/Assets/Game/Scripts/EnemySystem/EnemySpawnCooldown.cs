using UnityEngine;

namespace Game
{
    public class EnemySpawnCooldown : MonoBehaviour
    {
        [SerializeField] private float _minSpawnCooldown = 2;
        [SerializeField] private float _maxSpawnCooldown = 3;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        public bool ReadyToSpawn => Time.fixedTime - _spawnTime >= _spawnCooldown;
        
        private void Start() => 
            ResetSpawnCooldown();

        public void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }
    }
}