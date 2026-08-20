using Unity.Entities;

namespace Game
{
    public struct Ammo : IComponentData { public int Value; }
    public struct MaxAmmo : IComponentData { public int Value; }
    public struct RestoreAmmoCooldown : IComponentData, IEnableableComponent { public float Time, Duration; }
}