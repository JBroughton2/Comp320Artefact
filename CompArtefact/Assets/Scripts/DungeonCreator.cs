using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomMinWidth, roomMinLength;
    public int maxIterations;
    public int corridorWidth;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon() 
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth,dungeonLength);
        var roomList = generator.CalculateRooms(maxIterations, roomMinWidth, roomMinLength);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
