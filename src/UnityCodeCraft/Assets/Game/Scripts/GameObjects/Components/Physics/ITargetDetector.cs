using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ITargetDetector
    {
        Transform Origin { get; }
        IReadOnlyList<Transform> GetTargets();
    }
}