using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureDataManager
{
    static float textureSize = 0.125f;
    static float smallValue = 0.001f;
    static float uVSmallValue = 0.005f;

    public static Dictionary<BlockEnums, TextureDataClass> BlockTextureDataDict = new Dictionary<BlockEnums, TextureDataClass>();

    public static void InitBlockTextures()
    {
        // up down front back right left
        BlockTextureDataDict.Add(BlockEnums.Air, new TextureDataClass(BlockEnums.Air, false,
        new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0), new Vector2Int(0, 0)));

        BlockTextureDataDict.Add(BlockEnums.Stone, new TextureDataClass(BlockEnums.Stone, true,
        new Vector2Int(3, 7), new Vector2Int(3, 7), new Vector2Int(3, 7), new Vector2Int(3, 7), new Vector2Int(3, 7), new Vector2Int(3, 7)));

        BlockTextureDataDict.Add(BlockEnums.Grass, new TextureDataClass(BlockEnums.Grass, true,
        new Vector2Int(0, 7), new Vector2Int(2, 7), new Vector2Int(1, 7), new Vector2Int(1, 7), new Vector2Int(1, 7), new Vector2Int(1, 7)));

        BlockTextureDataDict.Add(BlockEnums.Dirt, new TextureDataClass(BlockEnums.Dirt, true,
        new Vector2Int(2, 7), new Vector2Int(2, 7), new Vector2Int(2, 7), new Vector2Int(2, 7), new Vector2Int(2, 7), new Vector2Int(2, 7)));

        BlockTextureDataDict.Add(BlockEnums.Log, new TextureDataClass(BlockEnums.Log, true,
        new Vector2Int(4, 7), new Vector2Int(4, 7), new Vector2Int(4, 7), new Vector2Int(4, 7), new Vector2Int(4, 7), new Vector2Int(4, 7)));

        BlockTextureDataDict.Add(BlockEnums.Leaf, new TextureDataClass(BlockEnums.Leaf, false,
        new Vector2Int(5, 7), new Vector2Int(5, 7), new Vector2Int(5, 7), new Vector2Int(5, 7), new Vector2Int(5, 7), new Vector2Int(5, 7)));
    }

    public static MeshClass AddBlockFaceData(Vector3Int direction, MeshClass meshClass, BlockEnums blockType, int x, int y, int z)
    {
        AddFaceVertices(meshClass, direction, x, y, z);
        meshClass.AddQuadTriangles();
        Vector2[] UVs = FaceUvs(direction, blockType);
        for (int i = 0; i < UVs.Length; i++) {
            meshClass.uv.Add(UVs[i]);
        }

        return meshClass;
    }

    private static Vector2[] FaceUvs(Vector3Int direction, BlockEnums blockType)
    {
        Vector2[] UVs = new Vector2[4];

        Vector2Int texturePos = BlockTextureDataDict[blockType].FindTexturePosition(direction);

        UVs[0] = new Vector2(texturePos.x * textureSize + uVSmallValue,
                             texturePos.y * textureSize + uVSmallValue);

        UVs[1] = new Vector2(texturePos.x * textureSize + uVSmallValue,
                             texturePos.y * textureSize + textureSize - uVSmallValue);

        UVs[2] = new Vector2(texturePos.x * textureSize + textureSize - uVSmallValue,
                             texturePos.y * textureSize + textureSize - uVSmallValue);

        UVs[3] = new Vector2(texturePos.x * textureSize + textureSize - uVSmallValue,
                             texturePos.y * textureSize + uVSmallValue);


        /*UVs[0] = new Vector2(0, 0);
        UVs[1] = new Vector2(0, 1);
        UVs[2] = new Vector2(1, 0);
        UVs[3] = new Vector2(1, 1);*/

        return UVs;
    }

    private static void AddFaceVertices(MeshClass meshClass, Vector3Int direction, int x, int y, int z)
    {
        /*
        else if (direction == Vector3Int.forward) {
            meshClass.AddVertex(new Vector3(x, y, z));
            meshClass.AddVertex(new Vector3(x, y, z));
            meshClass.AddVertex(new Vector3(x, y, z));
            meshClass.AddVertex(new Vector3(x, y, z));
        }
        */
        // starts in negative xyz coordinates
        if (direction == Vector3Int.up) {
            meshClass.AddVertex(new Vector3(x - smallValue, y + 1f, z - smallValue));
            meshClass.AddVertex(new Vector3(x - smallValue, y + 1f, z + 1f + smallValue));
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y + 1f, z + 1f + smallValue));
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y + 1f, z - smallValue));
        }
        else if (direction == Vector3Int.forward) {
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y - smallValue, z + 1f));            
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y + 1f + smallValue, z + 1f));
            meshClass.AddVertex(new Vector3(x - smallValue, y + 1f + smallValue, z + 1f));
            meshClass.AddVertex(new Vector3(x - smallValue, y - smallValue, z + 1f));
        }
        else if (direction == Vector3Int.back) {
            meshClass.AddVertex(new Vector3(x - smallValue, y - smallValue, z));
            meshClass.AddVertex(new Vector3(x - smallValue, y + 1f + smallValue, z));
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y + 1f + smallValue, z));
            meshClass.AddVertex(new Vector3(x + 1f + smallValue, y - smallValue, z));
        }
        else if (direction == Vector3Int.left) {
            meshClass.AddVertex(new Vector3(x, y - smallValue, z + 1f + smallValue));
            meshClass.AddVertex(new Vector3(x, y + 1f + smallValue, z + 1f + smallValue));
            meshClass.AddVertex(new Vector3(x, y + 1f + smallValue, z - smallValue));
            meshClass.AddVertex(new Vector3(x, y - smallValue, z - smallValue));
        }
        else if (direction == Vector3Int.right) {
            meshClass.AddVertex(new Vector3(x + 1f, y - smallValue, z - smallValue));
            meshClass.AddVertex(new Vector3(x + 1f, y + 1f + smallValue, z - smallValue));
            meshClass.AddVertex(new Vector3(x + 1f, y + 1f + smallValue, z + 1f + smallValue));
            meshClass.AddVertex(new Vector3(x + 1f, y - smallValue, z + 1f + smallValue));
        }
        else if (direction == Vector3Int.down) {
            meshClass.AddVertex(new Vector3(x, y, z));
            meshClass.AddVertex(new Vector3(x + 1f, y, z));
            meshClass.AddVertex(new Vector3(x + 1f, y, z + 1f));
            meshClass.AddVertex(new Vector3(x, y, z + 1f));
        }
    }
}
