using UnityEngine;

public static class RedguardSceneTransform
{
    public const float SceneScale = 1.0f / 5120.0f;
    public const float SceneOffsetScale = 256.0f * SceneScale;

    public static Vector3 ConvertMpobPosition(int posX, int posY, int posZ)
        => new Vector3(
            posX * SceneOffsetScale,
            -posY * SceneOffsetScale,
            -((0xFFFFFF - (posZ * 256.0f)) * SceneScale));

    public static Vector3 ConvertMarkerPosition(int posX, int posY, int posZ)
        => new Vector3(
            posX * SceneScale,
            -posY * SceneScale,
            -((0xFFFFFF - posZ) * SceneScale));

    public static Vector3 ConvertSceneOffset(int offsetX, int offsetY, int offsetZ)
        => new Vector3(
            offsetX * SceneOffsetScale,
            -offsetY * SceneOffsetScale,
            -offsetZ * SceneOffsetScale);

    public static Vector3 ReflectSceneLightPosition(Vector3 position)
        => new Vector3(-position.x, position.y, position.z);

    public static Vector3 ReflectVertex(Vector3 vertex)
        => new Vector3(-vertex.x, vertex.y, vertex.z);

    public static Vector3 ReflectNormal(Vector3 normal)
        => new Vector3(-normal.x, normal.y, normal.z);

    public static Matrix4x4 ReflectPlacementMatrix(Matrix4x4 matrix)
    {
        // M' = S * M * S, where S = diag(-1, 1, 1)
        matrix.m10 = -matrix.m10;
        matrix.m20 = -matrix.m20;
        matrix.m01 = -matrix.m01;
        matrix.m02 = -matrix.m02;
        matrix.m03 = -matrix.m03;
        return matrix;
    }
}
