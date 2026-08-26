using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class WarlockAuthoring : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        
        [Header("Death")] 
        [SerializeField] private float _deathDuration;
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        [Header("Mana")] 
        [SerializeField] private int _maxMana;
        [SerializeField] private float _restoreManaDuration;
        
        [Header("Fire")] 
        [SerializeField] private float _fireCooldown;
        [SerializeField] private float _fireDelay;

        [Header("Attack")] [SerializeField] private int _spellCost;
        [SerializeField] private float _attackDistance;
        [SerializeField] private float _attackRadius;
        [SerializeField] private int _damage;
        
        public class WarlockBaker : Baker<WarlockAuthoring>
        {
            public override void Bake(WarlockAuthoring authoring)
            {
                this.Entity()
                    .With<Warlock>()
                    .With<Unit>()
                    .With<Team>()
                    // Health
                    .With(new CurrentHealth { Value = authoring._currentHealth })
                    .With(new MaxHealth { Value = authoring._maxHealth })
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
                    // Mana
                    .With(new Mana { Value = authoring._maxMana })
                    .With(new MaxMana { Value = authoring._maxMana })
                    .WithEnabled(new ManaRestoreCooldown { Time = authoring._restoreManaDuration, Duration = authoring._restoreManaDuration })
                    // Fire
                    .WithEnabled<FireRequest>(false)
                    .WithEnabled<FireEvent>(false)
                    .WithEnabled(new FireDelay{ Time = authoring._fireDelay, Duration = authoring._fireDelay }, enabled: false)
                    .With(new FireCooldown{ Duration = authoring._fireCooldown })
                    // Heal
                    .WithBuffer<TakeHealRequest>()
                    .WithBuffer<TakeHealEvent>()
                    // Attack
                    .With(new AttackDistance{ Value = authoring._attackDistance })
                    .With(new AttackRadius{ Value = authoring._attackRadius })
                    .With(new Damage{ Value = authoring._damage })
                    .With(new SpellCost{ Value = authoring._spellCost })
                    .With<TargetEntity>()
                ;
            }
        }
    }
}