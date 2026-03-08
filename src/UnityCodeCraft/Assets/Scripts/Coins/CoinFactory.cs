using Modules;
using UnityEngine;

namespace Gameplay.GameContext
{
    public class CoinFactory
    {
        private readonly Coin _coinPrefab;

        public CoinFactory(Coin coinPrefab)
        {
            _coinPrefab = coinPrefab;
        }

        public ICoin CreateCoin(Vector2Int position)
        {
            Coin coin = Object.Instantiate(_coinPrefab);
            coin.Position = position;
            coin.Generate();
            return coin;
        }
    }
}