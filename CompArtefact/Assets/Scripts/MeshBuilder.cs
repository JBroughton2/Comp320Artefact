using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    private List<Vector3> m_Vertices = new List<Vector3>();
    public List<Vector3> Vertices { get { return m_Vertices; } }

    private List<float> m_Normals = new List<float>();
    public List<float> Normals { get { return m_Normals; } }
    
    private List<float> m_UVs = new List<float>();
    public List<float> UVs { get { return m_UVs; } }

    private List<int> m_Indices = new List<int>();

    public void AddTriangle(int index0, int index1, int index2)
    {
        m_Indices.Add(index0);
        m_Indices.Add(index1);
        m_Indices.Add(index2);
    }

    public Mesh CreateMesh()
    {
        Mesh customMesh = new Mesh();

        customMesh.vertices = m_Vertices.ToArray();
        customMesh.triangles = m_Indices.ToArray();

        customMesh.RecalculateBounds();

        return customMesh;
    }
}
