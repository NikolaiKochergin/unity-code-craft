using Atomic.Entities;
using Game.Gameplay;
using Game.UI;

public static class PlayerContextAPI
{
    public static ValueKey<IPlayerContext, IGameEntity> Character => new(nameof(Character));
}