using Atomic.Entities;
using Game.Modules;

namespace Game.UI
{
    public static class GameUIAPI
    {
        public static ValueKey<IGameUI, Joystick> MoveJoystick = new(nameof(MoveJoystick));
        public static ValueKey<IGameUI, Joystick> AttackJoystick = new(nameof(AttackJoystick));
    }
}