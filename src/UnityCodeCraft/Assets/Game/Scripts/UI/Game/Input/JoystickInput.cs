using UnityEngine;

namespace Game.UI
{
    public class JoystickInput : IGameUITick
    {
        public void Tick(IGameUI ui, float _)
        {
            Vector2 moveDirection = ui.GetMoveDirection();
            Vector2 aimDirection = ui.GetAimDirection();
            
            ui.ProcessCharacterMove(moveDirection);
            ui.ProcessCharacterAim(aimDirection);
        }
    }
}