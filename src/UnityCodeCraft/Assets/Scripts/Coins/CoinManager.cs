using Modules;
using SnakeGame;

namespace Gameplay.GameContext
{
    public class CoinManager
    {
        private readonly IDifficulty _difficulty;
        private readonly CoinFactory _factory;
        private readonly IWorldBounds _worldBounds;

        public CoinManager(IDifficulty difficulty, CoinFactory factory, IWorldBounds worldBounds)
        {
            _difficulty = difficulty;
            _factory = factory;
            _worldBounds = worldBounds;
        }

        public int ActiveCoins { get; set; }

        public void SpawnCoins()
        {
            if (!_difficulty.Next(out int difficulty))
                return;

            for (int i = 0; i < difficulty; i++) 
                _factory.CreateCoin(_worldBounds.GetRandomPosition());
        }
    }
}