using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class ProjectileAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;
        
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _stoppingDistance;
        [SerializeField] private Vector3 _targetOffset;
        
        [Header("Attack")]
        [SerializeField] private int _damage;
        
        [SerializeField] private float _lifetime;

        public class ProjectileBaker : Baker<ProjectileAuthoring>
        {
            public override void Bake(ProjectileAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Projectile>()
                    // Attack
                    .With(new Team { Value = authoring._team })
                    .With(new Damage { Value = authoring._damage })
                    .With<TargetEntity>()
                    // Move
                    .With(new MoveSpeed { Value = authoring._moveSpeed})
                    .With(new RotationSpeed { Value = authoring._rotationSpeed })
                    .With(new StoppingDistance { Value = authoring._stoppingDistance })
                    .With(new TargetOffset { Value = authoring._targetOffset})
                
                    .With(new Lifetime { Value = authoring._lifetime })
                ;
        }
    }
}