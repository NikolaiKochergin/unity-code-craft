using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public class KeyboardInput : IGameUITick
    {
        public void Tick(IGameUI entity, float _)
        {
            Vector2 moveDirection = entity.GetValue(GameUIAPI.InputMap).GetMoveDirection();
            Vector2 aimDirection = entity.GetValue(GameUIAPI.InputMap).GetAimDirection();
            
            if(entity.GetValue(GameUIAPI.MoveJoystick).Direction == Vector2.zero)
                entity.ProcessCharacterMove(moveDirection);
            
            if(entity.GetValue(GameUIAPI.AimJoystick).Direction == Vector2.zero)
                entity.ProcessCharacterAim(aimDirection);
        }
    }
}