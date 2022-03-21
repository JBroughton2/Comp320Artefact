using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class CorridorNode : Node
{
    private Node structureOne;
    private Node structureTwo;
    private int corridorWidth;
    private int modifierDistanceFromWall = 1;

    public CorridorNode(Node node1, Node node2, int corridorWidth) : base(null)
    {
        this.structureOne = node1;
        this.structureTwo = node2;
        this.corridorWidth = corridorWidth;
        GenerateCorridor();
    }

    //This function is breaking each time as after the corridor node has found its direction it will not need to change and find another.
    private void GenerateCorridor()
    {
        var relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structureOne, this.structureTwo);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structureTwo, this.structureOne);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationRightOrLeft(this.structureOne, this.structureTwo);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationRightOrLeft(this.structureTwo, this.structureOne);
                break;
            default:
                break;
                 
        }
    }

    private void ProcessRoomInRelationRightOrLeft(Node structureOne, Node structureTwo)
    {
        Node leftStructure = null;
        List<Node> leftStructureChildren = StructureManager.TraverseGraphToExtractLowestLeafs(structureOne);

        Node rightStructure = null;
        List<Node> rightStructureChildren = StructureManager.TraverseGraphToExtractLowestLeafs(structureTwo);

        //sort all the children of the left structure by the x value in the top right, this should stop overlap as these are the most right side structures from the left list.
        var sortedLeftStructures = leftStructureChildren.OrderByDescending(child => child.TopRightAreaCorner.x).ToList();
        if(sortedLeftStructures.Count == 1)
        {
            leftStructure = sortedLeftStructures[0];
        }
        else
        {
            int maxX = sortedLeftStructures[0].TopRightAreaCorner.x;
            sortedLeftStructures = sortedLeftStructures.Where(children => Math.Abs(maxX - children.TopRightAreaCorner.x) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedLeftStructures.Count);
            leftStructure = sortedLeftStructures[index];
        }

        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(
            child => GetValidYForNeightbourLeftRight(leftStructure.TopRightAreaCorner, 
            leftStructure.BottomRightAreaCorner,
            child.TopLeftAreaCorner, 
            child.BottomRightAreaCorner) != -1).OrderBy(child => child.BottomRightAreaCorner.x).ToList();
        if(possibleNeighboursInRightStructureList.Count <= 0)
        {
            rightStructure = structureTwo;
        }
        else
        {
            rightStructure = possibleNeighboursInRightStructureList[0];
        }
        int y = GetValidYForNeightbourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        while (y == -1 && sortedLeftStructures.Count > 1)
        {
            sortedLeftStructures = sortedLeftStructures.Where(Child => Child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
            leftStructure = sortedLeftStructures[0];
            y = GetValidYForNeightbourLeftRight(leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner,
            rightStructure.TopLeftAreaCorner, rightStructure.BottomLeftAreaCorner);
        }
        BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
        TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + this.corridorWidth);
    }

    //This big function will be doing all the checking for the nodes. This will check all the possible ways the corners could line up and then select
    //where to generate the corridor.
    private int GetValidYForNeightbourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if(rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
            return StructureManager.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        if(rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {
            return StructureManager.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)
                ).y;
        }
        if(leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {
            return StructureManager.CalculateMiddlePoint(
                rightNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)
                ).y;
        }
        if(leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {
            return StructureManager.CalculateMiddlePoint(
                leftNodeDown + new Vector2Int(0, modifierDistanceFromWall),
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)
                ).y;
        }
        return -1;
    }

    private void ProcessRoomInRelationUpOrDown(Node structureOne, Node structureTwo)
    {
        Node bottomStructure = null;
        List<Node> structureBottomChildren = StructureManager.TraverseGraphToExtractLowestLeafs(structureOne);
        Node topStructure = null;
        List<Node> structureAboveChildren = StructureManager.TraverseGraphToExtractLowestLeafs(structureTwo);

        var sortedBottomStructure = structureBottomChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();

        if (sortedBottomStructure.Count == 1)
        {
            bottomStructure = structureBottomChildren[0];
        }
        else
        {
            int maxY = sortedBottomStructure[0].TopLeftAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Mathf.Abs(maxY - child.TopLeftAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        var possibleNeighboursInTopStructure = structureAboveChildren.Where(
            child => GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                child.BottomLeftAreaCorner,
                child.BottomRightAreaCorner)
            != -1).OrderBy(child => child.BottomRightAreaCorner.y).ToList();
        if (possibleNeighboursInTopStructure.Count == 0)
        {
            topStructure = structureTwo;
        }
        else
        {
            topStructure = possibleNeighboursInTopStructure[0];
        }
        int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        while (x == -1 && sortedBottomStructure.Count > 1)
        {
            sortedBottomStructure = sortedBottomStructure.Where(child => child.TopLeftAreaCorner.x != topStructure.TopLeftAreaCorner.x).ToList();
            bottomStructure = sortedBottomStructure[0];
            x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner,
                bottomStructure.TopRightAreaCorner,
                topStructure.BottomLeftAreaCorner,
                topStructure.BottomRightAreaCorner);
        }
        BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
        TopRightAreaCorner = new Vector2Int(x + this.corridorWidth, topStructure.BottomLeftAreaCorner.y);
    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft,
        Vector2Int bottomNodeRight, Vector2Int topNodeLeft, Vector2Int topNodeRight)
    {
        if (topNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < topNodeRight.x)
        {
            return StructureManager.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if (topNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= topNodeRight.x)
        {
            return StructureManager.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)
                ).x;
        }
        if (bottomNodeLeft.x >= (topNodeLeft.x) && bottomNodeLeft.x <= topNodeRight.x)
        {
            return StructureManager.CalculateMiddlePoint(
                bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                topNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)

                ).x;
        }
        if (bottomNodeRight.x <= topNodeRight.x && bottomNodeRight.x >= topNodeLeft.x)
        {
            return StructureManager.CalculateMiddlePoint(
                topNodeLeft + new Vector2Int(modifierDistanceFromWall, 0),
                bottomNodeRight - new Vector2Int(this.corridorWidth + modifierDistanceFromWall, 0)

                ).x;
        }
        return -1;
    }

    //This method will check first find the middle point of the two rooms, this will be used in our angle check which will then find the direction the corridor will need to go to connect.
    private RelativePosition CheckPositionStructure2AgainstStructure1()
    {
        Vector2 middlePointStructure1Temp = ((Vector2)structureOne.TopRightAreaCorner + structureOne.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStructure2Temp = ((Vector2)structureTwo.TopRightAreaCorner + structureTwo.BottomLeftAreaCorner) / 2;
        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if((angle < 45 && angle >= 0) || (angle > -45 && angle < 0))
        {
            return RelativePosition.Right;
        }
        else if(angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if(angle > -135 && angle < -45)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }
    }

    private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
    {
        //Using the atan2 I can then check it and find out which direction the other structure is in. This will be used to decice which way the corridor will go.
        return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y, middlePointStructure2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
    }
}