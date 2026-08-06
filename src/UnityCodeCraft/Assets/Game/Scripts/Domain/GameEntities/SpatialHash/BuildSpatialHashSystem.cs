using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public partial struct BuildSpatialHashSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            SpatialGrid.Map = new NativeParallelMultiHashMap<int, Entity>(4096, Allocator.Persistent);
            SpatialGrid.CellSize = 3f;
        }

        public void OnDestroy(ref SystemState state) => 
            SpatialGrid.Map.Dispose();

        public void OnUpdate(ref SystemState state)
        {
            SpatialGrid.Map.Clear();

            foreach ((
                         RefRO<LocalTransform> transform, 
                         Entity entity) 
                     in SystemAPI.Query<
                         RefRO<LocalTransform>>()
                         .WithEntityAccess())
            {
                int2 cell = SpatialHash.GetCell(transform.ValueRO.Position, SpatialGrid.CellSize);
                SpatialGrid.Map.Add(SpatialHash.Hash(cell), entity);
            }
        }
    }

    public static class SpatialGrid
    {
        public static NativeParallelMultiHashMap<int, Entity> Map;
        public static float CellSize;
    }
}