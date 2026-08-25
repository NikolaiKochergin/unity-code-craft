using Unity.Entities;

namespace Game
{
    public partial struct RestoreManaSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((
                         RefRW<Mana> mana, 
                         RefRO<MaxMana> maxMana, 
                         RefRO<RestoreManaPerSecond> restoreManaPerSecond) 
                     in SystemAPI.Query<
                         RefRW<Mana>,
                         RefRO<MaxMana>,
                         RefRO<RestoreManaPerSecond>>())
            {
                if(mana.ValueRW.Value == maxMana.ValueRO.Value)
                    continue;
            }
        }
    }
}