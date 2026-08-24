using Unity.Collections;
using Unity.Entities;

namespace Game
{
    public struct SelectedUnit : IComponentData, IEnableableComponent { public FixedString32Bytes Name; }
}