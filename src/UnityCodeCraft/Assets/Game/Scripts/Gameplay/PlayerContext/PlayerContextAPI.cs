using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public static class PlayerContextAPI
    {
        public static ValueKey<IPlayerContext, IGameEntity> Character => new(nameof(Character));
        public static ValueKey<IPlayerContext, InputMap> InputMap => new(nameof(InputMap));
        public static ValueKey<IPlayerContext, IReactiveVariable<int>> Score = new(nameof(Score));
    }
}