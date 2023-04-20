using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureChunkData
{
    public List<BlockStruct> blocksToPlace = new List<BlockStruct>();
    public List<Vector3Int> blockLocations = new List<Vector3Int>();

    public StructureChunkData(Vector3Int blockLocation, BlockStruct blockToPlace)
    {
        this.blocksToPlace.Add(blockToPlace);
        this.blockLocations.Add(blockLocation);
    }

    public void AddBlock(BlockStruct blockToPlace, Vector3Int blockLocation)
    {
        this.blocksToPlace.Add(blockToPlace);
        this.blockLocations.Add(blockLocation);
    }
}
