using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Game.UI
{
    public static class UIInputUseCase
    {
        public static Vector2 GetMoveDirection(this IGameUI ui) => 
            ui.GetValue(GameUIAPI.MoveJoystick).Direction;
        
        public static Vector2 GetAimDirection(this IGameUI ui) => 
            ui.GetValue(GameUIAPI.AimJoystick).Direction;

        public static void ProcessCharacterMove(this IGameUI ui, Vector2 direction) =>
            ui
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Character)
                .GetValue(GameEntityAPI.MoveRequest)
                .Invoke(new Vector3(direction.x, 0, direction.y));

        public static void ProcessCharacterAim(this IGameUI ui, Vector2 direction) =>
            ui
                .GetValue(GameUIAPI.GameContext)
                .GetValue(GameContextAPI.Character)
                .GetValue(GameEntityAPI.AimRequest)
                .Invoke(new Vector3(direction.x, 0, direction.y));
    }
}