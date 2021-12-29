using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BasicMeshGeneration : MonoBehaviour
{
    Mesh myMesh;

    Vector3[] vertices;
    Vector2[] uvs;
    Vector3[] normals;
    int[] triangles;

    public int m_length;
    public int m_width;

    public int xSize, zSize;

    //Triangles have to be made clockewise in unity.
    // Start is called before the first frame update
    void Start()
    {
        myMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = myMesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        /*
        vertices = new Vector3[]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (0, 0, m_length),
            new Vector3 (m_width, 0, 0),
            new Vector3 (m_width, 0, m_length)
        };

        uvs = new Vector2[]
        {
            new Vector2 (0.0f, 0.0f),
            new Vector2 (0.0f, 1.0f),
            new Vector2 (1.0f, 1.0f),
            new Vector2 (1.0f, 0.0f)
        };

        normals = new Vector3[]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2
        };
        */

        
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++, i++) 
            {
                vertices[i] = new Vector3(x, z);
            }
        }
        
    }

    void UpdateMesh() 
    {
        //Clears any previous data on the mesh
        myMesh.Clear();

        //Set the mesh verticies to be the ones I have specified above
        myMesh.vertices = vertices;
        //Set the mesh triangles to be the ones I have specified above
        myMesh.triangles = triangles;

        myMesh.RecalculateBounds();
    }

    
    private void OnDrawGizmos()
    {
        //stops errors during edit mode.
        if(vertices == null) 
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
    
}
