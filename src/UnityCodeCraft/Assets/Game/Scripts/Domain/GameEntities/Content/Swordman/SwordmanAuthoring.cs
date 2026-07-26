using Unity.Entities;
using UnityEngine;

namespace Game
{
    public sealed class SwordmanAuthoring : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;
        
        [SerializeField]
        private float _rotationSpeed;

        public sealed class SwordmanBaker : Baker<SwordmanAuthoring>
        {
            public override void Bake(SwordmanAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Unit>()
                    .With<Swordman>()
                    .WithEnabled<MoveRequest>(false)
                    .With<MoveEvent>()
                    .With(new MoveSpeed { Value = authoring._moveSpeed })
                    .With(new RotationSpeed() { Value = authoring._rotationSpeed });
        }
    }
}