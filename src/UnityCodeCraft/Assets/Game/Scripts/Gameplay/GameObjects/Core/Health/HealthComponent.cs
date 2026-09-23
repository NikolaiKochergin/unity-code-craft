using System;
using Fusion;

namespace Game
{
    public sealed class HealthComponent : NetworkBehaviour
    {
        [Networked, OnChangedRender(nameof(InvokeHealthChanged))]
        public int Current { get; private set; } = 5;

        public bool IsAlive => Current > 0;
        public bool IsDead => Current <= 0;

        public event Action OnHealthChanged;

        public void Decrement(int damage) => 
            Current = Math.Max(0, Current - damage);

        private void InvokeHealthChanged() => 
            OnHealthChanged?.Invoke();
    }
}