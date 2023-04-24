using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    // These are variable to store each panel used. allows direct access to each panel
    [SerializeField] GameObject playerState;
    [SerializeField] GameObject theInitialTradePanel;
    [SerializeField] GameObject receivedTradePanel;
    [SerializeField] GameObject halfResourcePanel;
    [SerializeField] GameObject stealResourcePanel;

    // The following is a set of variables to store each prefab which is used to instantiate each of the later on

    // Prefabs for each hexagon: 
    [SerializeField] GameObject fieldHexPrefab;
    [SerializeField] GameObject lumbgerHexPrefab;
    [SerializeField] GameObject brickHexPrefab;
    [SerializeField] GameObject oreHexPrefab;
    [SerializeField] GameObject woolHexPrefab;
    [SerializeField] GameObject desertHexPrefab;

    // Prefabs for each number on the hexagon:
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

    // Prafbs for each object we "build".
    [SerializeField] GameObject settlementCityPrefab;
    [SerializeField] GameObject roadPrefab;
    [SerializeField] GameObject cityPrefab;


    // Variables to allow us to access properties of the PlayerStateManager and Player classes
    PlayerStateManager state;
    Player player;

    // Variables to allow us to access properties from the DiceRoller class
    [SerializeField] GameObject dice;
    DiceRoller diceRoller;

    // Variables to allow us to access properties from the Robber class
    [SerializeField] GameObject rob;
    Robber robber;

    // These variables are used to help setup the game (will reference them as the "base variables")

    // Width is the number of vertices on bottom row
    public int width;
    // Current width will change as we move up the board (during generation or setting of nodes / edges)
    public int cWidth;
    // Can be seen as the amount of nodes
    public int totalWidth;
    // Otherwise know as the "level", essentially how many rows of nodes we have gone through
    public int height;
    // Current height will be the current "level" of the board we are on
    public int cHeight;
    // The first node of the row / height we are on
    public int sIndex;

    public int i;
    public int nodeCounter = 0;
    public int introCounter = 0;

    public bool introTurn;
    public bool placedIntroSettlements;
    public bool placedIntroRoads;

    public int edgeNum;

    // These varaiables are used in assignHex() and takes a new value each iteration of the loop
    public int setValue;
    public string setResource;

    // Arrays to help with generation
    public int[] valueSet;
    public string[] resourceSet;

    public Edge[] edgeList;

    public Node[] boardNodes;
    public Node node;

    public string[] developmentCards;

    public GameObject[] settlementCities;

    // Awake function called before start to initialse the GameObject we use to access other classes
    void Awake()
    {
        state = playerState.GetComponent<PlayerStateManager>();

        diceRoller = dice.GetComponent<DiceRoller>();

        rob = (GameObject)Resources.Load("robber");
        robber = rob.GetComponent<Robber>();
    }

    // Start is the first method called to initialize various variables
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

        edgeList = new Edge[72];
        boardNodes = new Node[54];

        // Here we initialize our resource and value array that will hod the types / values of the hexagons / numbers and also the development card array
        developmentCards = new string[25] { "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "knight", "road", "road", "yearofplenty", "yearofplenty", "monopoly", "monopoly", "university", "market", "greathall", "chapel", "library" };
        resourceSet = new string[18] { "wool", "wool", "wool", "wool", "lumber", "lumber", "lumber", "lumber", "grain", "grain", "grain", "grain", "brick", "brick", "brick", "ore", "ore", "ore" };
        // We do 2 checks here to allow variable sized boards if implementation possible, otherwise uses the default size (3)
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

        // The next few lines of code is what actually sets up the game, the order of method calls is important, each will be explained
        // NOTE: it was important to "resetInitialVariables()" (which will reset the "base variables" to their inital values) after each stage of the board initialization as we use a checkWidth() method
        // at each stage and without resetting it would cause various erros.

        // Here we fill up the node array with node objets
        GenerateBoard();
        resetIntialvariables();

        // Here we loop through each node and call the setNode() method on each. This will, at each node, set it's respective nodeWest, nodeEast and nodeNorthSouth node.
        // Additionally, it will set a "boardLocation" to each node incase needed later.
        for (int q = 0; q < totalWidth; q++)
        {
            boardNodes[q].SetNode(q);
            boardNodes[q].boardLocation = q;
        }
        resetIntialvariables();

        // Here we do two things, the edges are created and fill up the edge array. Each edge is created with a "node1" and "node2". Secondly, when we create an edge, we set it's "node1"s and "node2"s edgeWest,
        // edgeEast and edgeNorthSouth respectivly.
        setBoardEdges();
        resetIntialvariables();

        // Here we call an important method which will do a few things, to start we loop through every node and call two more methods: assignHexPositions() and assignNumberPositions() which
        // will (depending on the node position) instantiate a hex / hexNumber respectivly and relocate it to it's correct position relative to the node. Within these 2 methods we do some additional
        // things which will be explained later.
        // The final thing we do in this method is, at each node, set it's respective rHex, lHex and oHex value / resource.
        assignHex();
        resetIntialvariables();

        // Here we once again loop through each node, at each node we set it's respective rHex, lHex and oHex locations on the board (1-19 on the default size)
        setLocalHexes();
        resetIntialvariables();

        // Here we edit some our UI compontants to not be displayed, we revert this after the "intro turn" has concluded
        GameObject.Find("EndTurn").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("Timer").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("VP").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("PlayerTrade").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("Resources").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("RollDiceButton").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("End Turn Button").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("BuildingCosts").GetComponent<CanvasGroup>().alpha = 0f;
        GameObject.Find("CurrentPlayer").transform.position = new Vector3(170, 385, 0);
        GameObject.Find("CurrentPlayer").GetComponent<Text>().fontSize = 28;

        // Pop up to say player 1 build house
        Debug.Log("Player 1 please build a settlement");

        // Here we set two of our panels to not active as they are not required at the start
        receivedTradePanel.SetActive(false);
        stealResourcePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Generate board will fill up the node array with a new node at each position
    void GenerateBoard()
    {
        for (int j = 0; j < boardNodes.Length; j++)
        {
            boardNodes[j] = new Node();   
        }
    }

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
                    setValue = getVal();
                    setResource = getResource();
                    // Loops through each of the five nodes on a hexagon and sets it's respective rHex, lhex and oHex value and resource
                    if (cHeight != 5)
                    {
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
                    else
                    {
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
                                boardNodes[m + (3 * cWidth) + 2].SetOHex(setValue);
                                boardNodes[m + (3 * cWidth) + 2].SetOHexResource(setResource);
                            }
                        }
                        asignHexPositions(m);
                        assignNumberPositions(m);
                    }
                }
                // Case where the current height is greater than total height / 2 (top half of then board) and m is
                // on an even height
                else if (cHeight > height / 2 + 2 && cHeight % 2 == 0)
                {
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

    // This method is very similar to the one above but is fixed as apose to being calculated. This was needed in some cases and will only work effectivly on the default sized board
    // Only calculated the cWidth
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

    // Once again similar to the above methods but will calculate the cHeight and sIndex of a particular node
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

    // Two methods to choose either our resources for the hexagons or the numbers for hexagons at random

    // For the values, we take a random position from the value array, if it's 0 we choose another, if not, we set
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
    // added to an edge array with a "node1" and a "node2".
    // We also, at each created edge, set it to it's "node1"s and "node2"s respective edgeWest, edgeEast and edgeNorthSouth
    public void setBoardEdges()
    {
        int edgeCounter = 0;
        for (int k = 0; k < boardNodes.Length; k++)
        {
            checkWidth(k);
            // Checks if k / current node is on the bottom half of the board
            if (cHeight <= height / 2)
            {
                // Checks if k is on an odd height, bottom half
                if (cHeight % 2 == 1)
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeWest());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeWest = edgeList[edgeCounter];
                    boardNodes[k].getNodeWest().edgeEast = edgeList[edgeCounter];
                    edgeCounter++;

                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeEast = edgeList[edgeCounter];
                    boardNodes[k].getNodeEast().edgeWest = edgeList[edgeCounter];

                    edgeCounter++;
                }
                else
                // If k wasn't on an odd height it must be on an even height, bottom half
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeNorthSouth = edgeList[edgeCounter];
                    boardNodes[k].getNodeNorthSouth().edgeNorthSouth = edgeList[edgeCounter];

                    edgeCounter++;
                }
            }
            // Top half of the board
            else
            {
                // Checks if k if on the 7th height / the first height of the top half
                // If so we skip it as the edge has already ben created and set
                if (cHeight == height / 2 + 1)
                {

                }
                // Checks if k is on an even heigt, top half
                else if (cHeight % 2 == 0)
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeWest());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeWest = edgeList[edgeCounter];
                    boardNodes[k].getNodeWest().edgeEast = edgeList[edgeCounter];
                    edgeCounter++;

                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeEast());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeEast = edgeList[edgeCounter];
                    boardNodes[k].getNodeEast().edgeWest = edgeList[edgeCounter];
                    edgeCounter++;
                }
                // If k wasn't on an even height, it must be on an odd heght, top half
                else
                {
                    edgeList[edgeCounter] = new Edge(boardNodes[k], boardNodes[k].getNodeNorthSouth());
                    edgeList[edgeCounter].edgeBoardLocation = edgeCounter;

                    boardNodes[k].edgeNorthSouth = edgeList[edgeCounter];
                    boardNodes[k].getNodeNorthSouth().edgeNorthSouth = edgeList[edgeCounter];

                    edgeCounter++;
                }
            }
        }
    }

    // The next two methods are what create the hexagons, numbers and building game objects (from their respective prefab in unity) and position it in the correct
    // position. Alot of the code is the same but we needed to have a case for every combination of hexagon and number location

    // This method does the hexes, will obtain the current node position, the current resource and instantiate it in the corrosponding
    // location that links to the node postion. Additionaly it will create a certain amount of settlements, cities and roads in order to fill the board with a gameobject in
    // every location where it may be needed
    // It will also give each hexagon a collider, the robber script componant and some variable from the robber script. The reason this was needed is due to these objects
    // needed access to the scripts but it couldnt be set in the inspector as they are created at run time
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
            temp.transform.name = "1";
            temp.AddComponent<SphereCollider>().center = new Vector3(-2,0,0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[0].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[3].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[0].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[3].setCity(cTemp3);

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
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[1].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[4].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[1].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[4].setCity(cTemp3);

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
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[2].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[5].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[6].setCity(cTemp3);

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
            temp.transform.name = "4";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[7].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[11].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[7].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[11].setCity(cTemp3);

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
            temp.transform.name = "5";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[8].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[12].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[8].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[12].setCity(cTemp3);

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
            temp.transform.name = "6";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[9].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[13].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[9].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[13].setCity(cTemp3);

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
            temp.transform.name = "7";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[10].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[14].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[15].setCity(cTemp4);

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
            temp.transform.name = "8";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[16].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[21].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[33].setCity(cTemp4);

            GameObject cTemp5 = Instantiate(cityPrefab, temp.transform);
            cTemp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            cTemp5.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[27].setCity(cTemp5);

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
            temp.transform.name = "9";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[17].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[22].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[34].setCity(cTemp4);

            GameObject cTemp5 = Instantiate(cityPrefab, temp.transform);
            cTemp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            cTemp5.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[28].setCity(cTemp5);

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
            temp.transform.name = "10";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[18].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[23].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[35].setCity(cTemp4);

            GameObject cTemp5 = Instantiate(cityPrefab, temp.transform);
            cTemp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            cTemp5.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[29].setCity(cTemp5);

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
            temp.transform.name = "11";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[19].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[24].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[36].setCity(cTemp4);

            GameObject cTemp5 = Instantiate(cityPrefab, temp.transform);
            cTemp5.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            cTemp5.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[30].setCity(cTemp5);

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
            temp.transform.name = "12";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, -0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[20].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.85f, 0.335f, -0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[25].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.1f, 0.335f, -0.45f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[26].setCity(cTemp4);

            GameObject cTemp5 = Instantiate(cityPrefab, temp.transform);
            cTemp5.transform.localPosition = new Vector3(-1.95f, 0.335f, 0.92f);
            cTemp5.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[37].setCity(cTemp5);

            GameObject cTemp6 = Instantiate(cityPrefab, temp.transform);
            cTemp6.transform.localPosition = new Vector3(-2.85f, 0.335f, 0.55f);
            cTemp6.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[31].setCity(cTemp6);

            GameObject cTemp7 = Instantiate(cityPrefab, temp.transform);
            cTemp7.transform.localPosition = new Vector3(-1.1f, 0.335f, 0.55f);
            cTemp7.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[32].setCity(cTemp7);

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
            temp.transform.name = "13";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[43].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[38].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[43].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[38].setCity(cTemp3);

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
            temp.transform.name = "14";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[44].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[39].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[44].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[39].setCity(cTemp3);

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
            temp.transform.name = "15";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[45].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[40].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[45].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[40].setCity(cTemp3);

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
            temp.transform.name = "16";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[46].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[41].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[42].setCity(cTemp4);

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
            temp.transform.name = "17";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[51].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[47].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[51].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[47].setCity(cTemp3);

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
            temp.transform.name = "18";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

            GameObject temp2 = Instantiate(settlementCityPrefab, temp.transform);
            temp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            temp2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[52].setSettlementHex(temp2);

            GameObject temp3 = Instantiate(settlementCityPrefab, temp.transform);
            temp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            temp3.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            boardNodes[48].setSettlementHex(temp3);

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[52].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[48].setCity(cTemp3);

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
            temp.transform.name = "19";

            temp.AddComponent<SphereCollider>().center = new Vector3(-2, 0, 0);
            temp.GetComponent<SphereCollider>().radius = 0.875f;
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");

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

            GameObject cTemp2 = Instantiate(cityPrefab, temp.transform);
            cTemp2.transform.localPosition = new Vector3(-2, 0.335f, 0.92f);
            cTemp2.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[53].setCity(cTemp2);

            GameObject cTemp3 = Instantiate(cityPrefab, temp.transform);
            cTemp3.transform.localPosition = new Vector3(-2.785f, 0.335f, 0.45f);
            cTemp3.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[49].setCity(cTemp3);

            GameObject cTemp4 = Instantiate(cityPrefab, temp.transform);
            cTemp4.transform.localPosition = new Vector3(-1.2f, 0.335f, 0.45f);
            cTemp4.transform.localScale = new Vector3(0.25f, 0.12f, 0.25f);

            boardNodes[50].setCity(cTemp4);

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
    // it in the correct location.
    // Once again it will give each of these objects a collider, the robber script and needed variables from the robber class. FOr the same reasoning as above
    public void assignNumberPositions(int nodePosition)
    {
        GameObject temp = instantiateHexNumber(setValue);
        if (nodePosition == 0)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, -150f);
            temp.transform.name = temp.name + "0";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 1)
        {
            temp.transform.position = new Vector3(0, 10.5f, -150f);
            temp.transform.name = temp.name + "1";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 2)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, -150f);
            temp.transform.name = temp.name + "2";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 7)
        {
            temp.transform.position = new Vector3(-130, 10.5f, -75);
            temp.transform.name = temp.name + "7";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 8)
        {
            temp.transform.position = new Vector3(-42.5f, 10.5f, -75);
            temp.transform.name = temp.name + "8";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 9)
        {
            temp.transform.position = new Vector3(45, 10.5f, -75);
            temp.transform.name = temp.name + "9";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 10)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, -75);
            temp.transform.name = temp.name + "10";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 16)
        {
            temp.transform.position = new Vector3(-175, 10.5f, 0);
            temp.transform.name = temp.name + "16";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 17)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 0);
            temp.transform.name = temp.name + "17";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
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
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 20)
        {
            temp.transform.position = new Vector3(175, 10.5f, 0);
            temp.transform.name = temp.name + "20";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
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
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 45)
        {
            temp.transform.position = new Vector3(45, 10.5f, 75);
            temp.transform.name = temp.name + "45";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 46)
        {
            temp.transform.position = new Vector3(132.5f, 10.5f, 75);
            temp.transform.name = temp.name + "46";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        if (nodePosition == 51)
        {
            temp.transform.position = new Vector3(-87.5f, 10.5f, 150f);
            temp.transform.name = temp.name + "51";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 52)
        {
            temp.transform.position = new Vector3(0, 10.5f, 150f);
            temp.transform.name = temp.name + "52";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
        else if (nodePosition == 53)
        {
            temp.transform.position = new Vector3(87.5f, 10.5f, 150f);
            temp.transform.name = temp.name + "53";

            temp.AddComponent<BoxCollider>();
            temp.AddComponent<Robber>();
            temp.GetComponent<Robber>().stealResourcePanel = GameObject.Find("StealResourcePanel");
            temp.GetComponent<Robber>().stealPlayer1Button = GameObject.Find("player1Button");
            temp.GetComponent<Robber>().stealPlayer2Button = GameObject.Find("player2Button");
            temp.GetComponent<Robber>().stealPlayer3Button = GameObject.Find("player3Button");
            temp.GetComponent<Robber>().stealPlayer4Button = GameObject.Find("player4Button");
        }
    }

    // This method is called in the asignHex method and was created to reduce repeated code. It takes in the randomly chosen resource,
    // instantiates it's corosponding hex prefab and returns it
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

    // Similar to the above method, but instead takes in a value, instantiates the corrosponding hexNumber prefab and returns it
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

    // This is a basic method that loops 19 times (working only on the default sized board) and on each iteration sets each nodes rHex location, lHexLocation and oHex location
    // respectivly.
    public void setLocalHexes()
    {
        // Loops 19 times as that is the number if hexes on the default board
        for (int i = 1; i < 20; i++)
        {
            // Checks if we are the on the bottom hlaf of the baord
            if (i < 13)
            {
                // Here is where the particualr width / height of a certain node was needed
                checkWidthOfParticularNode(nodeCounter);
                checkHeightOfParticularNode(nodeCounter);

                // We check that the current height isn't 5 as 5 has a special case where the array indexing needs to be adjusted by 1
                if (cHeight != 5)
                {
                    for (int w = 0; w <= 5; w++)
                    {
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
                }
                // If the curent height is 5
                else
                {
                    for (int w = 0; w <= 5; w++)
                    {
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
                            boardNodes[nodeCounter + (3 * cWidth) + 2].SetOHexLocation(i);
                        }

                    }
                }
                // This section of code will run after we loop through the 5 nodes around a hex and wil check if we are changing height
                // If so, the nodeCounter is adjusted correctly
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
                    nodeCounter = 43;
                }
                checkWidthOfParticularNode(nodeCounter);
                checkHeightOfParticularNode(nodeCounter);
                for (int w = 0; w <= 5; w++)
                {
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

    // This method will take in a index to a node as a parameter and will return a string to specify what hexagons that particlar node has surrounding it
    // ROL - RightOtherLeft hexagons, LR - LeftRight hexagons, RO - RightOther hexagons and LO - LeftOther hexagons.
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
        for (int i = 0; i < boardNodes.Length; i++)
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
