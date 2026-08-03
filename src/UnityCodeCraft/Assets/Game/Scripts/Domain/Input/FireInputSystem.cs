using Unity.Entities;
using UnityEngine.InputSystem;

namespace Game
{
    [DisableAutoCreation]
    public partial class FireInputSystem : SystemBase
    {
        private InputAction _fireAction;

        protected override void OnCreate()
        {
            _fireAction = InputSystem.actions.FindAction("Player/Fire");
        }
        
        protected override void OnUpdate()
        {
            if(!_fireAction.WasPressedThisFrame())
                return;

            // foreach ((
            //              EnabledRefRW<FireRequest> requestEnabled, 
            //              RefRW<FireRequest> requestValue,
            //              RefRO<TargetEntity> target
            //         ) in SystemAPI.Query<
            //                  EnabledRefRW<FireRequest>,
            //                  RefRW<FireRequest>,
            //                  RefRO<TargetEntity>>()
            //              .WithPresent<Unit, FireRequest>()
            // )
            // {
            //     requestEnabled.ValueRW = true;
            //     requestValue.ValueRW.Target = target.ValueRO.Value;
            // }
            
            foreach (
                EnabledRefRW<FireEvent> fireEventEnabled 
                in SystemAPI.Query<
                        EnabledRefRW<FireEvent>>()
                    .WithPresent<Unit, FireEvent>())
            {
                fireEventEnabled.ValueRW = true;
            }
        }
    }
}