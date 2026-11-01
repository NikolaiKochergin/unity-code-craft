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
        [SerializeField] private TeamsConfig _teamsConfig;
        
        private Mesh _quadMesh;
        private UnitSelectionRenderPassInstanced _selectionRenderPass;
        
        private UnitHealthRenderPass _healthRenderPass;

        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            _selectionRenderPass = new UnitSelectionRenderPassInstanced { renderPassEvent = RenderPassEvent.AfterRenderingOpaques };
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
            if(_selectionMaterial == null || _quadMesh == null)
                return;
            
            UnitSelectionManager selectionManager = FindAnyObjectByType<UnitSelectionManager>();
            if (selectionManager == null)
                return;
            
            _selectionRenderPass.Setup(selectionManager, _selectionMaterial, _quadMesh, _teamsConfig);
            renderer.EnqueuePass(_selectionRenderPass);
            
            if(_healthMaterial == null)
                return;
            
            _healthRenderPass.Setup(selectionManager, _healthMaterial, _quadMesh);
            renderer.EnqueuePass(_healthRenderPass);
        }
    }
}