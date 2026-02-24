using System;
using System.Collections;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public sealed class EnemyOrchestrator : MonoBehaviour, IEnemyDespawner
    {
        [Header("Spawn")]
        [SerializeField] private float _minSpawnCooldown = 2;
        [SerializeField] private float _maxSpawnCooldown = 3;
        
        private float _spawnCooldown;
        private float _spawnTime;
        
        [Header("Pool")]
        [SerializeField] private EnemyAi _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _initialCapacity = 10;
        
        private ComponentPool<EnemyAi> _pool;

        [Header("Target")]
        [SerializeField] private Ship _player;
        
        [Header("Points")]
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;
        
        private int _spawnIndex;
        private int _attackIndex;
        
        [Header("Bullets")]
        [SerializeField] private BulletSystem _bulletWorld;
        
        public int DestroyedEnemies { get; private set; }

        public event Action OnEnemyDied;
        
        private void Awake()
        {
            _pool = new ComponentPool<EnemyAi>(_prefab, _initialCapacity, _container);
            
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }
        
        private void Start() => 
            ResetSpawnCooldown();

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - _spawnTime < _spawnCooldown || !_player.Health.IsAlive)
                return;
            
            EnemyAi enemy = _pool.Get();

            enemy.transform.position = NextSpawnPosition();
            enemy.Setup(this, _player.Health, NextDestination(), OnFire);
            enemy.gameObject.SetActive(true);
                
            ResetSpawnCooldown();
        }

        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }

        public void Despawn(EnemyAi enemy)
        {
            DestroyedEnemies++;
            OnEnemyDied?.Invoke();
            StartCoroutine(DespawnInNextFrame(enemy));
        }

        private IEnumerator DespawnInNextFrame(EnemyAi enemy)
        {
            yield return null;
            enemy.gameObject.SetActive(false);
            _pool.Return(enemy);
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