using System;
using UnityEngine;
using SampleGame;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Material _selectionMaterial;
        [SerializeField] private Material _healthMaterial;
        [SerializeField] private Material _hoverMaterial;
        [SerializeField] private HealthBarRect _healthBarRect;
        [SerializeField] private TeamsConfig _teamsConfig;
        
        private Mesh _quadMesh;
        private UnitRenderPassInstanced _unitRenderPass;
        private UnitHoverRenderPass _hoverRenderPass;
        private UnitSelectionManager _selectionManager;
        private UnitHover _unitHover;

        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            
            _hoverRenderPass = new UnitHoverRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 10};
            _unitRenderPass = new UnitRenderPassInstanced() {  renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 20 };
        }

        protected override void Dispose(bool disposing)
        {
            if (!_quadMesh) 
                return;
            
            CoreUtils.Destroy(_quadMesh);
            _quadMesh = null;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(_quadMesh == null)
                return;
            
            if(!_selectionManager)
                _selectionManager = FindAnyObjectByType<UnitSelectionManager>();
            
            if(!_unitHover)
                _unitHover = FindAnyObjectByType<UnitHover>();

            if (_selectionManager && _selectionMaterial && _healthMaterial && _teamsConfig)
            {
                _unitRenderPass.Setup(_selectionManager, _selectionMaterial, _healthMaterial, _quadMesh, _teamsConfig, _healthBarRect);
                renderer.EnqueuePass(_unitRenderPass);
            }

            if (_unitHover && _hoverMaterial&& _teamsConfig)
            {
                _hoverRenderPass.Setup(_unitHover, _hoverMaterial, _quadMesh, _teamsConfig);
                renderer.EnqueuePass(_hoverRenderPass);
            }
        }
    }
    
    [Serializable]
    public class  HealthBarRect
    {
        public float Width;
        public float Height;
        public float OffsetX;
        public float OffsetY;
    }
}