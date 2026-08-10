using Modules.AudioEvents;
using Unity.Entities;

namespace Game
{
   public struct TakeDamageVfx : IComponentData { }
   public struct TakeDamageSfx : IComponentData { public AudioEventKey Value; }
}