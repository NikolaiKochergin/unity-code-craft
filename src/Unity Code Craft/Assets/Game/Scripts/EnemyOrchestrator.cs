using System.Collections;
using System.Collections.Generic;
using Modules.UI;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    // +
    public sealed class EnemyOrchestrator : MonoBehaviour, IEnemyDespawner
    {
        [Header("Spawn")]
        [SerializeField]
        private float _minSpawnCooldown = 2;

        [SerializeField]
        private float _maxSpawnCooldown = 3;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        [Header("Pool")]
        [SerializeField]
        private EnemyAi _prefab;

        [SerializeField]
        private Transform _container;
        
        private readonly Queue<EnemyAi> _pool = new();

        [Header("Target")]
        [SerializeField]
        private Ship _player;
        
        [Header("Points")]
        [SerializeField]
        private Transform[] _spawnPositions;
        
        [SerializeField]
        private Transform[] _attackPositions;
        
        private int _spawnIndex;
        private int _attackIndex;
        
        [Header("Bullets")]
        [SerializeField]
        private BulletSystem _bulletWorld;
        
        [Header("UI")]
        [SerializeField]
        private ScoreView _scoreView;
        
        private int _destroyedEnemies;
        
        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
            _scoreView.SetValue(_destroyedEnemies);
        }
        
        private void Start()
        {
            this.ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - _spawnTime < _spawnCooldown || !_player.Health.IsAlive)
                return;
            
            if (_pool.TryDequeue(out EnemyAi enemy))
                enemy.gameObject.SetActive(true);
            else
                enemy = Instantiate(_prefab, _container);

            enemy.transform.position = this.NextSpawnPosition();
            enemy.Setup(this, _player.Health, NextDestination());
            
            enemy.OnFire += this.OnFire;
                
            ResetSpawnCooldown();
        }

        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }

        public void Despawn(EnemyAi enemy)
        {
            _destroyedEnemies++;
            _scoreView.SetValue(_destroyedEnemies);
            this.StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(EnemyAi enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            _pool.Enqueue(enemy);
        }
        
        private void OnFire(AttackEvent attackEvent)
        {
            Vector2 position = attackEvent.FirePoint.position;
            Vector2 target = _player.transform.position;
            Vector2 direction = (target - position).normalized;
            _bulletWorld.Spawn(
                attackEvent.FirePoint.position,
                direction,
                attackEvent.Speed,
                attackEvent.Damage,
                TeamType.Enemy
            );
        }
        
        private Vector3 NextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (_attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                _attackIndex = 0;
            }

            return _attackPositions[_attackIndex++].position;
        }
    }
}