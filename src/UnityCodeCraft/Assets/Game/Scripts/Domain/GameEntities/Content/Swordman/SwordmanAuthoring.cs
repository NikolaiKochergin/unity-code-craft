using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public sealed class SwordmanAuthoring : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        [Header("Health")]
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _armor;
        
        [Header("Death")] 
        [SerializeField] private float _deathDuration;

        [Header("Fire")] 
        [SerializeField] private float _fireCooldown;
        [SerializeField] private float _fireDelay;

        [Header("Attack")] 
        [SerializeField] private float _attackDistance;
        [SerializeField] private int _damage;
        [SerializeField] private TeamType _team;

        public sealed class SwordmanBaker : Baker<SwordmanAuthoring>
        {
            public override void Bake(SwordmanAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Swordman>()
                    .With<Unit>()
                    .With(new Team{ Value = authoring._team })
                    // Health
                    .With(new CurrentHealth { Value = authoring._currentHealth })
                    .With(new MaxHealth { Value = authoring._maxHealth })
                    .With(new ArmorMultiplier { Value = authoring._armor })
                    // Take Damage
                    .WithBuffer<TakeDamageRequest>()
                    .WithBuffer<TakeDamageEvent>()
                    // Death
                    .WithEnabled<DeathEvent>(false)
                    .WithEnabled(new DeathCooldown{ Duration = authoring._deathDuration }, enabled: false)
                    // Movement
                    .WithEnabled<MoveRequest>(false)
                    .With<MoveEvent>()
                    .With(new MoveSpeed { Value = authoring._moveSpeed })
                    .With(new RotationSpeed { Value = authoring._rotationSpeed })
                    // Fire
                    .WithEnabled<FireRequest>(false)
                    .WithEnabled<FireEvent>(false)
                    .WithEnabled(new FireDelay{ Time = authoring._fireDelay, Duration = authoring._fireDelay }, enabled: false)
                    .With(new FireCooldown{ Duration = authoring._fireCooldown })
                    // Attack
                    .With(new AttackDistance{ Value = authoring._attackDistance })
                    .With(new Damage{ Value = authoring._damage })
                    .With<TargetEntity>()
                ;
        }
    }
}