using System;
using System.Collections.Generic;
using System.Linq;

public class CorridorsGenerator
{
    /* In this script the corridor list is being set up. 
     * It begins by going through every room that could have a corridor and creating new corridors, 
     * if there are no children then that room cannot have a corridor so the script continues past.
     * Any that can though will be added to the corridors list, this will be used later when placing walls and floors.
     */
    public List<Node> CreateCorridors(List<RoomNode> allSpaceNodes, int corridorWidth)
    {
        List<Node> corridorList = new List<Node>();
        Queue<RoomNode> structureToCheck = new Queue<RoomNode>(allSpaceNodes.OrderByDescending(Node => Node.TreeLayerIndex).ToList());
        while(structureToCheck.Count > 0)
        {
            var node = structureToCheck.Dequeue();
            if(node.ChildrenNodeList.Count == 0)
            {
                continue;
            }
            CorridorNode corridor = new CorridorNode(node.ChildrenNodeList[0], node.ChildrenNodeList[1], corridorWidth);
            corridorList.Add(corridor);
        }
        return corridorList;
    }
}