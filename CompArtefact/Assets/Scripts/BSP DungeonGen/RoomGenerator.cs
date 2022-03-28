using System;
using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator
{
    private int maxIterations;
    private int roomMinLength;
    private int roomMinWidth;

    //this is the custom room generator constructor.
    public RoomGenerator(int maxIterations, int roomMinLength, int roomMinWidth)
    {
        this.maxIterations = maxIterations;
        this.roomMinLength = roomMinLength;
        this.roomMinWidth = roomMinWidth;
    }

    //Here I am using the room nodes generated in the DungeonGenerator to map out the coordinates of each corner,
    //once this is done it will return a list for use in the visualisation process

    public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces)
    {
        List<RoomNode> listToReturn = new List<RoomNode>();
        foreach(var space in roomSpaces)
        {
            Vector2Int newBottomLeftPoint = StructureManager.GenerateBottomLeftCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, 0.1f, 1);
            Vector2Int newTopRightPoint = StructureManager.GenerateTopRightCornerBetween(
                space.BottomLeftAreaCorner, space.TopRightAreaCorner, 0.9f, 1);
            space.BottomLeftAreaCorner = newBottomLeftPoint;
            space.TopRightAreaCorner = newTopRightPoint;
            space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
            listToReturn.Add((RoomNode)space);
        }
        return listToReturn;
    }
}