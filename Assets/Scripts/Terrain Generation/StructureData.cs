using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureData
{
    public static List<BlockStruct> treeBlocks = new List<BlockStruct>();
    public static List<Vector3Int> treeBlockLocations = new List<Vector3Int>();

    public static void GenerateTreeStructure()
    {
        int treeHeight = Random.Range(1, 8);
        for (int y = 0; y < 3; y++) {
            for (int x = 0; x < 5; x++) {
                for (int z = 0; z < 5; z++) {
                    treeBlocks.Add(new BlockStruct(BlockEnums.Leaf, 0));
                    treeBlockLocations.Add(new Vector3Int(x - 2, treeHeight - y, z - 2));
                }
            }
        }

        for (int y = 0; y < treeHeight; y++) {
            treeBlocks.Add(new BlockStruct(BlockEnums.Log, 0));
            treeBlockLocations.Add(new Vector3Int(0, y, 0));
        }
    }
}
