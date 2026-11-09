using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitHoverRenderPass : ScriptableRenderPass
    {
        private static readonly int SizeID = Shader.PropertyToID("_Size");
        private static readonly int ColorID = Shader.PropertyToID("_BaseColor");
        
        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock MaterialPropertyBlock;
            public float Size;
            public Color Color;
            public Matrix4x4 Matrix;
        }
        
        private readonly MaterialPropertyBlock _materialPropertyBlock = new();
        
        private UnitHover _unitHover;
        private Material _material;
        private Mesh _quadMesh;
        private TeamsConfig _teamsConfig;

        public void Setup(UnitHover unitHover, Material material, Mesh quadMesh, TeamsConfig teamsConfig)
        {
            _teamsConfig = teamsConfig;
            _quadMesh = quadMesh;
            _material = material;
            _unitHover = unitHover;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if(!_unitHover.Target)
                return;

            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass(
                nameof(UnitHoverRenderPass),
                out PassData passData);
            
            Unit unit = _unitHover.Target;
            
            passData.Mesh = _quadMesh;
            passData.Material = _material;
            passData.MaterialPropertyBlock = _materialPropertyBlock;
            
            passData.Matrix = Matrix4x4.TRS(unit.Position, unit.Rotation, Vector3.one);
            passData.Size = unit.Scale * 2;
            passData.Color = _teamsConfig.GetColor(unit.TeamType);
            
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture,0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            passData.MaterialPropertyBlock.Clear();
            passData.MaterialPropertyBlock.SetFloat(SizeID, passData.Size);
            passData.MaterialPropertyBlock.SetColor(ColorID, passData.Color);
            
            RasterCommandBuffer commandBuffer = context.cmd;
            commandBuffer.DrawMesh(
                passData.Mesh,
                passData.Matrix,
                passData.Material,
                submeshIndex: 0,
                shaderPass: 0,
                passData.MaterialPropertyBlock
            );
        }
    }
}