using System.Collections.Generic;
using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitSelectionRenderPassInstanced : ScriptableRenderPass
    {
        private static readonly int SizeID = Shader.PropertyToID("_Size");
        private static readonly int ColorID = Shader.PropertyToID("_BaseColor");

        private sealed class PassData
        {
            public Mesh Mesh;
            public Material Material;
            public MaterialPropertyBlock PropertyBlock;

            public Matrix4x4[] Matrices;
            public Vector4[] Colors;
            public float[] Sizes;
            public int UnitsCount;
        }

        private readonly MaterialPropertyBlock _materialPropertyBlock = new();
        private readonly List<Unit> _culledUnits = new();

        private UnitSelectionManager _selectionManager;
        private Material _material;
        private Mesh _quadMesh;
        private TeamsConfig _teamsConfig;

        public void Setup(
            UnitSelectionManager selectionManager,
            Material material,
            Mesh quadMesh,
            TeamsConfig teamsConfig)
        {
            _teamsConfig = teamsConfig;
            _quadMesh = quadMesh;
            _material = material;
            _selectionManager = selectionManager;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
            Camera camera = cameraData.camera;
            CullUnits(camera);

            if (_culledUnits.Count == 0)
                return;

            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass(
                    nameof(UnitSelectionRenderPassInstanced),
                    out PassData passData);

            passData.Mesh = _quadMesh;
            passData.Material = _material;
            passData.PropertyBlock = _materialPropertyBlock;
            
            passData.UnitsCount = _culledUnits.Count;
            passData.Matrices = new Matrix4x4[_culledUnits.Count];
            passData.Colors = new Vector4[_culledUnits.Count];
            passData.Sizes = new float[_culledUnits.Count];

            for (int i = 0; i < _culledUnits.Count; i++)
            {
                Unit unit = _culledUnits[i];
                passData.Matrices[i] = Matrix4x4.TRS(unit.Position, unit.Rotation, Vector3.one);
                passData.Colors[i] = _teamsConfig.GetColor(unit.TeamType);
                passData.Sizes[i] = unit.Scale * 2;
            }

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            TextureHandle colorTexture = resourceData.activeColorTexture;
            builder.SetRenderAttachment(colorTexture, 0);
            
            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            MaterialPropertyBlock propertyBlock = passData.PropertyBlock;
            RasterCommandBuffer commandBuffer = context.cmd;

            propertyBlock.Clear();
            propertyBlock.SetFloatArray(SizeID, passData.Sizes);
            propertyBlock.SetVectorArray(ColorID, passData.Colors);

            commandBuffer.DrawMeshInstanced(
                passData.Mesh,
                0,
                passData.Material,
                0,
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
                if (unit.Scale <= 0 || !camera.IsPositionInFrustum(unit.Position))
                    continue;

                _culledUnits.Add(unit);
            }
        }
    }
}