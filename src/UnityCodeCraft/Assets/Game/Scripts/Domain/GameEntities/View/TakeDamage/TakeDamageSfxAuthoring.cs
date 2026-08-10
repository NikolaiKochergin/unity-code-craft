using Modules.AudioEvents;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class TakeDamageSfxAuthoring : MonoBehaviour
    {
        [SerializeField]
        private AudioEventSerialized _takeDamageSfx;

        public class TakeDamageSfxBaker : Baker<TakeDamageSfxAuthoring>
        {
            public override void Bake(TakeDamageSfxAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With(new TakeDamageSfx { Value = authoring._takeDamageSfx });
        }
    }
}