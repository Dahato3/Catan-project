using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Edge
{
    // Prefab gameobject of the road
    public GameObject road;

    // A given roads local conecting edges
    public Node node1;
    public Node node2;

    // Some properties of a road
    public int edgeColour;
    public int edgeBoardLocation;

    public bool checkedLongestRoad = false;

    public Edge(Node node1, Node node2)
    {
        this.node1 = node1;
        this.node2 = node2;
        edgeColour = 0;
    }

    // Various getters and setters for road properties
    public int getEdgeType()
    {
        return edgeColour;
    }

    public void setEdgeColour(int i)
    {
        edgeColour = i;
    }
    public Node getNode1()
    {
        return node1;
    }

    public Node getNode2()
    {
        return node2;
    }

    public void setRoad(GameObject g)
    {
        road = g;
    }

    public GameObject getRoad()
    {
        return road;
    }
}