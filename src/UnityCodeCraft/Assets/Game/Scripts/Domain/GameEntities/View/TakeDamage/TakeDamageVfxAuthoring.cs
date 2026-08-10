using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class TakeDamageVfxAuthoring : MonoBehaviour
    {
        public class TakeDamageVfxBaker : Baker<TakeDamageVfxAuthoring>
        {
            public override void Bake(TakeDamageVfxAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<TakeDamageVfx>();
        }
    }
}