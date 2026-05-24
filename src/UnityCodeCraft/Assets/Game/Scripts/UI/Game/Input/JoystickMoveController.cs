using UnityEngine;

namespace Game.UI
{
    public class JoystickMoveController : IGameUITick
    {
        public void Tick(IGameUI ui, float _)
        {
            Vector2 moveDirection = ui.GetMoveDirection();

            ui.ProcessCharacterMove(moveDirection);
        }
    }
}