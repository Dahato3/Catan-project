using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    public Board myboard = GameObject.Find("Board").GetComponent<Board>();

    public GameObject node;

    public int boardLocation;

    public Player player;

    public Button button;

    public Node nodeNorthSouth; // oHex
    public Node nodeEast; // rHex
    public Node nodeWest; //lHex

    public Edge edgeNorthSouth;
    public Edge edgeEast;
    public Edge edgeWest;

    int currentPosition;
    int currentWidth;
    int currentHeight;
    //int startIndex = 1;

    public int houseType; // 0 - none, 1 - settlement, 2 - city
    public int houseColour; // 1 - white, 2 - red, 3 - yellow, 4 - blue

    public int lHex;
    public String lHexResource;

    public int rHex;
    public String rHexResource;

    public int oHex;
    public String oHexResource;

    bool canPlace;

    public Node()
    {
        houseColour = 0;
        houseType = 0;

        //currentWidth = width;
        //currentHeight = height;

        //Debug.Log("Set Height " + currentHeight);
        //myboard = node.GetComponent<Board>();

        button = new Button();

    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    Debug.Log("TEST");
    //    Debug.Log("current width " + myboard.getCurrentWidth());
    //    Debug.Log("height " + myboard.getHeight());
    //    Debug.Log("index " + myboard.getLoopIndex());
        //CreateNode(myboard.getHeight(), myboard.getLoopIndex(), myboard.getCurrentWidth());

    //}
    //public void CreateNode(int height, int start, int width)
    //{
    //    //nodeVector = new Vector3(currentWidth - startIndex, 0, currentWidth);
    //    //Instantiate(node, new Vector3(currentWidth-startIndex, 0, currentWidth), Quaternion.identity);
    //    //myboard = FindObjectOfType<Board>();
    //    currentWidth = width;
    //    currentHeight = height;
    //    Debug.Log("Set Height " + currentHeight);
    //    SetNode(myboard.getLoopIndex());
    //}

    public void SetNode(int index)
    {
        myboard.checkWidth(index);
        // If index is on the bottom half of the board
        if (myboard.getCHeight() <= myboard.getHeight() / 2)
        {
            // Case where index is on the first row
            if (myboard.cHeight == 1)
            {
                nodeWest = myboard.getNode(index + myboard.getCWidth());
                nodeEast = myboard.getNode(index + myboard.getCWidth() + 1);
            }
            // Case where index is on the bottom half, an even height and in beween the first and last
            // index on that height
            else if (myboard.getCHeight() % 2 == 0 && index > myboard.sIndex && index < myboard.sIndex + myboard.cWidth - 1)
            {
                nodeWest = myboard.getNode(index - myboard.getCWidth());
                nodeEast = myboard.getNode(index - myboard.getCWidth() + 1);
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
            // Case where index is on the botton half of the board, an even height and is on the first node of that height
            else if (myboard.getCHeight() % 2 == 0 && index == myboard.sIndex)
            {
                nodeEast = myboard.getNode(index - (myboard.getCWidth() - 1));
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
            // Case where index is on the botton half of the board, an even height and is on the last node of that height
            else if (myboard.getCHeight() % 2 == 0 && index == myboard.sIndex + myboard.cWidth - 1)
            {
                nodeWest = myboard.getNode(index - myboard.getCWidth());
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
            // Case where index is on the bottom half of the board, an odd height
            else if (myboard.getCHeight() % 2 == 1)
            {
                nodeWest = myboard.getNode(index + myboard.getCWidth());
                nodeEast = myboard.getNode(index + myboard.getCWidth() + 1);
                nodeNorthSouth = myboard.getNode(index - myboard.getCWidth());
            }
            
        }
        else {
            // Case where index is on the top half of the board and is on the top row
            if (myboard.cHeight == myboard.getHeight())
            {
                nodeWest = myboard.getNode(index - myboard.getCWidth() - 1);
                nodeEast = myboard.getNode(index - myboard.getCWidth());
            }
            // Case where index is on the top half of the baord and inbetween the first and last node on that height
            else if (myboard.getCHeight() % 2 == 1 && index > myboard.sIndex && index < myboard.sIndex + myboard.cWidth - 1)
            {
                nodeWest = myboard.getNode(index + myboard.getCWidth() - 1);
                nodeEast = myboard.getNode(index + myboard.getCWidth());
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
            // Case where index is on the bottom half of the board, an odd height and is on the first node of that height
            else if (myboard.getCHeight() % 2 == 1 && index == myboard.sIndex)
            {
                nodeEast = myboard.getNode(index + myboard.getCWidth());
                nodeNorthSouth = myboard.getNode(index - myboard.getCWidth());
            }
            // Case where index is on the bottom hlaf of the board, an odd height and is on the last node of that height
            else if (myboard.getCHeight() % 2 == 1 && index == myboard.sIndex + myboard.cWidth - 1)
            {
                nodeWest = myboard.getNode(index + myboard.getCWidth() - 1);
                nodeNorthSouth = myboard.getNode(index - myboard.getCWidth());
            }
            // Case where index is on the top half of the board and is on an even height
            else if (myboard.getCWidth() % 2 == 0)
            {
                nodeWest = myboard.getNode(index - myboard.getCWidth() - 1);
                nodeEast = myboard.getNode(index - myboard.getCWidth());
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
        }
    }
    int edgeCounter = 0;
    public void setNodeEdge(int index)
    {
        myboard.checkWidth(index);
        if (myboard.getCHeight() <= myboard.getHeight() / 2)
        {
            if (myboard.getCHeight() % 2 == 1)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            else
            {
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
        }
        else
        {
            if (myboard.getCHeight() == myboard.getHeight() / 2 + 1)
            {

            }
            else if (myboard.getCHeight() % 2 == 0)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            else
            {
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
        }
    }

    public bool CanPlace()
    {
        return canPlace;
    }

    public void SetOHex(int val)
    {
        oHex = val;
    }

    public void SetRHex(int val)
    {
        rHex = val;
    }

    public void SetLHex(int val)
    {
        lHex = val;
    }
    public void SetOHexResource(string resource)
    {
        oHexResource = resource;
    }

    public void SetRHexResouce(string resource)
    {
        rHexResource = resource;
    }

    public void SetLHexResouce(string resource)
    {
        lHexResource = resource;
    }

    public int getHouseColour()
    {
        return houseColour;
    }
    public int getHouseType()
    {
        return houseType;
    }
    public Node getNodeEast()
    {
        return nodeEast;
    }
    public Node getNodeWest()
    {
        return nodeWest;
    }
    public Node getNodeNorthSouth()
    {
        return nodeNorthSouth;
    }
    public Edge getEdgeEast()
    {
        return edgeEast;
    }
    public Edge getEdgeWest()
    {
        return edgeWest;
    }
    public Edge getEdgeNorthSouth()
    {
        return edgeNorthSouth;
    }

}