using Unity.Entities;

namespace Game
{
    public interface IEntityPredicate
    {
        bool Invoke(Entity entity);
    }
}