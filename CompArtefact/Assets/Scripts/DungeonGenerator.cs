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

    public List<Node> CalculateDungeon(int maxIterations, int roomMinWidth, int roomMinLength, int corridorWidth)
    {
        BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, roomMinWidth, roomMinLength);

        //extract all the lowest nodes that have no children, these will represent our rooms.
        List<Node> roomSpaces = StructureManager.TraverseGraphToExtractLowestLeafs(bsp.RootNode);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, roomMinLength, roomMinWidth);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces);

        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridors(allSpaceNodes, corridorWidth); ;

        return new List<Node>(roomList);

        

    }
}