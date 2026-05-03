using UnityEngine;

namespace Game
{
    public class TargetComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _target;

        public GameObject Target
        {
            get => _target;
            set => _target = value;
        }
        
        public bool HasTarget => _target != null;
        public Vector3 TargetPosition => HasTarget ? Target.transform.position : Vector3.zero;
    }
}