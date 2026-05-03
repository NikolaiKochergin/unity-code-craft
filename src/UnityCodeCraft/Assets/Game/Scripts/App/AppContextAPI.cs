using Atomic.Entities;
using Game.Gameplay;
using Game.UI;

namespace Game.App
{
    public static class AppContextAPI
    {
        public static ValueKey<IAppContext, GameContext> GameContext = new(nameof(GameContext));
        public static ValueKey<IAppContext, GameUI> GameUI = new(nameof(GameUI));
    }
}