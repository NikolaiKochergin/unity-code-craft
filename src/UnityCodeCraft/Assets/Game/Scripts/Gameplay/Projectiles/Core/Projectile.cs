using System.Runtime.InteropServices;
using Fusion;
using UnityEngine;

namespace Game
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Projectile : INetworkStruct
    {
        [FieldOffset(0)] public int StartTick;
        [FieldOffset(4)] private Vector3Compressed _position;
        [FieldOffset(16)] private QuaternionCompressed _rotation;

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public Quaternion Rotation
        {
            get => _rotation;
            set => _rotation = value;
        }
        
        public Vector3 Direction => (Quaternion) _rotation * Vector3.forward;

        public bool IsAlive => StartTick > 0;
    }
}