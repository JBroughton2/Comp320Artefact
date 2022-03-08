using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureManager
{
    //This is going to be the method I use to pick out all the nodes that dont have children, these will then be
    //used as the actual rooms as they are the end of the branches.
    public static List<Node> TraverseGraphToExtractLowestLeafs(Node parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> listToReturn = new List<Node>();
        if(parentNode.ChildrenNodeList.Count == 0)
        {
            return new List<Node>() { parentNode };
        }
        foreach(var child in parentNode.ChildrenNodeList)
        {
            nodesToCheck.Enqueue(child);
        }
        while(nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if(currentNode.ChildrenNodeList.Count == 0)
            {
                listToReturn.Add(currentNode);
            }
            else
            {
                foreach(var child in currentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                }
            }
        }
        return listToReturn;
    }

    public static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeft, Vector2Int boundaryRight, float pointModifier, int offset)
    {
        int minX = boundaryLeft.x + offset;
        int maxX = boundaryRight.x - offset;
        int minY = boundaryLeft.y + offset;
        int maxY = boundaryRight.y - offset;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
            Random.Range(minY, (int)(minY + (minY - minY) * pointModifier)));
    }

    public static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeft, Vector2Int boundaryRight, float pointModifier, int offset)
    {
        int minX = boundaryLeft.x + offset;
        int maxX = boundaryRight.x - offset;
        int minY = boundaryLeft.y + offset;
        int maxY = boundaryRight.y - offset;
        return new Vector2Int(
            Random.Range((int)(minX+(maxX-minX)*pointModifier), maxX),
            Random.Range((int)(minY +(maxY - minY)*pointModifier), maxY));
    }

    public static Vector2Int CalculateMiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}
public enum RelativePosition
{
    Up,
    Down,
    Right,
    Left
}