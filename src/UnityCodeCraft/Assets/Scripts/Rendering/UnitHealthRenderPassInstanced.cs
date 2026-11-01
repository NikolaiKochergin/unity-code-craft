using System.Collections.Generic;
using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Unit = SampleGame.Unit;

namespace Rendering
{
    public sealed class UnitHealthRenderPassInstanced : ScriptableRenderPass
    {
        private static readonly int FillID = Shader.PropertyToID("_Fill");
        private static readonly int RectID = Shader.PropertyToID("_BarRect");
        
        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock PropertyBlock;
            
            public Matrix4x4[] Matrices;
            public Vector4[] HealthRects;
            public float[] Health;
            public int UnitsCount;
        }
        
        private readonly MaterialPropertyBlock _materialPropertyBlock = new();
        private readonly List<Unit> _culledUnits = new();
        
        private UnitSelectionManager _selectionManager;
        private Material _material;
        private Mesh _quadMesh;
        private HealthBarRect _healthBarRect;

        public void Setup(UnitSelectionManager selectionManager,
            Material material,
            Mesh quadMesh, 
            HealthBarRect healthBarRect)
        {
            _healthBarRect = healthBarRect;
            _quadMesh = quadMesh;
            _material = material;
            _selectionManager = selectionManager;
            
            _material.enableInstancing = true;
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
            passData.PropertyBlock = _materialPropertyBlock;
            
            passData.UnitsCount = _culledUnits.Count;
            passData.Matrices = new Matrix4x4[_culledUnits.Count];
            passData.HealthRects = new Vector4[_culledUnits.Count];
            passData.Health = new float[_culledUnits.Count];

            for (int i = 0; i < _culledUnits.Count; i++)
            {
                Unit unit = _culledUnits[i];
                passData.Matrices[i] = Matrix4x4.TRS(unit.Position, unit.Rotation, Vector3.one);
                passData.HealthRects[i] = new Vector4(
                    _healthBarRect.Width * unit.Scale, 
                    _healthBarRect.Height, 
                    _healthBarRect.OffsetX,
                    _healthBarRect.OffsetY * -unit.Scale);
                passData.Health[i] = (float)unit.CurrentHealth / unit.MaxHealth;
            }

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture,0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            MaterialPropertyBlock propertyBlock = passData.PropertyBlock;
            RasterCommandBuffer commandBuffer = context.cmd;

            propertyBlock.Clear();
            propertyBlock.SetFloatArray(FillID, passData.Health);
            propertyBlock.SetVectorArray(RectID, passData.HealthRects);

            commandBuffer.DrawMeshInstanced(
                passData.Mesh,
                0,
                passData.Material,
                shaderPass: 0,
                passData.Matrices,
                passData.UnitsCount,
                propertyBlock
            );
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