using System;

namespace Game
{
    public interface IAttack
    {
        event Action<AttackEvent> OnFire;
        void Fire();
    }
}