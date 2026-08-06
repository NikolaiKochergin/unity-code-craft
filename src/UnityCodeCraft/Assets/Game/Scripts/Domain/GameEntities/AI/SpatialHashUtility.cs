using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Game
{
    public static class SpatialHashUtility
    {
        public static int2 GetCell(float3 position, float cellSize) => 
            (int2)math.floor(position.xz / cellSize);

        public static int Hash(int2 cell) => 
            (cell.x * 73856093) ^ (cell.y * 19349663);

        public static Entity FindClosest<TPredicate>(
            NativeParallelMultiHashMap<int, Entity> gridMap,
            float3 position,
            float radius,
            float cellSize,
            in TPredicate predicate,
            in ComponentLookup<LocalTransform> transformLookup)
        where TPredicate : unmanaged, IEntityPredicate
        {
            Entity closest = Entity.Null;
            float closestDistanceSqr = radius * radius;

            int2 centerCell = GetCell(position, cellSize);
            int cellRadius = (int)math.ceil(radius / cellSize);
            
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                for (int x = -cellRadius; x <= cellRadius; x++)
                {
                    int2 cell = centerCell + new int2(x, y);

                    if (!gridMap.TryGetFirstValue(Hash(cell), out Entity entity, out var iterator))
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

                    } while (gridMap.TryGetNextValue(out entity, ref iterator));
                }
            }

            return closest;
        }
    }
}