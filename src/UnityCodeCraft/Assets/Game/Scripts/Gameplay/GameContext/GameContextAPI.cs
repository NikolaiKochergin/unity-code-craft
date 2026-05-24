using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class GameContextAPI
    {
        public static ValueKey<IGameContext, IGameEntity> Character = new(nameof(Character));
        public static ValueKey<IGameContext, IReactiveVariable<int>> Score = new(nameof(Score));
        
    }
}