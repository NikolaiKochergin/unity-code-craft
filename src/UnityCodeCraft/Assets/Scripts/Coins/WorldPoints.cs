using System;
using System.Collections.Generic;
using SnakeGame;
using UnityEngine;

namespace Game
{
    public class WorldPoints
    {
        private readonly IWorldBounds _worldBounds;

        public WorldPoints(IWorldBounds worldBounds) => 
            _worldBounds = worldBounds;

        public List<Vector2Int> GetRandomPoints(int count)
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

            return points;
        }
    }
}