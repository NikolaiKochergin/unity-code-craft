using Atomic.Entities;
using Game.Modules;

namespace Game.UI
{
    public static class GameUIAPI
    {
        public static ValueKey<IGameUI, Joystick> Joystick = new(nameof(Joystick));
    }
}