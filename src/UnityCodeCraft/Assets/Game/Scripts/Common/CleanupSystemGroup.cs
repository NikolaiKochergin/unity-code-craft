using Unity.Entities;

namespace Game
{
    [UpdateInGroup(typeof(PresentationSystemGroup), OrderLast = true)]
    public partial class CleanupSystemGroup : ComponentSystemGroup
    {
    }
}