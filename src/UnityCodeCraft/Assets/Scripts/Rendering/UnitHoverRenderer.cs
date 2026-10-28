using SampleGame;
using UnityEngine;

namespace Rendering
{
    public sealed class UnitHoverRenderer : MonoBehaviour
    {
        private static readonly int Size = Shader.PropertyToID("_Size");
        
        [SerializeField] private UnitHover _unitHover;
        [SerializeField] private Material _material;

        private MaterialPropertyBlock _materialPropertyBlock;
        private Camera _camera;
        private Mesh _quadMesh;

        private void Awake()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _camera = Camera.main;
            _quadMesh = GraphicsUtils.CreateQuadMesh();
        }

        private void LateUpdate()
        {
            if(!_unitHover.Target)
                return;
            
            Unit unit = _unitHover.Target;
            
            Matrix4x4 matrix = Matrix4x4.TRS(unit.Position, unit.Rotation, Vector3.one);
            
            _materialPropertyBlock.Clear();
            _materialPropertyBlock.SetFloat(Size, unit.Scale * 2);
            
            Graphics.DrawMesh(
                _quadMesh,
                matrix,
                _material,
                gameObject.layer,
                _camera,
                submeshIndex: 0,
                _materialPropertyBlock
            );
        }
    }
}