using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class BinarySpacePartitioner
{
    RoomNode rootNode;
    public RoomNode RootNode { get => rootNode; }

    //This is the main function is responsible for setting up the original room node from which all others will branch from.
    public BinarySpacePartitioner(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    //This list will control all the room nodes and handle them, it is also here where I parse the variables responsible for splitting up the rooms.
    //For each room that is is available to be split while we are still iterating then the split function will be called.
    public List<RoomNode> PrepareNodesCollection(int maxIterations, int roomMinWidth, int roomMinLength)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxIterations && graph.Count > 0) 
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if(currentNode.width >= roomMinWidth*2 || currentNode.length >= roomMinLength * 2) 
            {
                SplitTheSpace(currentNode, listToReturn, roomMinLength, roomMinWidth, graph);
            }

        }

        return listToReturn;
    }

    //Here at the split function I am parsing the current node and will be using the minimum sizes to divide the space.
    //the room will be split either horizontally or vertically.
    //When the room has been split it will turn into two new nodes, each with the new size information stored and later added to the list via function.
    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomMinLength, int roomMinWidth, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(currentNode.BottomLeftAreaCorner,
            currentNode.TopRightAreaCorner,
            roomMinWidth, 
            roomMinLength);

        RoomNode node1, node2;
        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode(currentNode.BottomLeftAreaCorner,
                new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y),
                currentNode.TopRightAreaCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }
        AddNewNodesToCollections(listToReturn, graph, node1);
        AddNewNodesToCollections(listToReturn, graph, node2);
    }

    //This function is rather simple but helps cut down on duplicate code.
    private void AddNewNodesToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node)
    {
        listToReturn.Add(node);
        graph.Enqueue(node);
    }

    //This function is responsible for deciding which orientation the room will be cut.
    //It does so depending on how much room there will be as every space needs to abide by the minimum size requirements.
    //To find the cut coordinates I have used another function in an attempt to keep this script clean.
    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomMinWidth, int roomMinLength)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2 * roomMinWidth;
        bool widthStatus = (topRightAreaCorner.x - bottomLeftAreaCorner.x) >= 2 * roomMinLength;
        if(lengthStatus && widthStatus) 
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }
        else if (widthStatus) 
        {
            orientation = Orientation.Vertical;
        }
        else 
        {
            orientation = Orientation.Horizontal;
        }
        return new Line(orientation, GetCoordinatesFororientation(orientation, 
            bottomLeftAreaCorner,
            topRightAreaCorner, 
            roomMinWidth, 
            roomMinLength));
    }

    //These coordinates will be used to start the cutting line,
    //Its a simple function as there are only two places that the line will spawn from.
    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomMinWidth, int roomMinLength)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if(orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, 
                Random.Range((bottomLeftAreaCorner.y + roomMinLength), 
                (topRightAreaCorner.y - roomMinLength)));
        }
        else
        {
            coordinates = new Vector2Int(Random.Range((bottomLeftAreaCorner.x + roomMinWidth), 
                (topRightAreaCorner.x - roomMinWidth)),
                0);
        }
        return coordinates;

    }
}