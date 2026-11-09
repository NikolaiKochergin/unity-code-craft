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
        private UnitSelectionRenderPassInstanced _selectionRenderPass;
        private UnitHealthRenderPassInstanced _healthRenderPass;
        private UnitHoverRenderPass _hoverRenderPass;
        private UnitSelectionManager _selectionManager;
        private UnitHover _unitHover;

        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            
            _hoverRenderPass = new UnitHoverRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 10};
            _selectionRenderPass = new UnitSelectionRenderPassInstanced { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 20 };
            _healthRenderPass = new UnitHealthRenderPassInstanced { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 30 };
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
            
            if (_selectionManager && _selectionMaterial && _teamsConfig)
            {
                _selectionRenderPass.Setup(_selectionManager, _selectionMaterial, _quadMesh, _teamsConfig);
                renderer.EnqueuePass(_selectionRenderPass);
            }

            if (_selectionManager && _healthMaterial)
            {
                _healthRenderPass.Setup(_selectionManager, _healthMaterial, _quadMesh, _healthBarRect);
                renderer.EnqueuePass(_healthRenderPass);
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