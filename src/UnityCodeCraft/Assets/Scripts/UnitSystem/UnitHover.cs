using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame
{
    public sealed class UnitHover : MonoBehaviour
    {
        public event Action<Unit> OnTargetChanged;

        public Unit Target
        {
            get { return _target; }
            set
            {
                if (_target != value)
                {
                    _target = value;
                    this.OnTargetChanged?.Invoke(value);
                }
            }
        }

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private MouseInput _mouseInput;

        [ShowInInspector, ReadOnly]
        private Unit _target;

        private void Update()
        {
            Vector2 position = _mouseInput.Position;
            Ray ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit)) 
                hit.transform.TryGetComponent(out _target);
        }
    }
}