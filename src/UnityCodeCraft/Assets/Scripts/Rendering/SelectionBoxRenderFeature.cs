using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class SelectionBoxRenderFeature : ScriptableRendererFeature
    {
        [SerializeField] private Material _material;
        
        private Mesh _quadMesh;
        private SelectionBoxRenderPass _renderPass;
        
        public override void Create()
        {
            _quadMesh = GraphicsUtils.CreateQuadMesh();
            _renderPass = new SelectionBoxRenderPass { renderPassEvent = RenderPassEvent.AfterRenderingOpaques };
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
            
            RectInput rectInput = FindAnyObjectByType<RectInput>();
            if (rectInput == null)
                return;
            
            _renderPass.Setup(rectInput, _material, _quadMesh);
            renderer.EnqueuePass(_renderPass);
        }
    }
}