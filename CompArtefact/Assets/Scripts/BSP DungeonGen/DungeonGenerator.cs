using System;
using System.Collections.Generic;
using System.Linq;
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

    /*
     *  This is the function that is responsible for handling the split and calculating all the nodes for rooms and corridors.
     *  The function begins by grabbing the binary space partitioning class as it will use the nodes from there to generate the dungeon.
     *  it will scan all nodes and any without children are the new room nodes, these will then be used in the room generator to create the rooms.
     *  Then the function will do something similar to create the corridors using all the nodes collected with the bsp.
     *  Finally both the room and corridor lists are joined using the Concat function. This final list has mapped out all nodes for the other scripts to utilise.
     */

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

        return new List<Node>(roomList).Concat(corridorList).ToList();

    }
}