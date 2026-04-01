using Game.Gameplay;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Countdown : MonoBehaviour, IComponentSerializer<float>
    {
        ///Variable
        [field: SerializeField]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }

        public string Key => nameof(Countdown);
        
        public float Serialize() => Current;

        public void Deserialize(float data) => Current = data;
    }
}