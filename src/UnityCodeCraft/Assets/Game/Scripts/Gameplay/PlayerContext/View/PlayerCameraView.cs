using UnityEngine;

namespace Game
{
    public sealed class PlayerCameraView : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        
        public void SetPosition(Vector3 position)
        {
            _pivot.position = position;
        }
    }
}