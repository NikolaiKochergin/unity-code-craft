using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public class KeyboardMoveController : IGameUITick
    {
        public void Tick(IGameUI entity, float _)
        {
            Vector2 moveDirection = entity.GetValue(GameUIAPI.InputMap).GetMoveDirection();
            
            if(entity.GetValue(GameUIAPI.MoveJoystick).Direction == Vector2.zero)
                entity.ProcessCharacterMove(moveDirection);
            
        }
    }
}