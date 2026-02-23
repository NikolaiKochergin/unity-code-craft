using System;

namespace Game
{
    public interface IHealth
    {
        int Max { get; }
        int Current { get; }
        bool IsAlive { get; }
        event Action<int> OnChanged;
        event Action OnDied;
        void TakeDamage(int damage);
        void Restore();
    }
}