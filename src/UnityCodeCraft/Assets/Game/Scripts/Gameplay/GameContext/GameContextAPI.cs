using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class GameContextAPI
    {
        public static ValueKey<IGameContext, IPlayerContext> PlayerContext = new(nameof(PlayerContext));
    }
}