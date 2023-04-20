using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : MonoBehaviour
{
    [SerializeField] GameObject chunkHolderGameObject;
    [SerializeField] GameObject block1;

    List<Vector2Int> renderedChunks = new List<Vector2Int>();
    Dictionary<Vector2Int, GameObject> chunkToObjectHolder = new Dictionary<Vector2Int, GameObject>();

    Vector3Int[] DirectionLookupTable3d = new Vector3Int[6] {new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0), new Vector3Int(0, 0, 1), 
                                                           new Vector3Int(0, 0, -1), new Vector3Int(1, 0, 0), new Vector3Int(-1, 0, 0)};
    
    Vector2Int[] DirectionLookupTable2d = new Vector2Int[4] { new Vector2Int(0, 1), new Vector2Int(1, 0), 
                                                              new Vector2Int(0, -1), new Vector2Int(-1, 0) };

    ChunkScript chunkScript;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    private void Awake()
    {
        chunkScript = gameObject.GetComponent<ChunkScript>();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void InstantiateChunk(Vector2Int location)
    {
        if (!chunkToObjectHolder.ContainsKey(location)) {
            renderedChunks.Add(location);
            RenderChunk(chunkScript.GetChunkFromLocation(location), true);
        }
    }

    public void UnRenderChunks(Vector2Int playerChunk, int renderDistance)
    {
        
        for (int i = renderedChunks.Count - 1; i >= 0; i--) {
            Vector2Int chunk = renderedChunks[i];
            /*Debug.Log("-------------------------------");
            Debug.Log(playerChunk);
            Debug.Log(chunk);
            Debug.Log(Mathf.Sqrt(Mathf.Pow(chunk.x - playerChunk.x, 2) + Mathf.Pow(chunk.y - playerChunk.y, 2)));*/

            if (Mathf.Sqrt(Mathf.Pow(chunk.x - playerChunk.x, 2) + Mathf.Pow(chunk.y - playerChunk.y, 2)) >= renderDistance + 0.5f) {
                Destroy(chunkToObjectHolder[chunk]);
                chunkToObjectHolder.Remove(chunk);
                renderedChunks.RemoveAt(i);
            }
        }
    }

    public void RenderChunk(ChunkDataClass chunkData, bool rerenderNeighbors)
    {
        if (chunkToObjectHolder.ContainsKey(chunkData.location)) {
            Destroy(chunkToObjectHolder[chunkData.location]);
            chunkToObjectHolder.Remove(chunkData.location);
        }

        GameObject holder = Instantiate(chunkHolderGameObject, new Vector3 (chunkData.location.x * 16, 0, chunkData.location.y * 16), Quaternion.identity) as GameObject; 
        holder.name = "holder" + chunkData.location;
        chunkToObjectHolder.Add(chunkData.location, holder);
        MeshClass meshClass = new MeshClass();


        for (int x = 0; x < 16; x++) {            
            for (int z = 0; z < 16; z++) {
                for (int y = 0; y < 256; y++) {
                    BlockStruct block = chunkData.chunkBlocks[x, y, z];
                    if (block.blockType == BlockEnums.Air) {
                        continue;
                    }
                    RenderNeededSides(meshClass, block.blockType, chunkData.location, x, y, z, chunkData);
                }
            }
        }

        holder.GetComponent<ChunkMeshRenderer>().RenderChunkMesh(meshClass);

        if (rerenderNeighbors) {
            for (int i = 0; i < 4; i++) {
                Vector2Int location = chunkData.location + DirectionLookupTable2d[i];
                if (chunkToObjectHolder.ContainsKey(location)) {
                    RenderChunk(chunkScript.GetChunkFromLocation(location), false);
                }
            }
        }
    }

    private void RenderNeededSides(MeshClass meshClass, BlockEnums blockType, Vector2Int chunkLocation, int x, int y, int z, ChunkDataClass chunkData)
    {
        BlockStruct adjacentBlock = new BlockStruct();
        foreach (Vector3Int direction in DirectionLookupTable3d)
        {
            Vector3Int blockLocation = new Vector3Int(x, y, z) + direction;
            if (blockLocation.x >= 0 && blockLocation.x < 16 && blockLocation.y >= 0 && blockLocation.y < 256 && blockLocation.z >= 0 && blockLocation.z < 16) {
                adjacentBlock = chunkData.chunkBlocks[blockLocation.x, blockLocation.y, blockLocation.z];
            }
            else {
                adjacentBlock = chunkScript.NeighboringBlockFromCoordinates(chunkLocation, blockLocation);
            }
            //try {
                if (!TextureDataManager.BlockTextureDataDict[adjacentBlock.blockType].isFullBlock) {
                    TextureDataManager.AddBlockFaceData(direction, meshClass, blockType, x, y, z);
                }
            /*}
            catch {
                Debug.Log(blockLocation);
                Debug.LogError(adjacentBlock.blockType);
            }*/
        }
    }
}
