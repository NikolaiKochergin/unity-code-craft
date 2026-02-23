using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class CameraShakerController : MonoBehaviour
    {
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private Ship _ship;

        private void OnEnable() => 
            _ship.Health.OnChanged += ShakeCamera;
        
        private void OnDisable() =>
            _ship.Health.OnChanged -= ShakeCamera;

        private void ShakeCamera(int _) => 
            _cameraShaker.Shake();
    }
}