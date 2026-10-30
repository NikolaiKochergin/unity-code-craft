using UnityEngine;
using SampleGame;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Material _material;
        
        private Mesh _quadMesh;
        private UnitSelectionRenderPass _renderPass;
        
        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            _renderPass = 
                new UnitSelectionRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing + 10 };
        }

        protected override void Dispose(bool disposing)
        {
            if (_quadMesh)
            {
                CoreUtils.Destroy(_quadMesh);
                _quadMesh = null;
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if(_material == null || _quadMesh == null)
                return;
            
            UnitSelectionManager selectionManager = FindAnyObjectByType<UnitSelectionManager>();
            if (selectionManager == null || selectionManager.UnitCount <= 0)
                return;
            
            _renderPass.Setup(renderer, _quadMesh);
            renderer.EnqueuePass(_renderPass);
        }
    }
}