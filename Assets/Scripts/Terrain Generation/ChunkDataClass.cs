using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDataClass
{
    public BlockStruct[,,] chunkBlocks;
    public Vector2Int location;

    public ChunkDataClass(int horizontalSize, int verticalSize, Vector2Int location)
    {
        this.chunkBlocks = new BlockStruct[horizontalSize, verticalSize, horizontalSize];
        this.location = location;
    }
}
