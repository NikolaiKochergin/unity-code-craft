using System;
using System.Collections.Generic;
using SampleGame;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Rendering
{
    public sealed class UnitRenderPassInstanced : ScriptableRenderPass
    {
        private const int MaxInstances = 1023;

        private static readonly int SizeID = Shader.PropertyToID("_Size");
        private static readonly int ColorID = Shader.PropertyToID("_BaseColor");
        private static readonly int FillID = Shader.PropertyToID("_Fill");
        private static readonly int RectID = Shader.PropertyToID("_BarRect");

        private sealed class PassData
        {
            public Mesh Mesh;

            public Material SelectionMaterial;
            public Material HealthMaterial;

            public MaterialPropertyBlock MaterialPropertyBlock;

            public Matrix4x4[] Matrices;
            public Vector4[] Colors;
            public float[] Sizes;

            public float[] Health;
            public Vector4[] HealthRects;

            public Matrix4x4[] BatchMatrices;
            public Vector4[] BatchColors;
            public float[] BatchSizes;

            public float[] BatchHealth;
            public Vector4[] BatchHealthRects;

            public int UnitsCount;
        }

        private readonly MaterialPropertyBlock _materialPropertyBlock = new();

        private readonly List<Unit> _culledUnits = new();

        private Matrix4x4[] _matrices = Array.Empty<Matrix4x4>();
        private Vector4[] _colors = Array.Empty<Vector4>();
        private float[] _sizes = Array.Empty<float>();

        private float[] _health = Array.Empty<float>();
        private Vector4[] _healthRects = Array.Empty<Vector4>();

        private readonly Matrix4x4[] _batchMatrices = new Matrix4x4[MaxInstances];
        private readonly Vector4[] _batchColors = new Vector4[MaxInstances];
        private readonly float[] _batchSizes = new float[MaxInstances];

        private readonly float[] _batchHealth = new float[MaxInstances];
        private readonly Vector4[] _batchHealthRects = new Vector4[MaxInstances];

        private UnitSelectionManager _selectionManager;
        private TeamsConfig _teamsConfig;

        private Material _selectionMaterial;
        private Material _healthMaterial;

        private Mesh _quadMesh;
        private HealthBarRect _healthBarRect;

        public void Setup(
            UnitSelectionManager selectionManager,
            Material selectionMaterial,
            Material healthMaterial,
            Mesh quadMesh,
            TeamsConfig teamsConfig,
            HealthBarRect healthBarRect)
        {
            _selectionManager = selectionManager;
            _selectionMaterial = selectionMaterial;
            _healthMaterial = healthMaterial;
            _quadMesh = quadMesh;
            _teamsConfig = teamsConfig;
            _healthBarRect = healthBarRect;

            if (_selectionMaterial)
                _selectionMaterial.enableInstancing = true;

            if (_healthMaterial)
                _healthMaterial.enableInstancing = true;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            if (!_selectionManager || !_quadMesh)
                return;

            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            Camera camera = cameraData.camera;
            CullUnits(camera);

            int count = _culledUnits.Count;
            if (count == 0)
                return;

            EnsureCapacity(count);
            FillInstanceData(count);

            using IRasterRenderGraphBuilder builder = renderGraph.AddRasterRenderPass(
                    nameof(UnitRenderPassInstanced),
                    out PassData passData);

            passData.Mesh = _quadMesh;
            passData.SelectionMaterial = _selectionMaterial;
            passData.HealthMaterial = _healthMaterial;

            passData.MaterialPropertyBlock = _materialPropertyBlock;

            passData.Matrices = _matrices;
            passData.Colors = _colors;
            passData.Sizes = _sizes;

            passData.Health = _health;
            passData.HealthRects = _healthRects;

            passData.BatchMatrices = _batchMatrices;
            passData.BatchColors = _batchColors;
            passData.BatchSizes = _batchSizes;

            passData.BatchHealth = _batchHealth;
            passData.BatchHealthRects = _batchHealthRects;

            passData.UnitsCount = count;

            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            builder.SetRenderAttachment(resourceData.activeColorTexture, 0);

            builder.SetRenderFunc(static (PassData data, RasterGraphContext context) => ExecutePass(data, context));
        }

        private void EnsureCapacity(int count)
        {
            if (_matrices.Length >= count)
                return;

            int capacity = Mathf.NextPowerOfTwo(count);

            Array.Resize(ref _matrices, capacity);
            Array.Resize(ref _colors, capacity);
            Array.Resize(ref _sizes, capacity);

            Array.Resize(ref _health, capacity);
            Array.Resize(ref _healthRects, capacity);
        }

        private void FillInstanceData(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Unit unit = _culledUnits[i];

                _matrices[i] = Matrix4x4.TRS(
                    unit.Position,
                    unit.Rotation,
                    Vector3.one);

                _colors[i] = _teamsConfig.GetColor(unit.TeamType);
                _sizes[i] = unit.Scale * 2f;

                _health[i] = unit.MaxHealth > 0
                    ? Mathf.Clamp01((float)unit.CurrentHealth / unit.MaxHealth)
                    : 0f;

                _healthRects[i] = new Vector4(
                    _healthBarRect.Width * unit.Scale,
                    _healthBarRect.Height,
                    _healthBarRect.OffsetX,
                    _healthBarRect.OffsetY * -unit.Scale);
            }
        }

        private static void ExecutePass(PassData passData, RasterGraphContext context)
        {
            MaterialPropertyBlock propertyBlock = passData.MaterialPropertyBlock;

            int totalCount = passData.UnitsCount;

            for (int start = 0; start < totalCount; start += MaxInstances)
            {
                int count = Mathf.Min(MaxInstances, totalCount - start);

                Array.Copy(
                    passData.Matrices,
                    start,
                    passData.BatchMatrices,
                    0,
                    count);

                Array.Copy(
                    passData.Colors,
                    start,
                    passData.BatchColors,
                    0,
                    count);

                Array.Copy(
                    passData.Sizes,
                    start,
                    passData.BatchSizes,
                    0,
                    count);
                
                Array.Copy(
                    passData.Health,
                    start,
                    passData.BatchHealth,
                    0,
                    count);

                Array.Copy(
                    passData.HealthRects,
                    start,
                    passData.BatchHealthRects,
                    0,
                    count);

                propertyBlock.Clear();
                propertyBlock.SetFloatArray(SizeID, passData.BatchSizes);
                propertyBlock.SetVectorArray(ColorID, passData.BatchColors);
                
                context.cmd.DrawMeshInstanced(
                    passData.Mesh,
                    0,
                    passData.SelectionMaterial,
                    0,
                    passData.BatchMatrices,
                    count,
                    propertyBlock);
                
                propertyBlock.Clear();
                propertyBlock.SetFloatArray(FillID, passData.BatchHealth);
                propertyBlock.SetVectorArray(RectID, passData.BatchHealthRects);

                context.cmd.DrawMeshInstanced(
                    passData.Mesh,
                    0,
                    passData.HealthMaterial,
                    0,
                    passData.BatchMatrices,
                    count,
                    propertyBlock);
            }
        }

        private void CullUnits(Camera camera)
        {
            _culledUnits.Clear();
            foreach (Unit unit in _selectionManager)
                if (camera.IsPositionInFrustum(unit.Position))
                    _culledUnits.Add(unit);
        }
    }
}