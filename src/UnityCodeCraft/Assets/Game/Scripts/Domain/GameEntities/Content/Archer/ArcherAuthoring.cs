using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class ArcherAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;
        
        [Header("Health")]
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _armor;
        
        [Header("Death")] 
        [SerializeField] private float _deathDuration;
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        
        [Header("Fire")] 
        [SerializeField] private float _fireCooldown;
        [SerializeField] private float _fireDelay;
        [SerializeField] private GameObject _projectilePrefab;
        
        [Header("Attack")] 
        [SerializeField] private float _attackDistance;
        [SerializeField] private int _damage;
        
        public class ArcherBaker : Baker<ArcherAuthoring>
        {
            public override void Bake(ArcherAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Archer>()
                    .With<Unit>()
                    .With(new Team{ Value = authoring._team })
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
                    // Fire
                    .WithEnabled<FireRequest>(false)
                    .WithEnabled<FireEvent>(false)
                    .WithEnabled(new FireDelay{ Time = authoring._fireDelay, Duration = authoring._fireDelay }, enabled: false)
                    .With(new FireCooldown{ Duration = authoring._fireCooldown })
                    .With(new ProjectilePrefab{ Value = GetEntity(authoring._projectilePrefab, TransformUsageFlags.Dynamic) })
                    // Attack
                    .With(new AttackDistance{ Value = authoring._attackDistance })
                    .With(new Damage{ Value = authoring._damage })
                    .With<TargetEntity>()
                ;
        }
    }
}