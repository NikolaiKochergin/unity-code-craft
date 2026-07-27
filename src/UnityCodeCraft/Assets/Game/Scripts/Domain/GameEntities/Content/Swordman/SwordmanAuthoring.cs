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

        public sealed class SwordmanBaker : Baker<SwordmanAuthoring>
        {
            public override void Bake(SwordmanAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Swordman>()
                    .With<Unit>()
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
                
                ;
        }
    }
}