using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuad : MonoBehaviour
{
    public float m_Length, m_Width, m_height;
    public int m_segmentCount;

    MeshBuilder meshBuilder = new MeshBuilder();

    private void Start()
    {
        /*
        for(int i = 0; i < m_segmentCount; i++)
        {
            float z = m_Length * i;

            for(int j = 0; j < m_segmentCount; j++)
            {
                float x = m_Width * j;

                Vector3 offset = new Vector3(x, 0, z);

                QuadGeneration(meshBuilder, offset);
            }
        }
        */

        Vector3 upDir = Vector3.up * m_height;
        Vector3 rightDir = Vector3.right * m_Width;
        Vector3 forwardDir = Vector3.forward * m_Length;

        Vector3 nearCorner = Vector3.zero;
        Vector3 farCorner = upDir + rightDir + forwardDir;

        //base of the cube
        CubeGeneration(meshBuilder, nearCorner, forwardDir, rightDir);
        //front face
        CubeGeneration(meshBuilder, nearCorner, rightDir, upDir);
        //left side face
        CubeGeneration(meshBuilder, nearCorner, upDir, forwardDir);

        //top face, fright and forward flipped so it faces up not down and numbers made negative to come the right direction.
        CubeGeneration(meshBuilder, farCorner, -rightDir, -forwardDir);
        //back face
        CubeGeneration(meshBuilder, farCorner, -upDir, -rightDir);
        //right side face
        CubeGeneration(meshBuilder, farCorner, -forwardDir, -upDir);

        GenerateMesh();
    }

    /*
    void QuadGeneration(MeshBuilder meshBuilder, Vector3 vertexOffset, Vector3 widthDir, Vector3 lengthDir) 
    {
        //Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, 0.0f) + vertexOffset);

        meshBuilder.Vertices.Add(new Vector3(0.0f, 0.0f, m_Length) + vertexOffset);

        meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, m_Length) + vertexOffset);

        meshBuilder.Vertices.Add(new Vector3(m_Width, 0.0f, 0.0f) + vertexOffset);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }
    */

    void CubeGeneration(MeshBuilder meshBuilder, Vector3 vertexOffset, Vector3 widthDir, Vector3 lengthDir)
    {
        //Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(vertexOffset);

        meshBuilder.Vertices.Add(vertexOffset + lengthDir);

        meshBuilder.Vertices.Add(vertexOffset + lengthDir + widthDir);

        meshBuilder.Vertices.Add(vertexOffset + widthDir);

        int baseIndex = meshBuilder.Vertices.Count - 4;

        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);
    }

    void GenerateMesh() 
    {
        MeshFilter filter = this.gameObject.GetComponent<MeshFilter>();

        if(filter != null)
        {
            filter.sharedMesh = meshBuilder.CreateMesh();
        }
    }

    //Set up the vertices and tris
    //
    //meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
    //meshBuilder.Normals.Add(Vector3.up);

    //
    //meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
    //meshBuilder.Normals.Add(Vector3.up);

    //
    //meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
    //meshBuilder.Normals.Add(Vector3.up);

    //
    //meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
    //meshBuilder.Normals.Add(Vector3.up);

    //

    //create mesh
    //
}
