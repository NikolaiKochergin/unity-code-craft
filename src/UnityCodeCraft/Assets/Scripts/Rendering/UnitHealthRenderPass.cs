using System.Collections.Generic;
using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitHealthRenderPass : ScriptableRenderPass
    {
        private static readonly int FillID = Shader.PropertyToID("_Fill");
        private static readonly int RectID = Shader.PropertyToID("_BarRect");

        private struct UnitSnapshot
        {
            public Vector3 Position;
            public float Scale;
            public float Health;
        }
        
        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock MaterialPropertyBlock;
            public List<UnitSnapshot> Units;
        }
        
        private readonly MaterialPropertyBlock _materialPropertyBlock = new();
        private readonly List<Unit> _culledUnits = new();
        
        private UnitSelectionManager _selectionManager;
        private Material _material;
        private Mesh _quadMesh;
        
        public void Setup(
            UnitSelectionManager selectionManager,
            Material material, 
            Mesh quadMesh)
        {
            _quadMesh = quadMesh;
            _material = material;
            _selectionManager = selectionManager;
        }
        
        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
            Camera camera = cameraData.camera;
            CullUnits(camera);
            
            if(_culledUnits.Count <= 0) 
                return;

            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass(
                nameof(UnitHealthRenderPass),
                out PassData passData);
            
            passData.Mesh = _quadMesh;
            passData.Material = _material;
            passData.MaterialPropertyBlock = _materialPropertyBlock;
            CopyToSnapshots(passData);

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture,0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            foreach (UnitSnapshot unit in passData.Units)
            {
                Matrix4x4 matrix = Matrix4x4.TRS(unit.Position, Quaternion.identity, Vector3.one);
                passData.MaterialPropertyBlock.Clear();
                passData.MaterialPropertyBlock.SetFloat(FillID, unit.Health);
                passData.MaterialPropertyBlock.SetVector(RectID, new Vector4(100 * unit.Scale, 8, 0, -50 * unit.Scale));

                RasterCommandBuffer commandBuffer = context.cmd;
                commandBuffer.DrawMesh(
                    passData.Mesh,
                    matrix,
                    passData.Material,
                    submeshIndex: 0,
                    shaderPass: 0,
                    passData.MaterialPropertyBlock
                );
            }
        }

        private void CopyToSnapshots(PassData passData)
        {
            if(passData.Units == null)
                passData.Units = new List<UnitSnapshot>();
            else
                passData.Units.Clear();

            foreach (Unit unit in _culledUnits)
            {
                passData.Units.Add(new UnitSnapshot
                {
                    Position = unit.Position,
                    Scale = unit.Scale,
                    Health = (float)unit.CurrentHealth / unit.MaxHealth,
                });
            }
        }
        
        private void CullUnits(Camera camera)
        {
            _culledUnits.Clear();

            foreach (Unit unit in _selectionManager)
            {
                if(unit.Scale <= 0 || !camera.IsPositionInFrustum(unit.Position))
                    continue;
                
                _culledUnits.Add(unit);
            }
        }
    }
}