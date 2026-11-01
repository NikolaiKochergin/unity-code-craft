using UnityEngine;
using SampleGame;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Material _material;
        
        [SerializeField] private Material _healthMaterial;
        
        private Mesh _quadMesh;
        private UnitSelectionRenderPass _renderPass;
        
        private UnitHealthRenderPass _healthRenderPass;

        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            _renderPass = new UnitSelectionRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingOpaques };
            
            _healthRenderPass = new UnitHealthRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing };
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
            if(_material == null || _quadMesh == null)
                return;
            
            UnitSelectionManager selectionManager = FindAnyObjectByType<UnitSelectionManager>();
            if (selectionManager == null)
                return;
            
            _renderPass.Setup(selectionManager, _material, _quadMesh);
            renderer.EnqueuePass(_renderPass);
            
            if(_healthMaterial == null)
                return;
            
            _healthRenderPass.Setup(selectionManager, _healthMaterial, _quadMesh);
            renderer.EnqueuePass(_healthRenderPass);
        }
    }
}