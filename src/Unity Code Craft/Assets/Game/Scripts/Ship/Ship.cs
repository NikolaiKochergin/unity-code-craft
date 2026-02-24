using UnityEngine;

namespace Game
{
    public sealed class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShipConfig config;
        [SerializeField] private ShipAnimator _animator;
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
        
        private void OnEnable()
        {
            Attack.OnFire += _animator.AnimateFire;
            Health.OnChanged += _animator.AnimateDamage;
            Health.OnDied += _animator.AnimateDeath;
        }

        private void OnDisable()
        {
            Attack.OnFire -= _animator.AnimateFire;
            Health.OnChanged -= _animator.AnimateDamage;
            Health.OnDied -= _animator.AnimateDeath;
        }

        private void FixedUpdate() => 
            Mover.Update(Time.fixedDeltaTime);

        private void LateUpdate() => 
            _animator.AnimateMovement(Mover.Direction, Time.deltaTime);
    }
}