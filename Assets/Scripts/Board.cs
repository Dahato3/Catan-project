using JetBrains.Annotations;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] GameObject playerState;
    [SerializeField] GameObject theInitialTradePanel;
    [SerializeField] GameObject receivedTradePanel;
    [SerializeField] GameObject halfResourcePanel;

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

    public GameObject robber;

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

    public int introCounter = 0;

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

    public string[] developmentCards;

    public GameObject[] settlementCities;

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

        developmentCards = new string[25] { "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "road", "road", "yearofplenty", "yearofplenty", "monopoly", "monopoly", "university", "market", "greathall", "chapel", "library" };

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

        assignHex();

        resetIntialvariables();

        setLocalHexes();

        resetIntialvariables();


        // Loops through the node array and will call a node method on each node to assign their neighbouring nodes
        // similar to assigning the hexagons to nodes (either: nodeWest - left most node, nodeEast - right most node, nodeNorthSouth - bottom
        // most or top most node)
        
        

        
       
        GameObject.Find("EndTurn").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("PlayerTrade").GetComponent<CanvasGroup>().alpha = 0f;

        GameObject.Find("Timer").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("Resources").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("RollDiceButton").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("End Turn Button").GetComponent<CanvasGroup>().alpha = 0f;

        GameObject.Find("CurrentPlayer").transform.position = new Vector3(200, 300, 0);




        // Pop up to say player 1 build house
        Debug.Log("Player 1 please build a settlement");



        //for (i = 0; i < boardNodes.Length; i++)
        //{
        //    Debug.Log("i: " + i);
        //    Debug.Log("rHex: " + boardNodes[i].rHex);
        //    Debug.Log("lHex: " + boardNodes[i].lHex);
        //    Debug.Log("oHex: " + boardNodes[i].oHex);
        //}


        receivedTradePanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (theInitialTradePanel.activeInHierarchy == false
            && receivedTradePanel.activeInHierarchy == false
            && GameObject.Find("RollDiceButton").GetComponent<Button>().interactable == false)
        {
            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;
        }
        else if (theInitialTradePanel.activeInHierarchy == true
            || receivedTradePanel.activeInHierarchy == true
            || halfResourcePanel.activeInHierarchy == true)
        {
            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;
        }
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
            if (cHeight == height / 2 - 1 && m == (sIndex + ((cWidth ) / 2)))
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
                    Debug.Log("M: " + m);
                    setValue = getVal();
                    setResource = getResource();
                    for (int w = 0; w <= 5; w++)
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
                    Debug.Log("M: " + m);
                    setValue = getVal();
                    setResource = getResource();

                    for (int w = 0; w <= 5; w++)
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
    public void checkWidthOfParticularNode(int nodePosition)
    {
        if (nodePosition <= 2)
        {
            cWidth = 3;
        }
        // 3-10
        else if (nodePosition >= 3 && nodePosition <= 10)
        {
            cWidth = 4;
        }
        // 11-20
        else if (nodePosition >= 11 && nodePosition <= 20)
        {
            cWidth = 5;
        }
        // 21-32
        else if (nodePosition >= 21 && nodePosition <= 32)
        {
            cWidth = 6;
        }
        // 33-42
        else if (nodePosition >= 33 && nodePosition <= 42)
        {
            cWidth = 5;
        }
        // 43 - 50
        else if (nodePosition >= 43 && nodePosition <= 50)
        {
            cWidth = 4;
        }
        // 51 - 53
        else if (nodePosition >= 51 && nodePosition <= 53)
        {
            cWidth = 3;
        }
    }
    public void checkHeightOfParticularNode(int nodePosition)
    {
        if (nodePosition <= 2)
        {
            cHeight = 1;
            sIndex = 0;
        }
        // 3-10
        else if (nodePosition >= 3 && nodePosition <= 6)
        {
            cHeight = 2;
            sIndex = 3;
        }
        // 11-20
        else if (nodePosition >= 7 && nodePosition <= 10)
        {
            cHeight = 3;
            sIndex = 7;
        }
        // 21-32
        else if (nodePosition >= 11 && nodePosition <= 15)
        {
            cHeight = 4;
            sIndex = 11;
        }
        // 33-42
        else if (nodePosition >= 16 && nodePosition <= 20)
        {
            cHeight = 5;
            sIndex = 16;
        }
        // 43 - 50
        else if (nodePosition >= 21 && nodePosition <= 26)
        {
            cHeight = 6;
            sIndex = 21;
        }
        // 51 - 53
        else if (nodePosition >= 27 && nodePosition <= 32)
        {
            cHeight = 7;
            sIndex = 27;
        }
        else if (nodePosition >= 33 && nodePosition <= 37)
        {
            cHeight = 8;
            sIndex = 33;
        }
        else if (nodePosition >= 38 && nodePosition <= 42)
        {
            cHeight = 9;
            sIndex = 38;
        }
        else if (nodePosition >= 43 && nodePosition <= 46)
        {
            cHeight = 10;
            sIndex = 43;
        }
        else if (nodePosition >= 47 && nodePosition <= 50)
        {
            cHeight = 11;
            sIndex = 47;
        }
        else if (nodePosition >= 51 && nodePosition <= 53)
        {
            cHeight = 12;
            sIndex = 51;
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
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
                    edgeCounter++;
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
                    edgeCounter++;
                }
                else
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
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
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
                    edgeCounter++;
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
                    edgeCounter++;
                }
                else
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;
                    edgeCounter++;
                }
            }
        }
        
    }


    public void placeRobber()
    {

    }


    // The next two methods are what create the hexagons / number game object (from the prefab in unity) and position it in the correct
    // position. Alot of the code is the same but we needed to have a case for every combination of hexagon and number location

    // This method does the hexes, will obtain the current node position, the current resource and instantiate it in the corrosponding
    // location that links to the node postion
    public void asignHexPositions(int nodeIndex)
    {
        GameObject temp = null;
        if (nodeIndex != 18)
        {
            temp = instantiateHex(setResource);
        }
        if (nodeIndex == 0)
        {
            temp.transform.position = new Vector3(12.5f, 0, -150);
            //temp.transform.name = temp.name + "0";
            temp.transform.name = "1";
            temp.AddComponent<SphereCollider>().center = new Vector3(-2,0,0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[0].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[3].setSettlementHex(temp3);


            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[0].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[6].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[1].setRoad(rTemp3);
        }
        else if (nodeIndex == 1) 
        {
            temp.transform.position = new Vector3(100, 0, -150);
            temp.transform.name = "2";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[1].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[4].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[2].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[7].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[3].setRoad(rTemp3);
        }
        else if (nodeIndex == 2)
        {
            temp.transform.position = new Vector3(186, 0, -150);
            temp.transform.name = "3";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[2].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[5].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[6].setSettlementHex(temp4);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[4].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[8].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[5].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, 0.05f);
            rTemp4.transform.Rotate(new Vector3(0, 90, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[9].setRoad(rTemp4);
        }
        else if (nodeIndex == 7)
        {
            temp.transform.position = new Vector3(-32, 0, -75);
            temp.transform.name = "7";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[7].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[11].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[10].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[18].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[11].setRoad(rTemp3);
        }
        else if (nodeIndex == 8)
        {
            temp.transform.position = new Vector3(56, 0, -75);
            temp.transform.name = "8";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[8].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[12].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[12].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[19].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[13].setRoad(rTemp3);
        }
        else if (nodeIndex == 9)
        {
            temp.transform.position = new Vector3(143, 0, -75);
            temp.transform.name = "9";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[9].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[13].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[14].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[20].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[15].setRoad(rTemp3);
        }
        else if (nodeIndex == 10)
        {
            temp.transform.position = new Vector3(230.5f, 0, -75);
            temp.transform.name = "10";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[10].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[14].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[15].setSettlementHex(temp4);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[16].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[21].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[17].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, 0.05f);
            rTemp4.transform.Rotate(new Vector3(0, 90, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[22].setRoad(rTemp4);
        }
        else if (nodeIndex == 16)
        {
            temp.transform.position = new Vector3(-75.5f, 0, 0);
            temp.transform.name = "16";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[16].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[21].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[33].setSettlementHex(temp4);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[27].setSettlementHex(temp5);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[23].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[33].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[24].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp4.transform.Rotate(new Vector3(0, 30, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[40].setRoad(rTemp4);

            GameObject rTemp5 = Instantiate(roadPrefab, temp.transform);
            rTemp5.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp5.transform.Rotate(new Vector3(0, -30, 0));
            rTemp5.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[39].setRoad(rTemp5);
        }
        else if (nodeIndex == 17)
        {
            temp.transform.position = new Vector3(12, 0, 0);
            temp.transform.name = "17";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[17].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[22].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[34].setSettlementHex(temp4);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[28].setSettlementHex(temp5);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[25].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[34].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[26].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp4.transform.Rotate(new Vector3(0, 30, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[42].setRoad(rTemp4);

            GameObject rTemp5 = Instantiate(roadPrefab, temp.transform);
            rTemp5.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp5.transform.Rotate(new Vector3(0, -30, 0));
            rTemp5.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[41].setRoad(rTemp5);
        }
        else if (nodeIndex == 18)
        {
            temp = Instantiate(desertHexPrefab, new Vector3(100, 0, 0), transform.rotation);
            temp.transform.localScale = new Vector3(50, 50, 50);
            temp.transform.name = "18";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            //GameObject robberTemp = Instantiate(robber, temp.transform);
            //robberTemp.transform.localPosition = new Vector3(-2f, 0.32f, 0.005f);
            //robberTemp.transform.localScale = new Vector3(0.6f, 0.1f, 0.6f);

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[18].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[23].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[35].setSettlementHex(temp4);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[29].setSettlementHex(temp5);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[27].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[35].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[28].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp4.transform.Rotate(new Vector3(0, 30, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[44].setRoad(rTemp4);

            GameObject rTemp5 = Instantiate(roadPrefab, temp.transform);
            rTemp5.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp5.transform.Rotate(new Vector3(0, -30, 0));
            rTemp5.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[43].setRoad(rTemp5);

        }
        else if (nodeIndex == 19)
        {
            temp.transform.position = new Vector3(186, 0, 0);
            temp.transform.name = "19";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[19].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[24].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[36].setSettlementHex(temp4);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[30].setSettlementHex(temp5);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[29].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[36].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[30].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp4.transform.Rotate(new Vector3(0, 30, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[46].setRoad(rTemp4);

            GameObject rTemp5 = Instantiate(roadPrefab, temp.transform);
            rTemp5.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp5.transform.Rotate(new Vector3(0, -30, 0));
            rTemp5.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[45].setRoad(rTemp5);
        }
        else if (nodeIndex == 20)
        {
            temp.transform.position = new Vector3(274, 0, 0);
            temp.transform.name = "20";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[20].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[25].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[26].setSettlementHex(temp4);

            GameObject temp5 = Instantiate(settlementCityPrefab, temp.transform);
            temp5.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            temp5.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[37].setSettlementHex(temp5);

            GameObject temp6 = Instantiate(settlementCityPrefab, temp.transform);
            temp6.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            temp6.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[31].setSettlementHex(temp6);

            GameObject temp7 = Instantiate(settlementCityPrefab, temp.transform);
            temp7.transform.localPosition = new Vector3(-1.1f, 0.335f, 0.55f);
            temp7.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[32].setSettlementHex(temp7);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.4f, 0.335f, -0.7f);
            rTemp1.transform.Rotate(new Vector3(0, 30, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[31].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.05f);
            rTemp2.transform.Rotate(new Vector3(0, 90, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[37].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-1.5f, 0.335f, -0.7f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[32].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, 0.05f);
            rTemp4.transform.Rotate(new Vector3(0, 90, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[38].setRoad(rTemp4);

            GameObject rTemp5 = Instantiate(roadPrefab, temp.transform);
            rTemp5.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp5.transform.Rotate(new Vector3(0, 30, 0));
            rTemp5.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[48].setRoad(rTemp5);

            GameObject rTemp6 = Instantiate(roadPrefab, temp.transform);
            rTemp6.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp6.transform.Rotate(new Vector3(0, -30, 0));
            rTemp6.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[47].setRoad(rTemp6);
        }
        else if (nodeIndex == 43)
        {
            temp.transform.position = new Vector3(-32, 0, 75);
            temp.transform.name = "43";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[43].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[38].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[49].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[55].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[54].setRoad(rTemp3);
        }
        else if (nodeIndex == 44)
        {
            temp.transform.position = new Vector3(56, 0, 75);
            temp.transform.name = "44";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[44].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[39].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[50].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[57].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[56].setRoad(rTemp3);

        }
        else if (nodeIndex == 45)
        {
            temp.transform.position = new Vector3(143, 0, 75);
            temp.transform.name = "45";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[45].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[40].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[51].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[59].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[58].setRoad(rTemp3);
        }
        else if (nodeIndex == 46)
        {
            temp.transform.position = new Vector3(230, 0, 75);
            temp.transform.name = "46";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[46].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[41].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[42].setSettlementHex(temp4);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[52].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[61].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[60].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.075f);
            rTemp4.transform.Rotate(new Vector3(0, 90, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[53].setRoad(rTemp4);
        }
        else if (nodeIndex == 51)
        {
            temp.transform.position = new Vector3(12.5f, 0, 150);
            temp.transform.name = "51";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[51].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[47].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[62].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[67].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[66].setRoad(rTemp3);
        }
        else if (nodeIndex == 52)
        {
            temp.transform.position = new Vector3(100, 0, 150);
            temp.transform.name = "52";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[52].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[48].setSettlementHex(temp3);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[63].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[69].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[68].setRoad(rTemp3);
        }
        else if (nodeIndex == 53)
        {
            temp.transform.position = new Vector3(186, 0, 150);
            temp.transform.name = "53";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[53].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[49].setSettlementHex(temp3);

            GameObject temp4 = Instantiate(settlementCityPrefab, temp.transform);
            temp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            temp4.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[50].setSettlementHex(temp4);

            GameObject rTemp1 = Instantiate(roadPrefab, temp.transform);
            rTemp1.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.075f);
            rTemp1.transform.Rotate(new Vector3(0, 90, 0));
            rTemp1.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[64].setRoad(rTemp1);

            GameObject rTemp2 = Instantiate(roadPrefab, temp.transform);
            rTemp2.transform.localPosition = new Vector3(-1.5f, 0.335f, 0.75f);
            rTemp2.transform.Rotate(new Vector3(0, 30, 0));
            rTemp2.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[71].setRoad(rTemp2);

            GameObject rTemp3 = Instantiate(roadPrefab, temp.transform);
            rTemp3.transform.localPosition = new Vector3(-2.4f, 0.335f, 0.75f);
            rTemp3.transform.Rotate(new Vector3(0, -30, 0));
            rTemp3.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[70].setRoad(rTemp3);

            GameObject rTemp4 = Instantiate(roadPrefab, temp.transform);
            rTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.075f);
            rTemp4.transform.Rotate(new Vector3(0, 90, 0));
            rTemp4.transform.localScale = new Vector3(0.5f, 0.25f, 0.1f);

            edgeList[65].setRoad(rTemp4);

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
            temp.transform.name = temp.name + "0";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 1)
        {
            temp.transform.position = new Vector3(0, 10.5f, -150f);
            temp.transform.name = temp.name + "1";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 2)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, -150f);
            temp.transform.name = temp.name + "2";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 7)
        {
            temp.transform.position = new Vector3(-130, 10.5f, -75);
            temp.transform.name = temp.name + "7";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 8)
        {
            temp.transform.position = new Vector3(-42.5f, 10.5f, -75);
            temp.transform.name = temp.name + "8";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 9)
        {
            temp.transform.position = new Vector3(45, 10.5f, -75);
            temp.transform.name = temp.name + "9";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 10)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, -75);
            temp.transform.name = temp.name + "10";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 16)
        {
            temp.transform.position = new Vector3(-175, 10.5f, 0);
            temp.transform.name = temp.name + "16";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 17)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 0);
            temp.transform.name = temp.name + "17";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 18)
        {

        }
        else if (nodePosition == 19)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, 0);
            temp.transform.name = temp.name + "19";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 20)
        {
            temp.transform.position = new Vector3(175, 10.5f, 0);
            temp.transform.name = temp.name + "20";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 43)
        {
            temp.transform.position = new Vector3(-130, 10.5f, 75);
            temp.transform.name = temp.name + "43";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 44)
        {
            temp.transform.position = new Vector3(-42.5f, 10.5f, 75);
            temp.transform.name = temp.name + "44";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 45)
        {
            temp.transform.position = new Vector3(45, 10.5f, 75);
            temp.transform.name = temp.name + "45";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 46)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, 75);
            temp.transform.name = temp.name + "46";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        if (nodePosition == 51)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 150f);
            temp.transform.name = temp.name + "51";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 52)
        {
            temp.transform.position = new Vector3(0, 10.5f, 150f);
            temp.transform.name = temp.name + "52";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
        }
        else if (nodePosition == 53)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, 150f);
            temp.transform.name = temp.name + "53";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
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
        //temp.transform.name = temp.name + counter.ToString();
        //counter++;
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
    public Node getCurrentNode(GameObject g)
    {
        for(int i = 0; i < boardNodes.Length; i++)
        {
            if (boardNodes[i].getSettlementHex() == g)
            {
                return boardNodes[i];
            }
        }
        return null;
    }
    public Edge getCurrentEdge(GameObject g)
    {
        for (int i = 0; i < edgeList.Length; i++)
        {
            if (edgeList[i].getRoad() == g)
            {
                return edgeList[i];
            }
        }
        return null;
    }

    public int nodeCounter = 0;
    public void setLocalHexes()
    {
        for (int i = 1; i < 20; i++)
        {
            Debug.Log("THIS I: " + i);
            if (i < 13)
            {
                checkWidthOfParticularNode(nodeCounter);
                checkHeightOfParticularNode(nodeCounter);
                Debug.Log("cWidth: " + cWidth);
                Debug.Log("cHeight: " + cHeight);
                Debug.Log("sIndex: " + sIndex);
                for (int w = 0; w <= 5; w++)
                {
                    Debug.Log("NodeCounter: " + nodeCounter);
                    if (w == 0)
                    {
                        boardNodes[nodeCounter].SetOHexLocation(i);
                    }
                    if (w == 1)
                    {
                        boardNodes[nodeCounter + cWidth].SetRHexLocation(i);
                    }
                    if (w == 2)
                    {
                        boardNodes[nodeCounter + cWidth + 1].SetLHexLocation(i);
                    }
                    if (w == 3)
                    {
                        boardNodes[nodeCounter + (2 * cWidth) + 1].SetRHexLocation(i);
                    }
                    if (w == 4)
                    {
                        boardNodes[nodeCounter + (2 * cWidth) + 2].SetLHexLocation(i);
                    }
                    if (w == 5)
                    {
                        boardNodes[nodeCounter + (3 * cWidth) + 3].SetOHexLocation(i);
                    }

                }
               
                if (nodeCounter == cWidth + sIndex - 1)
                {
                    nodeCounter = nodeCounter + cWidth + 2;
                }
                else
                {
                    nodeCounter++;
                }
               
            }
            else
            {
                if (i == 13)
                {
                    Debug.Log("HIT");
                    nodeCounter = 43;
                }
                Debug.Log("NEWHALF");
                Debug.Log("THIS I: " + i);
                
                checkWidthOfParticularNode(nodeCounter);
                checkHeightOfParticularNode(nodeCounter);
                Debug.Log("cWidth: " + cWidth);
                Debug.Log("cHeight: " + cHeight);
                Debug.Log("sIndex: " + sIndex);
                for (int w = 0; w <= 5; w++)
                {
                    Debug.Log("NodeCounter: " + nodeCounter);
                    if (w == 0)
                    {
                        boardNodes[nodeCounter].SetOHexLocation(i);
                    }
                    if (w == 1)
                    {
                        boardNodes[nodeCounter - cWidth].SetLHexLocation(i);
                    }
                    if (w == 2)
                    {
                        boardNodes[nodeCounter - cWidth - 1].SetRHexLocation(i);
                    }
                    if (w == 3)
                    {
                        boardNodes[nodeCounter - (2 * cWidth) - 1].SetLHexLocation(i);
                    }
                    if (w == 4)
                    {
                        boardNodes[nodeCounter - (2 * cWidth) - 2].SetRHexLocation(i);
                    }
                    if (w == 5)
                    {
                        boardNodes[nodeCounter - (3 * cWidth) - 3].SetOHexLocation(i);
                    }
                }
               
                if (nodeCounter == cWidth + sIndex - 1)
                {
                    nodeCounter = nodeCounter + cWidth + 1;
                }
                else
                {
                    nodeCounter++;
                }
            }
        }
    }


    

    public string isHex(int index)
    {
        checkWidthOfParticularNode(index);
        checkHeightOfParticularNode(index);
        // If the index is on the bottom half of the board
        if (index <= 26)
        {
            // Special case - bottom most height, only OHex
            if (cWidth == 3)
            {
                return "LR";
            }
            // If the index is on the bottom half and an odd height
            if (cHeight % 2 == 1)
            {
                return "ROL";
            }
            // If the index is on the bottom half and an even height
            if (cHeight % 2 == 0 && (sIndex < index && index < sIndex + cWidth - 1))
            {
                return "ROL";
            }
            else if (cHeight % 2 == 0 && sIndex == index)
            {
                return "RO";
            }
            else if (cHeight % 2 == 0 && index == sIndex + cWidth - 1)
            {
                return "LO";
            }
        }
        else if (index > 26)
        {
            if (cWidth == 3)
            {
                return "LR";
            }
            // If the index is on the bottom half and an odd height
            if (cHeight % 2 == 0)
            {
                return "ROL";
            }
            // If the index is on the bottom half and an even height
            if (cHeight % 2 == 1 && (sIndex < index && index < sIndex + cWidth - 1))
            {
                return "ROL";
            }
            else if (cHeight % 2 == 1 && sIndex == index)
            {
                return "RO";
            }
            else if (cHeight % 2 == 1 && index == sIndex + cWidth - 1)
            {
                return "LO";
            }
        }
        return null;
    }



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
