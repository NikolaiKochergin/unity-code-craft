using Atomic.Entities;
using UnityEngine;

namespace Game.UI
{
    public static class UIInputUseCase
    {
        public static Vector2 GetMoveDirection(this IGameUI ui) => 
            ui.GetValue(GameUIAPI.MoveJoystick).Direction;
        
        public static Vector2 GetAimDirection(this IGameUI ui) => 
            ui.GetValue(GameUIAPI.AimJoystick).Direction;
    }
}