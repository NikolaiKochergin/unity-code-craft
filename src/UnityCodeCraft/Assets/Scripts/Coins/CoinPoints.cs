using System;
using SnakeGame;
using UnityEngine;

namespace Gameplay.GameContext
{
    public class CoinPoints
    {
        private readonly IWorldBounds _worldBounds;

        public CoinPoints(IWorldBounds worldBounds) => 
            _worldBounds = worldBounds;

        public Vector2Int[] GetRandomPoints(int count)
        {
            if(count <= 0)
                throw new ArgumentException("Count must be greater than zero");
            
            Vector2Int[] points = new Vector2Int[count];

            for (int i = 0; i < count; i++)
            {
                Vector2Int point = _worldBounds.GetRandomPosition();
                
                
            }
            

            return points;
        }
        
        
    }
}