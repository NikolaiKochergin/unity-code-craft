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
        
        [Header("Death")] 
        [SerializeField] private float _deathDuration;

        [Header("Fire")] 
        [SerializeField] private float _fireCooldown;

        [Header("Attack")] 
        [SerializeField] private float _attackDistance;
        [SerializeField] private int _damage;

        [SerializeField] private GameObject _fakeTarget;
        [SerializeField] private TeamType _fakeTeam;

        public sealed class SwordmanBaker : Baker<SwordmanAuthoring>
        {
            public override void Bake(SwordmanAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Swordman>()
                    .With<Unit>()
                    // TODO:
                    // .With<Team>()
                    
                    // Health
                    .With(new CurrentHealth { Value = authoring._currentHealth })
                    .With(new MaxHealth { Value = authoring._maxHealth })
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
                    .With(new FireCooldown{ Duration = authoring._fireCooldown })
                    // Attack
                    .With(new AttackDistance{ Value = authoring._attackDistance })
                    .With(new Damage{ Value = authoring._damage })
                    // TODO:
                    // .With<TargetEntity>()
                    
                    // Take Damage
                    .WithBuffer<TakeDamageRequest>()
                    .WithBuffer<TakeDamageEvent>()
                    
                
                    .With(new TargetEntity{ Value = GetEntity(authoring._fakeTarget, TransformUsageFlags.None)})
            
                    .With(new Team{ Value = authoring._fakeTeam })
                ;
        }
    }
}