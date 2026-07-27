using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class ModelEntityAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject _modelEntity;

        public class ModelEntityBaker : Baker<ModelEntityAuthoring>
        {
            public override void Bake(ModelEntityAuthoring authoring)
            {
                this.Entity(TransformUsageFlags.Dynamic)
                    .With(new ModelEntity
                    {
                        Value = GetEntity(authoring._modelEntity, TransformUsageFlags.Dynamic)
                    });
            }
        }
    }
}