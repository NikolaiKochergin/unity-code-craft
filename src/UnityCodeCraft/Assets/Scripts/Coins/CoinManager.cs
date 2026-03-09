using System;
using System.Collections.Generic;
using Modules;
using UnityEngine;

namespace Game
{
    public class CoinManager
    {
        private readonly CoinsPool _pool;
        private readonly WorldPoints _worldPoints;
        private readonly Dictionary<Vector2Int, Coin> _activeCoins = new();

        public CoinManager(CoinsPool pool, WorldPoints worldPoints)
        {
            _pool = pool;
            _worldPoints = worldPoints;
        }

        public event Action OnCoinsOver;

        public void SpawnCoins(int count)
        {
            List<Vector2Int> points = _worldPoints.GetRandomPoints(count);

            foreach (Vector2Int point in points) 
                _activeCoins.Add(point, _pool.Spawn(point));
        }

        public void DespawnCoin(ICoin coin)
        {
            if(_activeCoins.Remove(coin.Position, out Coin activeCoin))
                _pool.Despawn(activeCoin);
            
            if(_activeCoins.Count == 0)
                OnCoinsOver?.Invoke();
        }

        public bool TryGetCoinAt(Vector2Int position, out ICoin coin)
        {
            if (_activeCoins.TryGetValue(position, out Coin activeCoin))
            {
                coin = activeCoin;
                return true;
            }

            coin = null;
            return false;
        }
    }
}