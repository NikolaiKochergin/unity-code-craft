using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    [BurstCompile]
    public static class SpatialHash
    {
        public static int2 GetCell(float3 position, float cellSize) => 
            (int2)math.floor(position.xz / cellSize);

        public static int Hash(int2 cell) => 
            (cell.x * 73856093) ^ (cell.y * 19349663);

        public static Entity FindClosest<TPredicate>(
            float3 position,
            float radius,
            in TPredicate predicate,
            in ComponentLookup<LocalTransform> transformLookup)
        where TPredicate : unmanaged, IEntityPredicate
        {
            Entity closest = Entity.Null;
            float closestDistanceSqr = radius * radius;

            int2 centerCell = GetCell(position, SpatialGrid.CellSize);
            int cellRadius = (int)math.ceil(radius / SpatialGrid.CellSize);
            
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                for (int x = -cellRadius; x <= cellRadius; x++)
                {
                    int2 cell = centerCell + new int2(x, y);

                    if (!SpatialGrid.Map.TryGetFirstValue(Hash(cell), out Entity entity, out var iterator))
                        continue;

                    do
                    {
                        if (!predicate.Invoke(entity))
                            continue;

                        float distanceSq = math.distancesq(
                            position,
                            transformLookup[entity].Position);

                        if (distanceSq < closestDistanceSqr)
                        {
                            closestDistanceSqr = distanceSq;
                            closest = entity;
                        }

                    } while (SpatialGrid.Map.TryGetNextValue(out entity, ref iterator));
                }
            }

            return closest;
        }
    }
}