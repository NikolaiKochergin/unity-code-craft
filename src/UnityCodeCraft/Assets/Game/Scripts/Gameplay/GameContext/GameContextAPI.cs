using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class GameContextAPI
    {
        public static ValueKey<IGameContext, PlayerContext> PlayerContext = new(nameof(PlayerContext));
    }
}