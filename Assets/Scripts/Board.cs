using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Board : MonoBehaviour
{
    // width is the number of vertices on bottom row
    public int width;
    public int cWidth;
    public int totalWidth;
    public Node[] boardNodes;
    public Node node;
    public int i;
    public int test = 5;
    public int height;
    void Start()
    {
        boardNodes = new Node[56];
        GenerateBoard();
        AssignHex();
        //Debug.Log("rhex: " + boardNodes[2].oHex);
        //Debug.Log("rhexResource: " + boardNodes[2].oHexResource);
        //Debug.Log("lhex: " + boardNodes[1].oHex);
        //Debug.Log("lhexResource: " + boardNodes[1].oHexResource);
        Debug.Log("ohex: " + boardNodes[0].oHex);
        Debug.Log("ohexResource: " + boardNodes[0].oHexResource);

        Debug.Log("");

        Debug.Log("lhex: " + boardNodes[4].lHex);
        Debug.Log("lhexResource: " + boardNodes[4].lHexResource);
        Debug.Log("rhex: " + boardNodes[4].rHex);
        Debug.Log("rhexResource: " + boardNodes[4].rHexResource);

        Debug.Log("");

        Debug.Log("ohex: " + boardNodes[7].oHex);
        Debug.Log("rhex: " + boardNodes[7].rHex);
        Debug.Log("ohexResource: " + boardNodes[7].oHexResource);
        Debug.Log("rhexResource: " + boardNodes[7].rHexResource);

        Debug.Log("");

        Debug.Log("ohex: " + boardNodes[8].oHex);
        Debug.Log("ohexResource: " + boardNodes[8].oHexResource);
        Debug.Log("lhex: " + boardNodes[8].lHex);
        Debug.Log("lhexResource: " + boardNodes[8].lHexResource);
        Debug.Log("rhex: " + boardNodes[8].rHex);
        Debug.Log("rhexResource: " + boardNodes[8].rHexResource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void GenerateBoard()
    {
        int height = 1;
        int cWidth = width;
        int sIndex = 0;
        //totalWidth = width * width * 6;
        totalWidth = 56;
        //board = new Node[totalWidth];
        //board[0] = new Node(height, sIndex, cWidth);
        for (int j = 0; j < totalWidth; j++)
        {
            boardNodes[j] = new Node(height, sIndex, cWidth);
            //board[j].CreateNode(height, sIndex, cWidth);
        }
        //Debug.Log("Node array length: " + boardNodes.Length);
        // check that the current index has gotten to the end of the board
        // increases or deacreases the board if it is the top or bottom half of the board and if the height is odd
        for (int i = 0; i < totalWidth; i++)
        {
            if (sIndex + cWidth == i)
            {
                height++;
                sIndex = +cWidth;
                if (height % 2 == 1)
                {
                    if (height < (width * 4) / 2)
                    {
                        cWidth++;
                    }
                    else
                    {
                        cWidth--;
                    }
                }
            }
        }
    }

    public Node getNode(int n)
    {
        return boardNodes[n];
    }

    public int getWidth()
    {
        return totalWidth;
    }

    public int getCurrentWidth()
    {
        return cWidth;
    }

    public int getLoopIndex()
    {
        return i;
    }
    public int getHeight()
    {
        return height;
    }

    void AssignHex()
    {
        //creating the values for the hexagon's
        int[] valueSet;
        string[] resourceSet;
        valueSet = new int[18] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
        resourceSet = new string[18] { "wool", "wool", "wool", "wool", "lumber", "lumber", "lumber", "lumber", "grain", "grain", "grain", "grain", "brick", "brick", "brick", "ore", "ore", "ore" };
        //assigning the values to the correct node and the correct hexagon
        for(int n = 0; n < boardNodes.Length; n++)
            //(3 * width * width) - (3 * width)
        {
            int setValue = valueSet[Random.Range(0, 17)];
            string setResource = resourceSet[Random.Range(0, 17)];

            for (int m = 0; m < 5; m++)
            {
                if(m == 0){
                    boardNodes[n].SetOHex(setValue);
                    boardNodes[n].SetOHexResource(setResource);
                }
                if(m == 1)
                {
                    boardNodes[n + cWidth].SetRHex(setValue);
                    boardNodes[n + cWidth].SetOHexResource(setResource);
                }
                if(m == 2)
                {
                    boardNodes[n + cWidth + 1].SetLHex(setValue);
                    boardNodes[n + cWidth + 1].SetOHexResource(setResource);
                }
                if(m == 3)
                {
                    boardNodes[n + (2 * cWidth) + 1].SetRHex(setValue);
                    boardNodes[n + (2 * cWidth) + 1].SetOHexResource(setResource);
                }
                if(m == 4)
                {
                    boardNodes[n + (2 * cWidth) + 2].SetLHex(setValue);
                    boardNodes[n + (2 * cWidth) + 2].SetOHexResource(setResource);
                }
                if(m == 5)
                {
                    boardNodes[n + (3 * cWidth) + 3].SetOHex(setValue);
                    boardNodes[n + (3 * cWidth) + 3].SetOHexResource(setResource);
                }
            }
        }
    }
}
