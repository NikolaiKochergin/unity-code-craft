using UnityEngine;

namespace SampleGame
{
    public static class GraphicsUtils
    {
        public static bool IsPositionInFrustum(this Camera camera, Vector3 position)
        {
            return camera.WorldToViewportPoint(position) is {z: > 0f, x: >= 0f and <= 1f, y: >= 0f and <= 1f};
        }
        
        public static Mesh CreateQuadMesh()
        {
            Mesh mesh = new()
            {
                name = "Unit Quad",
                vertices = new[]
                {
                    // new Vector3(-0.5f, 0f, -0.5f),
                    // new Vector3( 0.5f, 0f, -0.5f),
                    // new Vector3( 0.5f, 0f,  0.5f),
                    // new Vector3(-0.5f, 0f,  0.5f)
                    
                    new Vector3(-1.0f, 0f, -1.0f),
                    new Vector3( 1.0f, 0f, -1.0f),
                    new Vector3( 1.0f, 0f,  1.0f),
                    new Vector3(-1.0f, 0f,  1.0f)
                },
                uv = new[]
                {
                    new Vector2(0f, 0f),
                    new Vector2(1f, 0f),
                    new Vector2(1f, 1f),
                    new Vector2(0f, 1f)
                },
                triangles = new[]
                {
                    0, 1, 2,
                    0, 2, 3
                }
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
    }
}