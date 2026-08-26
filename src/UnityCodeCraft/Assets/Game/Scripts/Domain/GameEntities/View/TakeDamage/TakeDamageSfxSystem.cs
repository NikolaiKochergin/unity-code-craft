using Modules.AudioEvents;
using Unity.Entities;
using Unity.Transforms;

namespace Game
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct TakeDamageSfxSystem : ISystem
    {
        private const float DAMAGE_THRESHOLD = 0.1f;
        
        private BufferLookup<TakeDamageEvent> _takeDamageEventLookup;
        private ComponentLookup<LocalTransform> _transformLookup;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TakeDamageSfx>();

            _transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(isReadOnly: true);
            _takeDamageEventLookup = SystemAPI.GetBufferLookup<TakeDamageEvent>(isReadOnly: true);
        }

        public void OnUpdate(ref SystemState state)
        {
            _takeDamageEventLookup.Update(ref state);
            _transformLookup.Update(ref state);

            AudioSystem audioSystem = AudioSystem.Instance;

            foreach ((
                         RefRO<ModelEntity> modelEntityRef, 
                         RefRO<TakeDamageSfx> takeDamageSfx) 
                     in SystemAPI.Query<
                         RefRO<ModelEntity>,
                         RefRO<TakeDamageSfx>>()
                         .WithAll<TakeDamageSfx>())
            {
                Entity modelEntity = modelEntityRef.ValueRO.Value;
                DynamicBuffer<TakeDamageEvent> buffer = _takeDamageEventLookup[modelEntity];
                if(buffer.IsEmpty)
                    continue;

                ref readonly LocalTransform transform = ref _transformLookup.GetRefRO(modelEntity).ValueRO;
                ref readonly AudioEventKey audioEvent = ref takeDamageSfx.ValueRO.Value;
                audioSystem.PlayEvent(audioEvent, transform.Position, transform.Rotation, DAMAGE_THRESHOLD);
            }
        }
    }
}