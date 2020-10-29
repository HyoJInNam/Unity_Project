using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsGenerator
{
    public static GameObject previewObjects;
    public static List<GameObject> objects = new List<GameObject>();

    public static Transform Instance(Transform parent) {
        if (previewObjects == null) {
            Transform transform = parent.Find("Preview Objects");
            if (transform == null) {
                previewObjects = new GameObject("Preview Objects");
                previewObjects.transform.SetParent(parent);
                previewObjects.transform.localScale = parent.localScale;
                return previewObjects.transform;
            }
            previewObjects = transform.gameObject;
        }
        return previewObjects.transform;
    }
    
    public static Vector3 GetPostitionObject(float[,] heightMap, int skipIncrement, int numVertsPerLine, Vector2 topLeft, float meshWorldSize, int x, int y)
    {
        bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
        bool isMeshEdgeVertex = (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2) && !isOutOfMeshVertex;
        bool isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
        bool isEdgeConnectionVertex = (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

        Vector2 percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
        Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshWorldSize;
        float height = heightMap[x, y];

        if (isEdgeConnectionVertex)
        {
            bool isVertical = x == 2 || x == numVertsPerLine - 3;
            int dstToMainVertexA = ((isVertical) ? y - 2 : x - 2) % skipIncrement;
            int dstToMainVertexB = skipIncrement - dstToMainVertexA;
            float dstPercentFromAToB = dstToMainVertexA / (float)skipIncrement;

            float heightMainVertexA = heightMap[(isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y];
            float heightMainVertexB = heightMap[(isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y];

            height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;
        }
        return new Vector3(vertexPosition2D.x * 2, height * 2, vertexPosition2D.y * 2);
    }
    public static void CreateObjects(float heightMultiplier, float[,] heightMap, MeshSettings meshSettings, int levelOfDetail, ObjectsData settings, Transform terrain)
    {
        if (previewObjects != null){
            GameObject.DestroyImmediate(previewObjects);
        }
        
        int skipIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int numVertsPerLine = meshSettings.numVertsPerLine;

        Vector2 topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

        for (int y = 0; y < numVertsPerLine; y++)
        {
            for (int x = 0; x < numVertsPerLine; x++)
            {
                bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

                if (!isSkippedVertex)
                {
                    Vector3 pos = GetPostitionObject(heightMap, skipIncrement, numVertsPerLine, topLeft, meshSettings.meshWorldSize, x, y);
                    objects.Add(settings.instantiate(heightMultiplier, pos, Instance(terrain)));
                }
            }
        }
    }

    public static void GenerateObjects(float heightMultiplier, float[,] heightMap, MeshSettings meshSettings, int levelOfDetail, ObjectsData settings)
    {
        int skipIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
        int numVertsPerLine = meshSettings.numVertsPerLine;

        Vector2 topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

        int i = 0;
        foreach (GameObject obj in objects)
        {
            obj.name = obj.name;
            Vector3 pos = GetPostitionObject(heightMap, skipIncrement, numVertsPerLine, topLeft, meshSettings.meshWorldSize, i % numVertsPerLine, i / numVertsPerLine);
            obj.transform.position = pos;
            obj.transform.localScale = settings.obj.gameObejct.transform.localScale;
            i++;
        }
    }
}
