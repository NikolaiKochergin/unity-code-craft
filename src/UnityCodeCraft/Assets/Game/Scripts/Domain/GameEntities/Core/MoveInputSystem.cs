using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    // [DisableAutoCreation]
    public partial class MoveInputSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dx = Input.GetAxis("Horizontal");
            float dz = Input.GetAxis("Vertical");

            float3 moveDirection = new float3(dx, 0, dz);
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