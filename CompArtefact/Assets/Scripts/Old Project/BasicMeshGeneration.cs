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

    public int m_length;
    public int m_width;

    public int xSize, zSize;

    void Awake()
    {
        //StartCoroutine(CreateShape());
        CreateShape();
        //UpdateMesh();
    }

    //change to IEnumerator if you want to see it do each section.
    void CreateShape()
    {
        //uncomment if want to see process slower.
        //WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = myMesh = new Mesh();
        myMesh.name = "ProceduralMesh";

        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for(int i = 0, z = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++, i++) {
                vertices[i] = new Vector3(x, z);
            }
        }

        //set the mesh vertices to be that which we calculated above
        myMesh.vertices = vertices;

        //multiply by the amount of quads we want
        int[] triangles = new int[xSize * 6];
        for (int ti = 0, vi = 0, x = 0; x < xSize; x++, ti += 6, vi++) 
        {
            triangles[ti] = vi;
            triangles[ti + 3] = triangles[ti + 2] = vi + 1;
            triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
            triangles[ti + 5] = vi + xSize + 2;
            //set my mesh triangles to be that calculated above.
            myMesh.triangles = triangles;
            //uncomment if want to see process slower.
            //yield return wait;
        }
        
    }


    void UpdateMesh() 
    {
        //Clears any previous data on the mesh
        myMesh.Clear();

        //Set the mesh verticies to be the ones I have specified above
        myMesh.vertices = vertices;

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
