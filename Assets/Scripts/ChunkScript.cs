using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkScript : MonoBehaviour
{
    [SerializeField] int renderDistance;
    [SerializeField] Vector2 playerLocation;

    [Header("World generation")]
    [SerializeField] float scale;
    [SerializeField] int octaves;
    [SerializeField] float persistance; 
    [SerializeField] float lacunarity;
    [SerializeField] int worldMinHeight;
    [SerializeField] int worldMaxHeight;
    [SerializeField] int chunkLenght;
    [SerializeField] int chunkHeight;
    [SerializeField] float flatness;
    [SerializeField] int normalHeight;
    [SerializeField] float airTresshold;
    [SerializeField] bool renderSidesOutOfWorld;

    Dictionary<Vector2Int, ChunkDataClass> chunkDictionary = new Dictionary<Vector2Int, ChunkDataClass>();
    Dictionary<Vector2Int, StructureChunkData> unrenderedStructures = new Dictionary<Vector2Int, StructureChunkData>();

    ChunkRenderer chunkRenderer;
    GameObject playerGameObject;
    Vector2Int previousChunk;

    [Header("Debug")]
    [SerializeField] Vector2Int playerCurrentChunk;

    private void Awake()
    {
        chunkRenderer = gameObject.GetComponent<ChunkRenderer>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        TextureDataManager.InitBlockTextures();
    }

    void Start()
    {
        UpdateRenderedChunks();
    }

    void Update()
    {
        playerCurrentChunk = new Vector2Int(Mathf.FloorToInt(playerGameObject.transform.position.x / 16),
                                            Mathf.FloorToInt(playerGameObject.transform.position.z / 16));

        if (playerCurrentChunk != previousChunk) {
            UpdateRenderedChunks();
        }

        previousChunk = playerCurrentChunk;
    }

    private void UpdateRenderedChunks()
    {
        DateTime time = DateTime.Now;
        List<Vector2Int> chunksToBeRendered = GetChunksInRenderDistance(playerCurrentChunk);

        for (int i = 0; i < chunksToBeRendered.Count; i++) {
            Vector2Int chunk = chunksToBeRendered[i];
            GenerateChunk(chunk);
            chunkRenderer.InstantiateChunk(chunk);
        }

        chunkRenderer.UnRenderChunks(playerCurrentChunk, renderDistance);
        Debug.Log("took " + (DateTime.Now - time));
    }

    private List<Vector2Int> GetChunksInRenderDistance(Vector2Int playerChunk)
    {
        List<Vector2Int> returnList = new List<Vector2Int>();

        for (int x = -renderDistance; x <= renderDistance; x++) {
            for (int z = -renderDistance; z <= renderDistance * 2 + 1; z++) {
                Vector2Int chunk = new Vector2Int(x + playerChunk.x, z + playerChunk.y);

                if (Mathf.Sqrt(Mathf.Pow(chunk.x - playerChunk.x, 2) + Mathf.Pow(chunk.y - playerChunk.y, 2)) < renderDistance + 0.5f) {
                    returnList.Add(chunk);
                }
            }
        }

        return returnList;
    }

    public ChunkDataClass GetChunkFromLocation(Vector2Int location)
    {
        return chunkDictionary[location];
    }

    public BlockStruct NeighboringBlockFromCoordinates(Vector2Int chunkCoordinates, Vector3Int blockCoordinates)
    {
        if (blockCoordinates.y > 255) {
            return new BlockStruct(BlockEnums.Air, 0);
        }
        if (blockCoordinates.y < 0) {
            return new BlockStruct(BlockEnums.Stone, 0);
        }
        if (blockCoordinates.x < 0) {
            chunkCoordinates.x--;
            blockCoordinates.x += 16;
        } 
        else if (blockCoordinates.x > 15) {
            chunkCoordinates.x++;
            blockCoordinates.x -= 16;
        }
        else if (blockCoordinates.z < 0) {
            chunkCoordinates.y--;
            blockCoordinates.z += 16;
        } 
        else if (blockCoordinates.z > 15) {
            chunkCoordinates.y++;
            blockCoordinates.z -= 16;
        }
        if (chunkDictionary.TryGetValue(chunkCoordinates, out ChunkDataClass chunk)) {
            return chunk.chunkBlocks[blockCoordinates.x, blockCoordinates.y, blockCoordinates.z];
        }
        // if there is no chunk loaded in the requested coordinates
        else {
            return renderSidesOutOfWorld ? new BlockStruct(BlockEnums.Air, 0) : new BlockStruct(BlockEnums.Stone, 0);
        }
    }

    public void GenerateChunk(Vector2Int location)
    {
        if (!chunkDictionary.ContainsKey(location)) {

            ChunkDataClass chunkDataClass = new ChunkDataClass(chunkLenght, chunkHeight, location);
            unrenderedStructures.TryGetValue(location, out StructureChunkData chunkStructures);

            WorldGenerator.GenerateChunk(chunkDataClass, chunkStructures, scale, octaves, persistance, lacunarity, worldMinHeight, worldMaxHeight, airTresshold, flatness, normalHeight, this);
            
            chunkDictionary.Add(location, chunkDataClass);
        }
    }

    public void StoreUnrenderedBlock(Vector3Int location, Vector2Int chunkLocation, BlockStruct block)
    {
        if (location.y > 255 || location.y < 0) {
            return;
        }
        if (location.x < 0) {
            chunkLocation.x--;
            location.x += 16;
        } 
        else if (location.x > 15) {
            chunkLocation.x++;
            location.x -= 16;
        }
        if (location.z < 0) {
            chunkLocation.y--;
            location.z += 16;
        } 
        else if (location.z > 15) {
            chunkLocation.y++;
            location.z -= 16;
        }
        if (chunkDictionary.TryGetValue(chunkLocation, out ChunkDataClass chunkData)) {
            chunkData.chunkBlocks[location.x, location.y, location.z] = block;
        }
        else if (unrenderedStructures.TryGetValue(chunkLocation, out StructureChunkData chunkStructures)) {
            chunkStructures.AddBlock(block, location);
        }
        else {
            unrenderedStructures.Add(chunkLocation, new StructureChunkData(location, block));
        }
    }
}
