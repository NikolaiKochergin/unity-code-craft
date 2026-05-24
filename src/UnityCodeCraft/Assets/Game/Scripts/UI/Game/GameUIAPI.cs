using Atomic.Entities;
using Game.Gameplay;
using Game.Modules;
using TMPro;

namespace Game.UI
{
    public static class GameUIAPI
    {
        public static ValueKey<IGameUI, Joystick> MoveJoystick = new(nameof(MoveJoystick));
        public static ValueKey<IGameUI, Joystick> AimJoystick = new(nameof(AimJoystick));
        public static ValueKey<IGameUI, HealthScreenView> HealthScreenView = new(nameof(HealthScreenView));
        public static ValueKey<IGameUI, StatView> HealthView = new(nameof(HealthView));
        public static ValueKey<IGameUI, StatView> AmmoView = new(nameof(AmmoView));
        public static ValueKey<IGameUI, TMP_Text> KillsView = new(nameof(KillsView));
        public static ValueKey<IGameUI, InputMap> InputMap = new(nameof(InputMap));
        public static ValueKey<IGameUI, IGameContext> GameContext = new(nameof(GameContext));
    }
}