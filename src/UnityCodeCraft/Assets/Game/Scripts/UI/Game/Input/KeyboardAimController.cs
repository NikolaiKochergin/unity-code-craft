using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public class KeyboardAimController : IGameUITick
    {
        public void Tick(IGameUI entity, float _)
        {
            Vector2 aimDirection = entity.GetValue(GameUIAPI.InputMap).GetAimDirection();
            
            if(entity.GetValue(GameUIAPI.AimJoystick).Direction == Vector2.zero)
                entity.ProcessCharacterAim(aimDirection);
        }
    }
}