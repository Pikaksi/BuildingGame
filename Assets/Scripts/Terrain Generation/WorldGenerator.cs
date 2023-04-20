using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldGenerator
{
    public static void GenerateChunk(ChunkDataClass chunkDataClass, StructureChunkData structures, float scale, int octaves, float persistance, float lacunarity, int lowestHeight, int biggestHeight, float airTresshold, float flatness, int normalHeight, ChunkScript chunkScript)
    {
        //float[,] perlinMap = NoiseGenerator.GenerateChunk2dNoise(16, 16, chunkDataClass.location.x * 16, chunkDataClass.location.y * 16, scale, octaves, persistance, lacunarity);
        float[,,] perlinMap = NoiseGenerator.GenerateChunk3dNoise(16, 256, 16, chunkDataClass.location.x * 16, 0, chunkDataClass.location.y * 16, 
                                                                  scale, octaves, persistance, lacunarity, flatness, normalHeight);
        int lastAirCounter = 0;
        for (int x = 0; x < 16; x++) {            
            for (int z = 0; z < 16; z++) {
                for (int y = 255; y >= 0; y--) {
                    if (perlinMap[x, y, z] < airTresshold) {
                        chunkDataClass.chunkBlocks[x, y, z] = new BlockStruct(BlockEnums.Air, 0);
                        lastAirCounter = 0;
                    }
                    else {
                        if (lastAirCounter == 0) {
                            chunkDataClass.chunkBlocks[x, y, z] = new BlockStruct(BlockEnums.Grass, 0);
                        }
                        else if (lastAirCounter > 0 && lastAirCounter < 4) {
                            chunkDataClass.chunkBlocks[x, y, z] = new BlockStruct(BlockEnums.Dirt, 0);
                        }
                        else {
                            chunkDataClass.chunkBlocks[x, y, z] = new BlockStruct(BlockEnums.Stone, 0);
                        }
                        lastAirCounter++;
                    }
                }
            }
        }
        AddTreesToChunk(chunkDataClass, chunkScript);
        AddBeforeRenderedBlocks(structures, chunkDataClass);
    }

    private static void AddBeforeRenderedBlocks(StructureChunkData structures, ChunkDataClass chunkDataClass)
    {
        if (structures == null) {
            return;
        }
        for (int i = 0; i < structures.blocksToPlace.Count; i++) {
            chunkDataClass.chunkBlocks[structures.blockLocations[i].x, structures.blockLocations[i].y, structures.blockLocations[i].z] = structures.blocksToPlace[i];
        }
    }

    private static void AddTreesToChunk(ChunkDataClass chunkDataClass, ChunkScript chunkScript)
    {
        for (int x = 0; x < 16; x++) {            
            for (int z = 0; z < 16; z++) {
                if (Random.Range(0f, 1f) <= 0.01f) {
                    
                    for (int y = 255; y >= 0; y--) {
                        if (chunkDataClass.chunkBlocks[x, y, z].blockType == BlockEnums.Grass) {
                            GenerateTree(new Vector3Int(x, y + 1, z), chunkDataClass, chunkScript);
                        } 
                    }
                }
            }
        }
    }

    private static void GenerateTree(Vector3Int location, ChunkDataClass chunkDataClass, ChunkScript chunkScript)
    {
        PlaceStructure(location, StructureData.treeBlocks, StructureData.treeBlockLocations, chunkDataClass, chunkScript);
    }

    private static void PlaceStructure(Vector3Int structureLocation, List<BlockStruct> blocksToPlace, List<Vector3Int> blockLocations, ChunkDataClass chunkDataClass, ChunkScript chunkScript)
    {
        for (int i = 0; i < blocksToPlace.Count; i++) {
            // is inside chunk
            Vector3Int location = new Vector3Int(structureLocation.x + blockLocations[i].x, 
                                                 structureLocation.y + blockLocations[i].y, 
                                                 structureLocation.z + blockLocations[i].z);
            if (location.x >= 0 && location.x < 16 && location.y >= 0 && location.y < 256 && location.z >= 0 && location.z < 16) {
                chunkDataClass.chunkBlocks[location.x, location.y, location.z] = blocksToPlace[i];
            }
            else {
                chunkScript.StoreUnrenderedBlock(location, chunkDataClass.location, blocksToPlace[i]);
            }
        }
    }
}
