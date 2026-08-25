using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class WarlockAuthoring : MonoBehaviour
    {
        public class WarlockBaker : Baker<WarlockAuthoring>
        {
            public override void Bake(WarlockAuthoring authoring)
            {
                this.Entity()
                    .With<Warlock>()
                    .With<Unit>()
                    .With<Team>()
                    
                    
                    // Heal
                    .WithBuffer<TakeHealRequest>()
                    .WithBuffer<TakeHealEvent>()
                ;
            }
        }
    }
}