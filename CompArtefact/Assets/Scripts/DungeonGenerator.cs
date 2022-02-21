using System;
using System.Collections.Generic;
using UnityEngine;
public class DungeonGenerator
{
    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;


    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonLength = dungeonLength;
        this.dungeonWidth = dungeonWidth;
    }

    public List<Node> CalculateRooms(int maxIterations, int roomMinWidth, int roomMinLength)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomMinWidth, roomMinLength);
        return new List<Node>(allSpaceNodes);
    }
}