using Game.Gameplay;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Health : MonoBehaviour, IComponentSerializer<int>
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;

        public string Key => nameof(Health);
        
        public int Serialize() => Current;

        public void Deserialize(int data) => Current = data;
    }
}