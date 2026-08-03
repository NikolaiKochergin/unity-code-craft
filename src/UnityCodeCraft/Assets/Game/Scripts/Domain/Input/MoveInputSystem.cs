using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [DisableAutoCreation]
    public partial class MoveInputSystem : SystemBase
    {
        private InputAction _moveAction;

        protected override void OnCreate()
        {
            _moveAction = InputSystem.actions.FindAction("Player/Move");
        }

        protected override void OnUpdate()
        {
            Vector2 direction = _moveAction.ReadValue<Vector2>();

            float3 moveDirection = new(direction.x, 0, direction.y);
            if (math.all(moveDirection == float3.zero) )
                return;
            
            foreach ((RefRW<MoveRequest> moveRequestValue, EnabledRefRW<MoveRequest> moveRequestEnabled) in 
                     SystemAPI.Query<RefRW<MoveRequest>, EnabledRefRW<MoveRequest>>().WithPresent<Unit, MoveRequest>())
            {
                moveRequestEnabled.ValueRW = true;
                moveRequestValue.ValueRW.Direction = moveDirection;
            }
        }
    }
}