using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class OpponentAIAuthoring : MonoBehaviour
    {
        public class UnitToBuyBaker : Baker<OpponentAIAuthoring>
        {
            public override void Bake(OpponentAIAuthoring authoring) =>
                this.Entity()
                    .WithEnabled<SelectedUnit>(false);
        }
    }
}