using UnityEngine;

namespace Game
{
    public interface IPoint
    {
        Vector3 Position { get; }
        float Size { get; }
        bool IsValid { get; }
    }
}