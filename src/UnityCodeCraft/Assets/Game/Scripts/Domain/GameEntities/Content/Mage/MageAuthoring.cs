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
        
        
        [Header("Heal")] 
        [SerializeField] private float _healDistance;
        [SerializeField] private int _heal;
        
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
                
                    // Attack
                    // .With(new AttackDistance{ Value = authoring._attackDistance })
                    // .With(new Damage{ Value = authoring._damage })
                    .With<TargetEntity>()
                ;
        }
    }
}