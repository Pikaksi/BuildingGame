using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkMeshRenderer : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    Mesh mesh;

    private void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        mesh = meshFilter.mesh;
    }

    public void RenderChunkMesh(MeshClass meshClass)
    {
        //Debug.Log("triangle count = " + meshClass.triangles.Count);
        //Debug.Log("vertex count = " + meshClass.vertices.Count);
        //Debug.Log("uv coordinate count = " + meshClass.uv.Count);
        mesh.Clear();

        mesh.vertices = meshClass.vertices.ToArray();
        mesh.triangles = meshClass.triangles.ToArray();
        mesh.uv = meshClass.uv.ToArray();

        mesh.RecalculateNormals();

        Mesh colliderMesh = new Mesh();
        colliderMesh.vertices = meshClass.colliderVertices.ToArray();
        colliderMesh.triangles = meshClass.colliderTriangles.ToArray();
        colliderMesh.RecalculateNormals();

        meshCollider.sharedMesh = colliderMesh;
    }
}
