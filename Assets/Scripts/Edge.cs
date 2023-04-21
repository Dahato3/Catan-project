using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{

    public GameObject road;

    public int edgeColour;

    public int edgeBoardLocation;

    public Node node1;
    public Node node2;

    public Edge(Node node1, Node node2)
    {
        this.node1 = node1;
        this.node2 = node2;
        edgeColour = 0;
    }

    //public void setEdgeNodes(int index)
    //{
    //    myboard.checkWidth(index);
    //    if (myboard.getCHeight() <= myboard.getHeight() / 2)
    //    {
    //        if (myboard.getCHeight() % 2 == 1)
    //        {
    //            node1 = myboard.boardNodes[index + myboard.cWidth];
    //            node2 = myboard.boardNodes[index + myboard.cWidth + 1];
    //        }
    //        else
    //        {
    //            edgeNorthSouth = myboard.getEdge(edgeCounter);
    //        }
    //    }
    //    else
    //    {
    //        if (myboard.getCHeight() == myboard.getHeight() / 2 + 1)
    //        {

    //        }
    //        else if (myboard.getCHeight() % 2 == 0)
    //        {
    //            edgeWest = myboard.getEdge(edgeCounter);
    //            edgeCounter++;
    //            edgeEast = myboard.getEdge(edgeCounter);
    //            edgeCounter++;
    //        }
    //        else
    //        {
    //            edgeNorthSouth = myboard.getEdge(edgeCounter);
    //            edgeCounter++;
    //        }
    //    }
    //}

    public int getEdgeType()
    {
        return edgeColour;
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