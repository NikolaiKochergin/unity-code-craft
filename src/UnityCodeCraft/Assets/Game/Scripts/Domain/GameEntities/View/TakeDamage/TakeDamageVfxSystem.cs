using Unity.Entities;
using UnityEngine;
using static Unity.Entities.SystemAPI.ManagedAPI;

namespace Game
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageVfxSystem : ISystem
    {
        private BufferLookup<TakeDamageEvent> _takeDamageEventLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TakeDamageVfx>();

            _takeDamageEventLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(isReadOnly: true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _takeDamageEventLookup.Update(ref state);

            foreach ((
                         RefRO<ModelEntity> modelEntityRef, 
                         UnityEngineComponent<ParticleSystem> particleSystemRef) 
                     in SystemAPI.Query<
                         RefRO<ModelEntity>,
                         UnityEngineComponent<ParticleSystem>>()
                         .WithAll<TakeDamageVfx>())
            {
                Entity modelEntity = modelEntityRef.ValueRO.Value;

                DynamicBuffer<TakeDamageEvent> buffer = _takeDamageEventLookup[modelEntity];
                if(!buffer.IsEmpty)
                    particleSystemRef.Value.Play();
            }
        }
    }
}