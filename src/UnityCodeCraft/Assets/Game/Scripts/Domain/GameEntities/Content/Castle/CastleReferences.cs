using Unity.Entities;

namespace Game
{
    public struct CastleReferences : IComponentData
    {
        public Entity BlueCastle;
        public Entity RedCastle;
    }
}