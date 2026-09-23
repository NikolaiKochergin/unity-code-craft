using System;
using Fusion;

namespace Game
{
    public sealed class HealthComponent : NetworkBehaviour
    {
        [Networked]
        public int Current { get; set; } = 5;

        public bool IsAlive => Current > 0;
        public bool IsDead => Current <= 0;

        public void Decrement(int damage)
        {
            Current = Math.Max(0, Current - damage);
        }
    }
}