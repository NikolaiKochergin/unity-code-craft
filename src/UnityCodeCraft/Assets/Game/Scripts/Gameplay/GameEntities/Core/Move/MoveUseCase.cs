using Atomic.Entities;

namespace Game.Gameplay
{
    public static class MoveUseCase
    {
        public static bool IsMoving(this IGameEntity entity) => 
            entity.GetValue(GameEntityAPI.MoveTime).IsPlaying();
    }
}