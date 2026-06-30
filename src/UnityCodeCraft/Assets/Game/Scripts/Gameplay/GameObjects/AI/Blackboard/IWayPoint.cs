using UnityEngine;

namespace Game
{
    public interface IWayPoint
    {
        Vector3 Position { get; }
        float Size { get; }
        bool IsValid { get; }
    }
}