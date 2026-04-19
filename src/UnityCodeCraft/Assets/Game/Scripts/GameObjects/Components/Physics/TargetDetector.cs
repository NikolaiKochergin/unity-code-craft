using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class TargetDetector : MonoBehaviour
    {
        public abstract IReadOnlyList<Transform> GetTargets();
    }
}