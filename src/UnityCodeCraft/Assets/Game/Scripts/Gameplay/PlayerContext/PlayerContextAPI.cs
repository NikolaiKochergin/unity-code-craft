using Atomic.Entities;
using Game.Gameplay;

public static class PlayerContextAPI
{
    public static ValueKey<IPlayerContext, IGameEntity> Character => new(nameof(Character));
}