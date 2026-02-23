using UnityEngine;

namespace Game
{
    public sealed class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShipConfig config;
        [SerializeField] private HealthComponent _health;
        [SerializeField] private MoveComponent _move;
        [SerializeField] private AttackComponent _attack;

        public IHealth Health => _health;
        public IMover Mover => _move;
        public IAttack Attack => _attack;
        
        private void Awake()
        {
            _health.Setup(config.Health);
            _move.Setup(config.Move);
            _attack.Setup(config.Attack, () => Health.IsAlive);
        }

        private void FixedUpdate() => 
            Mover.Update(Time.fixedDeltaTime);
    }
}