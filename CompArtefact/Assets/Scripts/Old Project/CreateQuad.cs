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
        GenerateCube();
        GenerateMesh();
    }

    public void GenerateCube() 
    {
        Vector3 upDir = Vector3.up * m_height;
        Vector3 rightDir = Vector3.right * m_Width;
        Vector3 forwardDir = Vector3.forward * m_Length;

        Vector3 nearCorner = Vector3.zero;
        Vector3 farCorner = upDir + rightDir + forwardDir;

        //base of the cube
        QuadGeneration(meshBuilder, nearCorner, forwardDir, rightDir);
        //front face
        QuadGeneration(meshBuilder, nearCorner, rightDir, upDir);
        //left side face
        QuadGeneration(meshBuilder, nearCorner, upDir, forwardDir);

        //top face, fright and forward flipped so it faces up not down and numbers made negative to come the right direction.
        QuadGeneration(meshBuilder, farCorner, -rightDir, -forwardDir);
        //back face
        QuadGeneration(meshBuilder, farCorner, -upDir, -rightDir);
        //right side face
        QuadGeneration(meshBuilder, farCorner, -forwardDir, -upDir);
    }

    void QuadGeneration(MeshBuilder meshBuilder, Vector3 vertexOffset, Vector3 widthDir, Vector3 lengthDir)
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

    public void GenerateMesh() 
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
