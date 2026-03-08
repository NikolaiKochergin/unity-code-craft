using System;
using System.Collections.Generic;
using SnakeGame;
using UnityEngine;

namespace Gameplay.GameContext
{
    public class CoinPoints : IWorldBounds
    {
        private readonly IWorldBounds _worldBounds;

        public CoinPoints(IWorldBounds worldBounds) => 
            _worldBounds = worldBounds;

        public Vector2Int[] GetRandomPoints(int count)
        {
            if(count <= 0)
                throw new ArgumentException("Count must be greater than zero");
            
            List<Vector2Int> points = new(count);

            for (int i = 0; i < count; i++)
            {
                bool isAdded = false;
                while (!isAdded)
                {
                    Vector2Int point = _worldBounds.GetRandomPosition();
                    
                    if(points.Contains(point))
                        continue;
                    
                    points.Add(point);
                    isAdded = true;
                }
            }

            return points.ToArray();
        }

        public bool IsInBounds(Vector2Int position) => 
            _worldBounds.IsInBounds(position);

        public Vector2Int GetRandomPosition() => 
            _worldBounds.GetRandomPosition();
    }
}