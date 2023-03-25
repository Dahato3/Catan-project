using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node
{
    public Board myboard = GameObject.Find("Board").GetComponent<Board>();

    public GameObject node;

    public Node nodeNorthSouth;
    public Node nodeEast;
    public Node nodeWest;
    int currentPosition;
    int currentWidth;
    int currentHeight;
    int startIndex = 1;
    public int houseType;
    public int houseColour;

    public int lHex;
    public String lHexResource;

    public int rHex;
    public String rHexResource;

    public int oHex;
    public String oHexResource;




    bool canPlace;

    public Node(int height, int start, int width)
    {
        currentWidth = width;
        currentHeight = height;
        //Debug.Log("Set Height " + currentHeight);
        //myboard = node.GetComponent<Board>();
        SetNode(myboard.getLoopIndex());
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
        if (index < (myboard.getWidth() / 2))
        {
            if (currentHeight <= 1)
            {

            }
            else
            {
                //Debug.Log("Current height" + currentHeight);
                if (index % 2 == 0)
                {
                    //Debug.Log(index);
                    //Debug.Log("index link" + (index - currentWidth));
                    nodeNorthSouth = myboard.getNode(index - currentWidth);
                }
                else
                {
                    nodeNorthSouth = myboard.getNode(index + currentWidth);
                }
            }
            
        }
        else
        {
            if(currentHeight >= 12) // Checking if it is the top node
            {

            }
            else
            {
                if (index % 2 == 0) // Noth Node
                {
                    //Debug.Log("current height" + currentHeight);
                    //Debug.Log("index" + index);
                    //Debug.Log("link index" + (index + currentWidth));
                    nodeNorthSouth = myboard.getNode(index + currentWidth);
                }
                else // South Node
                {
                    nodeNorthSouth = myboard.getNode(index - currentWidth);
                }
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
}
