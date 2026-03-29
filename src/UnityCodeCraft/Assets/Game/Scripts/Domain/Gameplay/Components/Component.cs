using Game.Gameplay;
using UnityEngine;

namespace SampleGame.Gameplay
{
    public abstract class Component : MonoBehaviour
    {
        public virtual ComponentData AsData() => default;

        public virtual void Restore(ComponentData data){}
    }
}