using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    public Board myboard = GameObject.Find("Board").GetComponent<Board>();

    public GameObject settlementHex;
    public GameObject city;

    public int boardLocation;

    public Player player;

    public PlayerStateManager state;

    public Node nodeNorthSouth; // oHex
    public Node nodeEast; // rHex
    public Node nodeWest; //lHex

    public Edge edgeNorthSouth;
    public Edge edgeEast;
    public Edge edgeWest;

    public int houseType; // 0 - none, 1 - settlement, 2 - city
    public int houseColour; // 1 - white, 2 - red, 3 - yellow, 4 - blue

    static int edgeCounter = 0;

    public int lHexLocation;
    public int lHex;
    public String lHexResource;

    public int rHexLocation;
    public int rHex;
    public String rHexResource;

    public int oHexLocation;
    public int oHex;
    public String oHexResource;

    bool canPlace;

    public bool hasRobber;

    public Node()
    {
        houseColour = 0;
        houseType = 0;

        hasRobber = false;
    }

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
        else if (myboard.getCHeight() > myboard.getHeight() / 2)
        {
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
                nodeNorthSouth = myboard.getNode(index - myboard.getCWidth());
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
            else if (myboard.getCHeight() % 2 == 0)
            {
                nodeWest = myboard.getNode(index - myboard.getCWidth() - 1);
                nodeEast = myboard.getNode(index - myboard.getCWidth());
                nodeNorthSouth = myboard.getNode(index + myboard.getCWidth());
            }
        }
    }

    public void setNodeEdge(int index)
    {
        myboard.checkWidth(index);
        // If index is on the bottom half of the board
        if (myboard.getCHeight() <= myboard.getHeight() / 2)
        {
            // Case where index is on the first row
            if (myboard.cHeight == 1)
            {

                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the bottom half, an even height and in beween the first and last
            // index on that height
            else if (myboard.getCHeight() % 2 == 0 && index > myboard.sIndex && index < myboard.sIndex + myboard.cWidth - 1)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the botton half of the board, an even height and is on the first node of that height
            else if (myboard.getCHeight() % 2 == 0 && index == myboard.sIndex)
            {
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the botton half of the board, an even height and is on the last node of that height
            else if (myboard.getCHeight() % 2 == 0 && index == myboard.sIndex + myboard.cWidth - 1)
            { 
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the bottom half of the board, an odd height
            else if (myboard.getCHeight() % 2 == 1)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }

        }
        else if (myboard.getCHeight() > myboard.getHeight() / 2)
        {
            // Case where index is on the top half of the board and is on the top row
            if (myboard.cHeight == myboard.getHeight())
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the top half of the baord and inbetween the first and last node on that height
            else if (myboard.getCHeight() % 2 == 1 && index > myboard.sIndex && index < myboard.sIndex + myboard.cWidth - 1)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the bottom half of the board, an odd height and is on the first node of that height
            else if (myboard.getCHeight() % 2 == 1 && index == myboard.sIndex)
            {
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the bottom hlaf of the board, an odd height and is on the last node of that height
            else if (myboard.getCHeight() % 2 == 1 && index == myboard.sIndex + myboard.cWidth - 1)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
            // Case where index is on the top half of the board and is on an even height
            else if (myboard.getCHeight() % 2 == 0)
            {
                edgeWest = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeEast = myboard.getEdge(edgeCounter);
                edgeCounter++;
                edgeNorthSouth = myboard.getEdge(edgeCounter);
                edgeCounter++;
            }
        }
    }

    public bool CanPlace()
    {
        return canPlace;
    }
    public void SetOHexLocation(int val)
    {
        oHexLocation = val;
    }

    public void SetRHexLocation(int val)
    {
        rHexLocation = val;
    }

    public void SetLHexLocation(int val)
    {
        lHexLocation = val;
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
    public void setSettlementHex(GameObject g)
    {
        settlementHex = g;
    }
    public GameObject getSettlementHex()
    {
        return settlementHex;
    }
    public void setCity(GameObject g)
    {
        city = g;
    }

    public GameObject getCity()
    {
        return city;
    }

}
