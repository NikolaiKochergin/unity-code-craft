using Modules;
using UnityEngine;
using Zenject;

namespace Game
{
    public class CoinsPool : MonoMemoryPool<Vector2Int, Coin>
    {
        protected override void Reinitialize(Vector2Int position, Coin coin)
        {
            base.Reinitialize(position, coin);
            coin.Generate();
            coin.Position = position;
        }
    }
}