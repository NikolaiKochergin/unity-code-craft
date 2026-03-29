using Game.Gameplay;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Health : Component
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;

        public override ComponentData AsData() =>
            new()
            {
                Name = nameof(Health),
                Value = Current.ToString()
            };

        public override void Restore(ComponentData data)
        {
            if(int.TryParse(data.Value, out int value))
                Current = value;
        }
    }
}