using System;
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

    [Header("Walls")]
    public GameObject wallVertical, wallHorizontal;
    List<Vector3Int> possibleDoorVerticalPos;
    List<Vector3Int> possibleDoorHorizPos;
    List<Vector3Int> possibleWallHorizontalPos;
    List<Vector3Int> possibleWallVerticalPos;

    void Start()
    {
        GenerateDungeon();
    }

    // GenerateDungeon will be responsible for taking the users input and sending it to the generator, 
    // It then has to set up the wall lists and finally create the meshes for every room node in the list that the generator has returned.
    public void GenerateDungeon() 
    {
        DestroyAllChildren();
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth,dungeonLength);
        var roomList = generator.CalculateDungeon(maxIterations, roomMinWidth, roomMinLength, corridorWidth);


        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPos = new List<Vector3Int>();
        possibleDoorHorizPos = new List<Vector3Int>();
        possibleWallHorizontalPos = new List<Vector3Int>();
        possibleWallVerticalPos = new List<Vector3Int>();

        for (int i = 0; i < roomList.Count; i++)
        {
            CreateMesh(roomList[i].BottomLeftAreaCorner, roomList[i].TopRightAreaCorner);          
        }
        CreateWalls(wallParent);
    }

    //The walls will be using the information that comes from the mesh creation function to fill in the walls with prefabs.
    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPos)
        {
            Instantiate(wallHorizontal, wallPosition, Quaternion.identity, wallParent.transform);
        }
        foreach (var wallPosition in possibleWallVerticalPos)
        {
            Instantiate(wallVertical, wallPosition, Quaternion.identity, wallParent.transform);
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

        //Create new gameobject to hold the mesh. It requires both of these components so it can be visualised.
        GameObject dungeonFloor = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        /*
         * The walls and floor generation is quite messy as there are many different variables having to managed at once here.
         * It begins by with some simple variable setting however then has to complete four different for loops
         * these loops manage all the different positions a wall could be and adds them to the wall position list that is used above.
         */
        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        for (int row = (int)bottomLeftVertex.x; row < (int)bottomRightVertex.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftVertex.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPos, possibleDoorHorizPos);
        }
        for (int row = (int)topLeftVertex.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightVertex.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPos, possibleDoorHorizPos);
        }
        for (int col = (int)bottomLeftVertex.z; col < (int)topLeftVertex.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftVertex.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPos, possibleDoorVerticalPos);
        }
        for (int col = (int)bottomRightVertex.z; col < (int)topRightVertex.z; col++)
        {
            var wallPosition = new Vector3(bottomRightVertex.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPos, possibleDoorVerticalPos);
        }
    }

    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while(transform.childCount != 0)
        {
            foreach(Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}
