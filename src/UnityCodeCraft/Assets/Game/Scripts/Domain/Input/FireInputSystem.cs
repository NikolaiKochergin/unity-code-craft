using Game.Scripts.Domain.GameEntities.Core.Fire;
using Unity.Entities;
using UnityEngine.InputSystem;

namespace Game
{
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
            //              RefRW<FireRequest> requestValue
            //         ) in SystemAPI.Query<
            //         EnabledRefRW<FireRequest>,
            //         RefRW<FireRequest>>()
            //         .WithPresent<Unit, FireRequest>()
            // )
            // {
            //     requestEnabled.ValueRW = true;
            // }
            
            foreach ((
                         EnabledRefRW<FireEvent> requestEnabled, 
                         RefRW<FireEvent> requestValue
                     ) in SystemAPI.Query<
                             EnabledRefRW<FireEvent>,
                             RefRW<FireEvent>>()
                         .WithPresent<Unit, FireEvent>()
                    )
            {
                requestEnabled.ValueRW = true;
            }
        }
    }
}