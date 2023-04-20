using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshClass
{
    public List<Vector3> vertices = new List<Vector3>();
    public List<int> triangles = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> colliderVertices = new List<Vector3>();
    public List<int> colliderTriangles = new List<int>();

    public MeshClass()
    {

    }

    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        colliderVertices.Add(vertex);
    }

    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);

        colliderTriangles.Add(colliderVertices.Count - 4);
        colliderTriangles.Add(colliderVertices.Count - 3);
        colliderTriangles.Add(colliderVertices.Count - 2);
        colliderTriangles.Add(colliderVertices.Count - 4);
        colliderTriangles.Add(colliderVertices.Count - 2);
        colliderTriangles.Add(colliderVertices.Count - 1);
    }
}
