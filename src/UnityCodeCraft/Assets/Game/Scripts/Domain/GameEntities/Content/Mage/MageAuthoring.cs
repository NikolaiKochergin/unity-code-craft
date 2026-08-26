using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class MageAuthoring : MonoBehaviour
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
        
        [Header("Heal")] 
        [SerializeField] private int _healManaCost;
        [SerializeField] private float _healDistance;
        [SerializeField] private int _healValue;
        
        public class MageBaker : Baker<MageAuthoring>
        {
            public override void Bake(MageAuthoring authoring) =>
                this.Entity()
                    .With<Mage>()
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
                    .With(new AttackDistance{ Value = authoring._healDistance })
                    .With(new Heal{ Value = authoring._healValue })
                    .With(new SpellCost{ Value = authoring._healManaCost })
                    .With<TargetEntity>()
                
                    // Heal
                    .WithBuffer<TakeHealRequest>()
                    .WithBuffer<TakeHealEvent>()
                ;
        }
    }
}