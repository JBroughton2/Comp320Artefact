using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class Node
{
    /*
     * This is a custom class that will be used as the room nodes.
     * Therefore I have set up variables for the information that will be required in the other scripts.
     * There is also a couple simple list functions included as it will be usefun for managing the nodes children later.
     */

    private List<Node> childrenNodeList;

    public List<Node> ChildrenNodeList { get => childrenNodeList;}

    public bool Visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }

    public Node Parent { get; set; }
    public int TreeLayerIndex { get; set; }

    public Node(Node parentNode) 
    {
        childrenNodeList = new List<Node>();
        this.Parent = parentNode;
        if(parentNode != null) 
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        childrenNodeList.Add(node);
    }

    public void RemoveChild(Node node)
    {
        childrenNodeList.Remove(node);
    }
}