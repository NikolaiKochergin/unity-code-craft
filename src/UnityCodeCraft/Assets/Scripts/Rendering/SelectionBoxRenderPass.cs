using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class SelectionBoxRenderPass : ScriptableRenderPass
    {
        private static readonly int RectID = Shader.PropertyToID("_Rect");
        
        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock MaterialPropertyBlock;
            public Vector4 Rect;
        }
        
        private readonly MaterialPropertyBlock _materialPropertyBlock = new();
        
        private RectInput _rectInput;
        private Material _material;
        private Mesh _quadMesh;
        
        public void Setup(RectInput rectInput, Material material, Mesh quadMesh)
        {
            _quadMesh = quadMesh;
            _material = material;
            _rectInput = rectInput;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if(!_rectInput.IsDragging)
                return;

            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass(
                nameof(SelectionBoxRenderPass),
                out PassData passData);
            
            passData.Mesh = _quadMesh;
            passData.Material = _material;
            passData.MaterialPropertyBlock = _materialPropertyBlock;
            
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            int width = cameraData.cameraTargetDescriptor.width;
            int height = cameraData.cameraTargetDescriptor.height;
          
            Vector2 min = Vector2.Min(
                _rectInput.StartPoint,
                _rectInput.EndPoint
            );

            Vector2 max = Vector2.Max(
                _rectInput.StartPoint,
                _rectInput.EndPoint
            );

            passData.Rect = new Vector4(
                min.x / width,
                1f - max.y / height,
                max.x / width,
                1f - min.y / height
            );
            
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture,0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            passData.MaterialPropertyBlock.Clear();
            passData.MaterialPropertyBlock.SetVector(RectID, passData.Rect);

            RasterCommandBuffer commandBuffer = context.cmd;
            commandBuffer.DrawMesh(
                passData.Mesh,
                Matrix4x4.identity,
                passData.Material,
                submeshIndex: 0,
                shaderPass: 0,
                passData.MaterialPropertyBlock
            );
        }
    }
}