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
        private static readonly int RectSizePixelsID = Shader.PropertyToID("_RectSizePixels");
        
        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock MaterialPropertyBlock;
            public Vector4 Rect;
            public Vector2 RectSizePixels;
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
            if(!_rectInput.IsSelecting)
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

            Vector4 rect = new Vector4(
                min.x / width,
                1f - max.y / height,
                max.x / width,
                1f - min.y / height
            );
            
            passData.RectSizePixels = new Vector2(
                (rect.z - rect.x) * Screen.width,
                (rect.w - rect.y) * Screen.height
            );
            
            passData.Rect = rect;    
            
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture,0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            passData.MaterialPropertyBlock.Clear();
            passData.MaterialPropertyBlock.SetVector(RectID, passData.Rect);
            passData.MaterialPropertyBlock.SetVector(RectSizePixelsID, passData.RectSizePixels);

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