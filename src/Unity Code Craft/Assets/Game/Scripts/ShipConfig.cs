using System;
using UnityEngine;

namespace Game
{
    // +
    [CreateAssetMenu(menuName = "Game/Create " + nameof(ShipConfig))]
    public sealed class ShipConfig : ScriptableObject
    {
        [field: SerializeField] public HealthConfig Health { get; private set; }
        [field: SerializeField] public MoveConfig Move { get; private set; }
        [field: SerializeField] public AttackConfig Attack { get; private set; }
    }

    [Serializable]
    public class HealthConfig
    {
        [field: SerializeField] public int Max { get; private set; } = 5;
    }

    [Serializable]
    public class MoveConfig
    {
        [field: SerializeField] public float Speed { get; private set; } = 5;
    }

    [Serializable]
    public class AttackConfig
    {
        [field: SerializeField] public float FireCooldown { get; private set; } = 0.25f;
        [field: SerializeField] public float BulletSpeed { get; private set; }
        [field: SerializeField] public int BulletDamage { get; private set; }
    }
}