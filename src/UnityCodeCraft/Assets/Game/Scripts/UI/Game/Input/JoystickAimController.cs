using UnityEngine;

namespace Game.UI
{
    public class JoystickAimController : IGameUITick
    {
        public void Tick(IGameUI ui, float _)
        {
            Vector2 aimDirection = ui.GetAimDirection();
            ui.ProcessCharacterAim(aimDirection);
        }
    }
}