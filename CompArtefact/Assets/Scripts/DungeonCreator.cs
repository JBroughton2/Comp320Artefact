using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomMinWidth, roomMinLength;
    public int maxIterations;
    public int corridorWidth;
    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon() 
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth,dungeonLength);
        var roomList = generator.CalculateDungeon(maxIterations, roomMinWidth, roomMinLength, corridorWidth);
        for (int i = 0; i < roomList.Count; i++)
        {
            CreateMesh(roomList[i].BottomLeftAreaCorner, roomList[i].TopRightAreaCorner);
        }
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        //begin by mapping out each corner of the room.
        Vector3 bottomLeftVertex = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightVertex = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftVertex = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightVertex = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        //filling the verticies array for the mesh
        Vector3[] vertices = new Vector3[]
        {
            topLeftVertex,
            topRightVertex,
            bottomLeftVertex,
            bottomRightVertex
        };

        //creating the uvs for the mesh
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        //finally create thje triangles for the mesh, must be placed clockwise
        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };

        //creat a new mesh and replace all the variables with my own that I just set up.
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        //Create new gameobject to hold the mesh. IT requires both of these components so it can be visualised.
        GameObject dungeonFloor = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
    }
}
