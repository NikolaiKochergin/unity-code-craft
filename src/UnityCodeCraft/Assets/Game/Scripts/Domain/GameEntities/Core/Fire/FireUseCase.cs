using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public static class FireUseCase
    {
        public static bool IsExpired(in this FireCooldown cooldown) => 
            cooldown.Time <= 0;
        
        public static bool IsPlaying(in this FireCooldown cooldown) =>
            cooldown.Time > 0;

        public static void ResetTime(ref this FireCooldown cooldown) =>
            cooldown.Time = cooldown.Duration;

        public static float3 GetFirePoint(in LocalTransform transform, in FireOffset fireOffset) => 
            transform.Position + math.rotate(transform.Rotation, fireOffset.Value);
        
        public static bool IsExpired(in this FireDelay delay) => 
            delay.Time <= 0;
        
        public static bool IsPlaying(in this FireDelay delay) =>
            delay.Time > 0;

        public static void ResetTime(ref this FireDelay delay) =>
            delay.Time = delay.Duration;
    }
}