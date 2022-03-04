using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class CorridorNode : Node
{
    private Node structureOne;
    private Node structureTwo;
    private int corridorWidth;

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

        var possibleNeighboursInRightStructureList = rightStructureChildren.Where(child => GetValidYForNeightbourLeftRight(leftStructure.TopRightAreaCorner, leftStructure.BottomRightAreaCorner, child.TopLeftAreaCorner, child.BottomRightAreaCorner) != -1).ToList();
    }

    //This big function will be doing all the checking for the nodes. This will check all the possible ways the corners could line up and then select
    //where to generate the corridor.
    private int GetValidYForNeightbourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if(rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {

        }
        if(rightNodeUp.y <= leftNodeUp.y && leftNodeDown.y <= rightNodeDown.y)
        {

        }
        if(leftNodeUp.y >= rightNodeDown.y && leftNodeUp.y <= rightNodeUp.y)
        {

        }
        if(leftNodeDown.y >= rightNodeDown.y && leftNodeDown.y <= rightNodeUp.y)
        {

        }
        return -1;
    }

    private void ProcessRoomInRelationUpOrDown(Node structureOne, Node structureTwo)
    {
        throw new NotImplementedException();
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