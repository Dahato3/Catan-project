using JetBrains.Annotations;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject playerState;

    PlayerStateManager state;
    Player player;


    // Game objects to instantiate the board:

    // Game objects for each hexagon type
    [SerializeField] GameObject fieldHexPrefab;
    [SerializeField] GameObject lumbgerHexPrefab;
    [SerializeField] GameObject brickHexPrefab;
    [SerializeField] GameObject oreHexPrefab;
    [SerializeField] GameObject woolHexPrefab;
    [SerializeField] GameObject desertHexPrefab;

    //Game objects for each number
    [SerializeField] GameObject twoHexPrefab;
    [SerializeField] GameObject threeHexPrefab;
    [SerializeField] GameObject fourHexPrefab;
    [SerializeField] GameObject fiveHexPrefab;
    [SerializeField] GameObject sixHexPrefab;
    [SerializeField] GameObject eightHexPrefab;
    [SerializeField] GameObject nineHexPrefab;
    [SerializeField] GameObject tenHexPrefab;
    [SerializeField] GameObject elevenHexPrefab;
    [SerializeField] GameObject twelveHexPrefab;

    [SerializeField] GameObject settlementCityPrefab;
    [SerializeField] GameObject roadPrefab;

    // These variables are used to help setup the game (will reference them as the "base variables")

    // Width is the number of vertices on bottom row
    public int width;
    // Current width will change as we move up the board (during generation or setting of nodes / edges)
    public int cWidth;
    public int totalWidth;
    public int i;
    public int height;
    public int cHeight;
    public int sIndex;

    public bool introTurn;
    public bool placedIntroSettlements;
    public bool placedIntroRoads;

    // Arrays to help with generation
    public int[] valueSet;
    public string[] resourceSet;

    public int edgeNum;
    public Edge[] edgeList;

    public Node[] boardNodes;
    public Node node;

    void Awake()
    {
        state = playerState.GetComponent<PlayerStateManager>();
    }

    void Start()
    {
        // First intialization of the base variables
        width = 3;
        cWidth = width;
        totalWidth = width * width * 6;
        height = width * 4;
        cHeight = 1;
        sIndex = 0;

        introTurn = true;
        placedIntroSettlements = false;
        placedIntroRoads = false;

        //edgeNum = 7 * width * width + 13 * width - 32;
        edgeList = new Edge[72];
        boardNodes = new Node[54];

        // Here we initialize our resource array and value array that will hod the types / values of the hexagons / numbers
        resourceSet = new string[18] { "wool", "wool", "wool", "wool", "lumber", "lumber", "lumber", "lumber", "grain", "grain", "grain", "grain", "brick", "brick", "brick", "ore", "ore", "ore" };
        if (width == 3)
        {
            valueSet = new int[18] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
        }
        else
        {
            valueSet = new int[3 * width * width - 3 * width + 1];
            for (int k = 0; k <= valueSet.Length; k++)
            {
                valueSet[k] = Random.Range(2, 13);
            }

        }

        GenerateBoard();
        assignHex();

        resetIntialvariables();

        // Loops through the node array and will call a node method on each node to assign their neighbouring nodes
        // similar to assigning the hexagons to nodes (either: nodeWest - left most node, nodeEast - right most node, nodeNorthSouth - bottom
        // most or top most node)
        for (int q = 0; q < totalWidth; q++)
        {
            boardNodes[q].SetNode(q);
            boardNodes[q].boardLocation = q;

        }
        for (int w = 0; w < totalWidth; w++)
        {
            boardNodes[w].setNodeEdge(w);
        }

        resetIntialvariables();

        setBoardEdges();

        resetIntialvariables();

        for (int i = 0; i < totalWidth; i++)
        {
            GameObject temp = GameObject.FindWithTag("settlementCity");
            temp.SetActive(false);
            temp.GetComponent<Button>().onClick.AddListener(delegate { player.buildSettlementCity(i); });

        }
        

        // Pop up to say player 1 build house
        Debug.Log("Player 1 please build a settlement");


        while (placedIntroSettlements && placedIntroRoads)
        {

        }

        introTurn = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    // generate board will fill up the node array with a new node at each position
    void GenerateBoard()
    {
        for (int j = 0; j < boardNodes.Length; j++)
        {
            boardNodes[j] = new Node();   
        } 
    }

    // These varaiables are used in asign hex and takes a new value each iteration of the loop
    public int setValue;
    public string setResource;

    // Assign hex will loop through every node we set up previously and depending on where it's located on the board, will assign
    // it's corrosponding hexagon (either Ohex - up or down, Rhex - on the right side, Lhex - on the left side)
    public void assignHex()
    {
        for (int m = 0; m < boardNodes.Length; m++)
        {
            // Special case where the node (m) is on the centre hexagon, we dont need to assign any hexagon here
            if (cHeight == height / 2 - 1 && m == (sIndex + ((cWidth + 1) / 2)))
            {
                asignHexPositions(m);
                assignNumberPositions(m);
            }
            else
            {
                checkWidth(m);

                // Node (m) is on an odd height, current height is less than the total height / 2 (bottom half of the board)
                // and m hasn't exceded the final node on that height
                if (cHeight % 2 == 1 && cHeight < height / 2 && m <= sIndex + cWidth - 1)
                {
                    setValue = getVal();
                    setResource = getResource();
                    for (int w = 0; w < 5; w++)
                    {
                        if (w == 0)
                        {
                            boardNodes[m].SetOHex(setValue);
                            boardNodes[m].SetOHexResource(setResource);
                        }
                        if (w == 1)
                        {
                            boardNodes[m + cWidth].SetRHex(setValue);
                            boardNodes[m + cWidth].SetRHexResouce(setResource);
                        }
                        if (w == 2)
                        {
                            boardNodes[m + cWidth + 1].SetLHex(setValue);
                            boardNodes[m + cWidth + 1].SetLHexResouce(setResource);
                        }
                        if (w == 3)
                        {
                            boardNodes[m + (2 * cWidth) + 1].SetRHex(setValue);
                            boardNodes[m + (2 * cWidth) + 1].SetRHexResouce(setResource);
                        }
                        if (w == 4)
                        {
                            boardNodes[m + (2 * cWidth) + 2].SetLHex(setValue);
                            boardNodes[m + (2 * cWidth) + 2].SetLHexResouce(setResource);
                        }
                        if (w == 5)
                        {
                            boardNodes[m + (3 * cWidth) + 3].SetOHex(setValue);
                            boardNodes[m + (3 * cWidth) + 3].SetOHexResource(setResource);
                        }  
                    }
                    asignHexPositions(m);
                    assignNumberPositions(m);
                }
                // Case where the current height is greater than total height / 2 (top half of then board) and m is
                // on an even height
                else if (cHeight > height / 2 + 2 && cHeight % 2 == 0)
                {
                    setValue = getVal();
                    setResource = getResource();

                    for (int w = 0; w < 5; w++)
                    {
                        if (w == 0)
                        {
                            boardNodes[m].SetOHex(setValue);
                            boardNodes[m].SetOHexResource(setResource);
                        }
                        if (w == 1)
                        {
                            boardNodes[m - cWidth].SetLHex(setValue);
                            boardNodes[m - cWidth].SetLHexResouce(setResource);
                        }
                        if (w == 2)
                        {
                            boardNodes[m - cWidth - 1].SetRHex(setValue);
                            boardNodes[m - cWidth - 1].SetRHexResouce(setResource);
                        }
                        if (w == 3)
                        {
                            boardNodes[m - (2 * cWidth) - 1].SetLHex(setValue);
                            boardNodes[m - (2 * cWidth) - 1].SetLHexResouce(setResource);
                        }
                        if (w == 4)
                        {
                            boardNodes[m - (2 * cWidth) - 2].SetRHex(setValue);
                            boardNodes[m - (2 * cWidth) - 2].SetRHexResouce(setResource);
                        }
                        if (w == 5)
                        {
                            boardNodes[m - (3 * cWidth) - 3].SetOHex(setValue);
                            boardNodes[m - (3 * cWidth) - 3].SetOHexResource(setResource);
                        }
                    }
                    asignHexPositions(m);
                    assignNumberPositions(m);
                }
            }

        }
    }

    // This method is called throughout various methods, will take the node postion and calculate the height and width of that node position
    // to be used in the corrosponding method
    public void checkWidth(int nodePosition)
    {
        if (nodePosition <= 26)
        {
            if (sIndex + cWidth == nodePosition)
            {
                cHeight++;
                sIndex = sIndex + cWidth;
                if (cHeight % 2 == 0)
                {
                    cWidth++;
                }
            }
        }
        else
        {
            if (sIndex + cWidth == nodePosition)
            {
                cHeight++;
                sIndex = sIndex + cWidth;
                if (cHeight % 2 == 0)
                {
                   cWidth--;
                   
                }
            }
        }
    }

    // Two methods to choose either our resources form the hexagons or the numbers for hexagons at random


    // For the vlaues, we take a random position from the value array, if it's 0 we choose another, if not, we set
    // that position to 0 and return the value at the position previously
    public int getVal()
    {
        int randInt = Random.Range(0, 18);
        int setVal = valueSet[randInt];
        while (setVal == 0)
        {
            randInt = Random.Range(0, 18);
            setVal = valueSet[randInt];
        }
        valueSet[randInt] = 0;
        return setVal;
    }

    // Acts similar to the value method above but will check is the position is an empty string ("") and if not will
    // set it to an empty string and return the string at the position previously
    public string getResource()
    {
        int randInt = Random.Range(0, 18);
        string setResource = resourceSet[randInt];
        while (setResource == "")
        {
            randInt = Random.Range(0, 18);
            setResource = resourceSet[randInt];
        }
        if (width == 3)
        {
            resourceSet[randInt] = "";
        }
        return setResource;
    }

    // This method will once again loop through every node and create an edge between it's neighbouring nodes that we assigned earlier on. The edge
    // added to an edge array so we can use them later on
    public void setBoardEdges()
    {
        int edgeCounter = 0;
        for (int k = 0; k < boardNodes.Length; k++)
        {
            checkWidth(k);
            if (cHeight <= height / 2)
            {
                if (cHeight % 2 == 1)
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeWest());
                    edgeCounter++;
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeCounter++;
                }
                else
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeCounter++;
                }
            }
            else
            {
                if (cHeight == height / 2 + 1)
                {

                }
                else if (cHeight % 2 == 0)
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeWest());
                    edgeCounter++;
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeCounter++;
                }
                else
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeCounter++;
                }
            }
        }
        
    }


    // The next two methods are what create the hexagons / number game object (from the prefab in unity) and position it in the correct
    // position. Alot of the code is the same but we needed to have a case for every combination of hexagon and number location

    // This method does the hexes, will obtain the current node position, the current resource and instantiate it in the corrosponding
    // location that links to the node postion
    public void asignHexPositions(int nodeIndex)
    {
        Debug.Log(nodeIndex);
        GameObject temp = instantiateHex(setResource);
        if (nodeIndex == 0)
        {
            temp.transform.position = new Vector3(12.5f, 0, -150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 1) 
        {
            temp.transform.position = new Vector3(100, 0, -150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 2)
        {
            temp.transform.position = new Vector3(186, 0, -150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 7)
        {
            temp.transform.position = new Vector3(-32, 0, -75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 8)
        {
            temp.transform.position = new Vector3(55.5f, 0, -75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 9)
        {
            temp.transform.position = new Vector3(143, 0, -75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 10)
        {
            temp.transform.position = new Vector3(230.5f, 0, -75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 16)
        {
            temp.transform.position = new Vector3(-75.5f, 0, 0);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 17)
        {
            temp.transform.position = new Vector3(12, 0, 0);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 18)
        {
            temp = Instantiate(desertHexPrefab, new Vector3(100, 0, 0), transform.rotation);
            temp.transform.localScale = new Vector3(50, 50, 50);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        }
        else if (nodeIndex == 19)
        {
            temp.transform.position = new Vector3(186, 0, 0);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 20)
        {
            temp.transform.position = new Vector3(274, 0, 0);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp6 = Instantiate(settlementCityPrefab, temp.transform);
            temp6.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp6.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp7 = Instantiate(settlementCityPrefab, temp.transform);
            temp7.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            temp7.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 43)
        {
            temp.transform.position = new Vector3(-32, 0, 75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 44)
        {
            temp.transform.position = new Vector3(55.5f, 0, 75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        }
        else if (nodeIndex == 45)
        {
            temp.transform.position = new Vector3(143, 0, 75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 46)
        {
            temp.transform.position = new Vector3(230, 0, 75);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 51)
        {
            temp.transform.position = new Vector3(12.5f, 0, 150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 52)
        {
            temp.transform.position = new Vector3(100, 0, 150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else if (nodeIndex == 53)
        {
            temp.transform.position = new Vector3(186, 0, 150);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            
        }
        
    }


    // This method will do a simiar operation as the method above but will instead instantite the number and postion
    // it in the correct location. We also need to flip the obk
    public void assignNumberPositions(int nodePosition)
    {
        GameObject temp = instantiateHexNumber(setValue);
        if (nodePosition == 0)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, -150f);
        }
        else if (nodePosition == 1)
        {
            temp.transform.position = new Vector3(0, 10.5f, -150f);
        }
        else if (nodePosition == 2)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, -150f);
        }
        else if (nodePosition == 7)
        {
            temp.transform.position = new Vector3(-130, 10.5f, -75);
        }
        else if (nodePosition == 8)
        {
            temp.transform.position = new Vector3(-42.5f, 10.5f, -75);
        }
        else if (nodePosition == 9)
        {
            temp.transform.position = new Vector3(45, 10.5f, -75);
        }
        else if (nodePosition == 10)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, -75);
        }
        else if (nodePosition == 16)
        {
            temp.transform.position = new Vector3(-175, 10.5f, 0);
        }
        else if (nodePosition == 17)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 0);
        }
        else if (nodePosition == 18)
        {

        }
        else if (nodePosition == 19)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, 0);
        }
        else if (nodePosition == 20)
        {
            temp.transform.position = new Vector3(175, 10.5f, 0);
        }
        else if (nodePosition == 43)
        {
            temp.transform.position = new Vector3(-130, 10.5f, 75);
        }
        else if (nodePosition == 44)
        {
            temp.transform.position = new Vector3(-42.5f, 10.5f, 75);
        }
        else if (nodePosition == 45)
        {
            temp.transform.position = new Vector3(45, 10.5f, 75);
        }
        else if (nodePosition == 46)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, 75);
        }
        if (nodePosition == 51)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 150f);
        }
        else if (nodePosition == 52)
        {
            temp.transform.position = new Vector3(0, 10.5f, 150f);
        }
        else if (nodePosition == 53)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, 150f);
        }
    }

    public GameObject instantiateHex(string resource)
    {
        GameObject temp = null;
        if (resource == "lumber")
        {
            temp = Instantiate(lumbgerHexPrefab);
        }
        else if (resource == "grain")
        {
            temp = Instantiate(fieldHexPrefab);
        }
        else if (resource == "brick")
        {
            temp = Instantiate(brickHexPrefab);
        }
        else if (resource == "ore")
        {
            temp = Instantiate(oreHexPrefab);
        }
        else if (resource == "wool")
        {
            temp = Instantiate(woolHexPrefab);
        }
        temp.transform.localScale = new Vector3(50, 50, 50);
        return temp;
    }

    public GameObject instantiateHexNumber(int value)
    {
        GameObject temp = null;
        if (value == 2)
        {
            temp = Instantiate(twoHexPrefab);
        }
        else if (value == 3)
        {
            temp = Instantiate(threeHexPrefab);
        }
        else if (value == 4)
        {
            temp = Instantiate(fourHexPrefab);
        }
        else if (value == 5)
        {
            temp = Instantiate(fiveHexPrefab);
        }
        else if (value == 6)
        {
            temp = Instantiate(sixHexPrefab);
        }
        else if (value == 8)
        {
            temp = Instantiate(eightHexPrefab);
        }
        else if (value == 9)
        {
            temp = Instantiate(nineHexPrefab);
        }
        else if (value == 10)
        {
            temp = Instantiate(tenHexPrefab);
        }
        else if (value == 11)
        {
            temp = Instantiate(elevenHexPrefab);
        }
        else if (value == 12)
        {
            temp = Instantiate(twelveHexPrefab);
        }
        temp.transform.localScale = new Vector3(17.5f, 17.5f, 17.5f);
        temp.transform.Rotate(0f, 180f, 0);
        return temp;
    }

    // This method will reset the initial variables, was needed as checkWidth() updates them and we need them
    // reset for when checkWidth() is used again
    public void resetIntialvariables()
    {
        width = 3;
        cWidth = width;
        totalWidth = width * width * 6;
        height = width * 4;
        cHeight = 1;
        sIndex = 0;
    }

    // Getters to help with other methods
    public Node getNode(int n)
    {
        return boardNodes[n];
    }
    public Edge getEdge(int n)
    {
        return edgeList[n];
    }
    public int getWidth()
    {
        return totalWidth;
    }

    public int getCWidth()
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
    public int getCHeight()
    {
        return cHeight;
    }



    // METHOD CREATED TO CHECK WHICH HEXES A GIVEN NODE HAD ACCESS TO - NOT USED

    //public string isHex(int index)
    //{
    //    // If the index is on the bottom half of the board
    //    if (index <= 26)
    //    {
    //        // Special case - bottom most height, only OHex
    //        if (cWidth == 3)
    //        {
    //            return "O";
    //        }
    //        // Special case - one layer above the bottom most height
    //        // ALSO in between the start and end nodes of that height
    //        // only lHex and Rhex
    //        if (height == 2 && (sIndex < index && index < ((sIndex + cWidth) - 1)) || height == 11 && (sIndex < index && index < ((sIndex + cWidth) - 1)))
    //        {
    //            return "RL";
    //        }
    //        // If the index is on the bottom half and an odd height
    //        if (height % 2 == 1)
    //        {
    //            // Special case - has the centre hexagon at a surrounding
    //            // position, we don't assign this hex
    //            if (height == 5 && index == (sIndex + cWidth - 3))
    //            {
    //                return "LR";
    //            }
    //            // First node on bottom half of the board, odd height
    //            if (index == sIndex)
    //            {
    //                return "RO";
    //            }
    //            // Last node on bottom half of the board, odd height
    //            else if (index == (sIndex + cWidth) - 1)
    //            {
    //                return "LO";
    //            }
    //            // In between start node and last node on bottom half of the board, odd height
    //            else if (sIndex < index && index < ((sIndex + cWidth) - 1))
    //            {
    //                return "ROL";
    //            }
    //        }
    //        // If the index is on the bottom half and an even height
    //        else
    //        {
    //            // Two special cases - has the centre hexagon at a surrounding
    //            // position, we don't assign this hex
    //            if (height == 6 && index == (sIndex + cWidth - 4))
    //            {
    //                return "LO";
    //            }
    //            if (height == 6 && index == (sIndex + cWidth - 3))
    //            {
    //                return "RO";
    //            }

    //            // First node on bottom half of the board, even height
    //            if (index == sIndex)
    //            {
    //                return "R";
    //            }
    //            // Last node on bottom half of the board, even height
    //            else if (index == (sIndex + cWidth) - 1)
    //            {
    //                return "L";
    //            }
    //            // In between start node and last node on bottom half of the board, even height
    //            else if (sIndex < index && index < ((sIndex + cWidth) - 1))
    //            {
    //                return "ROL";
    //            }
    //        }
    //    }

    //    // If the index is on the top half of the board
    //    else
    //    {
    //        // Special case - top most height, only OHex
    //        if (cWidth == 3)
    //        {
    //            return "O";
    //        }
    //        // Special case - one layer below the top most height
    //        // ALSO in between the start and end nodes of that height
    //        // only lHex and Rhex
    //        if (height == 11 && (sIndex < index && index < ((sIndex + cWidth) - 1)) || height == 11 && (sIndex < index && index < ((sIndex + cWidth) - 1)))
    //        {
    //            return "RL";
    //        }
    //        // If the index is on the top half and an even height
    //        if (height % 2 == 0)
    //        {
    //            // Special case - has the centre hexagon at a surrounding
    //            // position, we don't assign this hex
    //            if (height == 8 && index == (sIndex + cWidth - 3))
    //            {
    //                return "LR";
    //            }
    //            // First node on top half of the board, even height
    //            if (index == sIndex)
    //            {
    //                return "RO";
    //            }
    //            // Last node on bottom half of the board, even height
    //            else if (index == (sIndex + cWidth) - 1)
    //            {
    //                return "LO";
    //            }
    //            // In between start node and last node on top half of the board, even height
    //            else if (sIndex < index && index < ((sIndex + cWidth) - 1))
    //            {
    //                return "ROL";
    //            }
    //        }
    //        else
    //        {
    //            // Two special cases - has the centre hexagon at a surrounding
    //            // position, we don't assign this hex
    //            if (height == 7 && index == (sIndex + cWidth - 4))
    //            {
    //                return "LO";
    //            }
    //            if (height == 7 && index == (sIndex + cWidth - 3))
    //            {
    //                return "RO";
    //            }

    //            // First node on top half of the board, odd height
    //            if (index == sIndex)
    //            {
    //                return "R";
    //            }
    //            // Last node on top half of the board, odd height
    //            else if (index == (sIndex + cWidth) - 1)
    //            {
    //                return "L";
    //            }
    //            // In between start node and last node on top half of the board, odd height
    //            else if (sIndex < index && index < ((sIndex + cWidth) - 1))
    //            {
    //                return "ROL";
    //            }
    //        }
    //    }
    //    return null;
    //}



    // OLD ASSIGN HEX METHOD

    //void AssignHex()
    //{
    //    //creating the values for the hexagon's
    //    int[] valueSet;
    //    string[] resourceSet;
    //    valueSet = new int[18] { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
    //    resourceSet = new string[18] { "wool", "wool", "wool", "wool", "lumber", "lumber", "lumber", "lumber", "grain", "grain", "grain", "grain", "brick", "brick", "brick", "ore", "ore", "ore" };
    //    //assigning the values to the correct node and the correct hexagon
    //    for (int n = 0; n < boardNodes.Length - 1; n++)
    //    { 
    //        // 0-2
    //        if (n <= 2)
    //        {
    //            cWidth = 3;
    //        }
    //        // 3-10
    //        else if (n >= 3 && n <= 10)
    //        {
    //            cWidth = 4;
    //        }
    //        // 11-20
    //        else if (n >= 11 && n <= 20)
    //        {
    //            cWidth = 5;
    //        }
    //        // 21-32
    //        else if (n >= 21 && n <= 32)
    //        {
    //            cWidth = 6;
    //        }
    //        // 33-42
    //        else if (n >= 33 && n <= 42)
    //        {
    //            cWidth = -5;
    //        }
    //        // 43 - 50
    //        else if (n >= 43 && n <= 50) 
    //        {
    //            cWidth = -4;
    //        }
    //        // 51 - 53
    //        else if (n >= 51 && n <= 53)
    //        {
    //            cWidth = -3;
    //        }



    //        //(3 * width * width) - (3 * width)

    //        tempWidth = cWidth;
    //        //checkWidth();

    //        int setValue = valueSet[Random.Range(0, 17)];
    //        if (setValue == 0)
    //        {
    //            while (setValue == 0)
    //            {
    //                setValue = valueSet[Random.Range(0, 17)];
    //            }

    //        }
    //        int tempV = valueSet[setValue];
    //        valueSet[tempV] = 0;

    //        string setResource = resourceSet[Random.Range(0, 17)];
    //        if (setResource == "USED")
    //        {
    //            while (setResource == "USED")
    //            {
    //                setResource = resourceSet[Random.Range(0, 17)];
    //            }
    //        }
    //        int tempR = valueSet[setValue];
    //        resourceSet[tempR] = "USED";

    //        for (int m = 0; m < 5; m++)
    //        {

    //            // 0, 1, 2    53, 52, 51 = only have an 0 hex (Width = 3)
    //            // 


    //            if(m == 0){
    //                boardNodes[n].SetOHex(setValue);
    //                boardNodes[n].SetOHexResource(setResource);
    //            }
    //            if(m == 1)
    //            {
    //                boardNodes[n + tempWidth].SetRHex(setValue);
    //                boardNodes[n + tempWidth].SetRHexResouce(setResource);
    //            }
    //            if(m == 2)
    //            {
    //                boardNodes[n + tempWidth + 1].SetLHex(setValue);
    //                boardNodes[n + tempWidth + 1].SetLHexResouce(setResource);
    //            }
    //            if(m == 3)
    //            {
    //                boardNodes[n + (2 * tempWidth) + 1].SetRHex(setValue);
    //                boardNodes[n + (2 * tempWidth) + 1].SetRHexResouce(setResource);
    //            }
    //            if(m == 4)
    //            {
    //                boardNodes[n + (2 * tempWidth) + 2].SetLHex(setValue);
    //                boardNodes[n + (2 * tempWidth) + 2].SetLHexResouce(setResource);
    //            }
    //            if(m == 5)
    //            {
    //                boardNodes[n + (3 * tempWidth) + 3].SetOHex(setValue);
    //                boardNodes[n + (3 * tempWidth) + 3].SetOHexResource(setResource);
    //            }
    //        }
    //    }
    //}


}
