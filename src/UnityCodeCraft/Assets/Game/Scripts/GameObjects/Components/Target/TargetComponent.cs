using UnityEngine;

namespace Game
{
    public sealed class TargetComponent : MonoBehaviour
    {
        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }

        [SerializeField]
        private GameObject _target;
    }
}