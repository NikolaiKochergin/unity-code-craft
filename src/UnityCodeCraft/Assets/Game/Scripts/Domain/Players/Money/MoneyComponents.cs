using Unity.Entities;

namespace Game
{
    public struct Money : IComponentData { public int Value; }
    public struct MoneyIncome : IComponentData { public int Value; }
    public struct IncomeTickCooldown : IComponentData, IEnableableComponent { public float Time, Duration; }
}