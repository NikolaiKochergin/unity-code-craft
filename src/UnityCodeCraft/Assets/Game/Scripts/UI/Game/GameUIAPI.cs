using Atomic.Entities;
using Game.Modules;
using TMPro;

namespace Game.UI
{
    public static class GameUIAPI
    {
        public static ValueKey<IGameUI, Joystick> MoveJoystick = new(nameof(MoveJoystick));
        public static ValueKey<IGameUI, Joystick> AttackJoystick = new(nameof(AttackJoystick));
        public static ValueKey<IGameUI, HealthScreenView> HealthScreenView = new(nameof(HealthScreenView));
        public static ValueKey<IGameUI, StatView> HealthView = new(nameof(HealthView));
        public static ValueKey<IGameUI, StatView> AmmoView = new(nameof(AmmoView));
        public static ValueKey<IGameUI, TMP_Text> KillsView = new(nameof(KillsView));
    }
}