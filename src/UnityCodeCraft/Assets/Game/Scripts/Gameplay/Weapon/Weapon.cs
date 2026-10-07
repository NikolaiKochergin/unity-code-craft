using Fusion;

namespace Game
{
    public abstract class Weapon : NetworkBehaviour
    {
        public abstract bool CanFire();
        public abstract void Fire();
        public virtual void SetDamage(int value) { }
        public virtual void SetCooldown(float value) { }
    }
}