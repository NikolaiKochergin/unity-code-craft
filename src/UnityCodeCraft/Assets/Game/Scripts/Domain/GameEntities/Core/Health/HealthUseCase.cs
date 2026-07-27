using Unity.Mathematics;

namespace Game
{
    public static class HealthUseCase
    {
        public static bool IsAlive(in this CurrentHealth health) => 
            health.Value > 0;

        public static bool IsDead(in this CurrentHealth health) => 
            health.Value <= 0;

        public static void Reduce(ref this CurrentHealth health, int damage) => 
            health.Value = math.max(0, health.Value - math.max(0, damage));
    }
}