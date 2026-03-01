using System;
using UnityEngine;

namespace Game
{
    public sealed class Ship : MonoBehaviour
    {
        [SerializeField] private ShipConfig _config;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private MoveComponent _move;
        [SerializeField] private FireComponent _fire;

        public Vector2 Position { get => transform.position; set => transform.position = value; }
        public Vector3 Direction { get => _move.Direction; set => _move.SetDirection(value); }
        public int HealthCurrent => _health.Current;
        public int HealthMax => _health.Max;
        public bool IsAlive => _health.IsAlive;

        public event Action<int> OnHealthChanged
        {
            add => _health.OnChanged += value;
            remove => _health.OnChanged -= value;
        }

        public event Action OnDied
        {
            add => _health.OnDied += value;
            remove => _health.OnDied -= value;
        }

        public event Action OnFire
        {
            add => _fire.OnFire += value;
            remove => _fire.OnFire -= value;
        }

        private void Awake()
        {
            _health.Setup(_config.Health);
            _move.Setup(_config.Move);
            _fire.Setup(_config.Fire);
        }

        public void FireAt(Vector2 position)
        {
            if(_health.IsAlive)
                _fire.FireAt(position);
        }

        public void FireUp() => 
            _fire.FireUp();

        public void TakeDamage(int value) => 
            _health.TakeDamage(value);

        private void FixedUpdate()
        {
            if(_health.IsAlive)
                _move.Move();
        }
    }
}