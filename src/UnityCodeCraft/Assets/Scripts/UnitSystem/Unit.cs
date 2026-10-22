using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SampleGame
{
    [ExecuteAlways]
    public sealed class Unit : MonoBehaviour
    {
        public string Name => this.name;

        public float Scale => _scale;
        public float Height => _height;
        public TeamType TeamType => _teamType;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public Vector3 Position => this.transform.position;
        public Quaternion Rotation => this.transform.rotation;

        [FormerlySerializedAs("_radius")]
        [SerializeField]
        private float _scale = 1;

        [SerializeField]
        private float _height = 1;

        [Header("Health")]
        [SerializeField]
        private int _currentHealth = 5;

        [SerializeField]
        private int _maxHealth = 10;

        [Header("Team")]
        [SerializeField]
        private TeamType _teamType = TeamType.Blue;

        [SerializeField]
        private TeamsConfig _teamConfig;

        [Header("View")]
        [SerializeField]
        private MeshRenderer[] _renderers = Array.Empty<MeshRenderer>();

        [Header("Gizmos")]
        [SerializeField]
        private bool _drawGizmos = true;

        private void Reset()
        {
            _renderers = this.GetComponentsInChildren<MeshRenderer>();
            _teamConfig = Resources.Load<TeamsConfig>(nameof(TeamsConfig));
            this.OnEnable();
        }

        private void OnEnable()
        {
            if (!_teamConfig)
                return;

            Material material = _teamConfig.GetMaterial(_teamType);
            for (int i = 0, count = _renderers.Length; i < count; i++)
                _renderers[i].material = material;
        }

        private void OnValidate()
        {
            this.OnEnable();
        }

        private void OnDrawGizmos()
        {
            if (!_drawGizmos)
                return;

            const float thickness = 2;
            Vector3 center = this.transform.position;

            Color prevColor = Handles.color;
            Handles.color = _teamConfig.GetColor(_teamType);
            Handles.DrawWireDisc(center, Vector3.up, _scale, thickness);
            Handles.DrawLine(center, center + Vector3.up * _height, thickness);
            
            Handles.color = prevColor;
            
            
        }
    }
}