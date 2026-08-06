using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class CastleReferencesAuthoring : MonoBehaviour
    {
        [SerializeField] private CastleAuthoring _blueCastle;
        [SerializeField] private CastleAuthoring _redCastle;
        
        public class CastleReferencesBaker : Baker<CastleReferencesAuthoring>
        {
            public override void Bake(CastleReferencesAuthoring authoring)
            {
                this.Entity()
                    .With(new CastleReferences
                    {
                        BlueCastle = GetEntity(authoring._blueCastle, TransformUsageFlags.Dynamic),
                        RedCastle = GetEntity(authoring._redCastle, TransformUsageFlags.Dynamic),
                    });
            }
        }
    }
}