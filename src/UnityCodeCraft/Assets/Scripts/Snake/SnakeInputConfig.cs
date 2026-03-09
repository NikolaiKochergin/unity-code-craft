using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Create SnakeInputConfig")]
    public class SnakeInputConfig : ScriptableObject
    {
        [field: SerializeField] public KeyCode Up { get; private set; }
        [field: SerializeField] public KeyCode Down { get; private set; }
        [field: SerializeField] public KeyCode Left { get; private set; }
        [field: SerializeField] public KeyCode Right { get; private set; }
    }
}