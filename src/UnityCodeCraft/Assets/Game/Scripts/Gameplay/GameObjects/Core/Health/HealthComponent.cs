using System;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class HealthComponent : NetworkBehaviour
    {
        public delegate void HealthChangeHandler(int previous, int current);
        
        [Networked, OnChangedRender(nameof(InvokeHealthChanged))]
        public int Current { get; private set; } = 5;
        
        [SerializeField] private int _max = 10;
        public int Max => _max;

        private static PropertyReader<int> _healthReader =
            GetPropertyReader<int>(typeof(HealthComponent), nameof(Current));

        public bool IsAlive => Current > 0;
        public bool IsDead => Current <= 0;

        public event HealthChangeHandler OnHealthChanged;

        public void Decrement(int damage) => 
            Current = Math.Max(0, Current - damage);

        private void InvokeHealthChanged(NetworkBehaviourBuffer previousSnapshot)
        {
            int previousHealth = _healthReader.Read(previousSnapshot);
            OnHealthChanged?.Invoke(previousHealth, Current);
        }
    }
}