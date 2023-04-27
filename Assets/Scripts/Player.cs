using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class Player
{
    // Some variables to hold important information about a player
    public int victoryPoints;
    public int playerNumber;
    public int playerColour;
    public int totalResources;
    public int longetRoad;
    public int avalibleKnights;
    public int usedKnights; // Largest army

    // Will store the development cards for an individual player
    //public string[] inventory;
    public bool freeBuild = false;
    int freeBuildCounter = 0;
    public bool HASLargestArmy = false;

    // A variable to store the amount of each resource individually
    // The type indicated the integer value that will internally represent each resource
    public int currencyLumber; // Type 1
    public int currencyGrain; // Type 2
    public int currencyBrick; // Type 3
    public int currencyOre; // Type 4
    public int currencyWool; // Type 5

    string previousObj = "road";
    string currentObj = "settlement";

    // Some variables to help us access properties from different classes
    GameObject PlayerState;
    PlayerStateManager state;

    GameObject board;
    Board myboard;

    GameObject dice;
    DiceRoller diceRoller;

    [SerializeField] GameObject fromBankPanel;
    [SerializeField] GameObject stealAllOfOneResourcePanel;

    // Player constructor
    public Player()
    {
        // Initalizies each variable
        victoryPoints = 0;
        currencyLumber = 0;
        currencyGrain = 0;
        currencyBrick = 0;
        currencyOre = 0;
        currencyWool = 0;

        //inventory = new string[14];

        board = GameObject.Find("Board");
        myboard = board.GetComponent<Board>();

        PlayerState = GameObject.Find("End Turn Button");
        state = PlayerState.GetComponent<PlayerStateManager>();

        fromBankPanel = GameObject.Find("yearoplentyPanel");
        stealAllOfOneResourcePanel = GameObject.Find("stealAllOf1Type");

        dice = GameObject.Find("DiceRolls");
        diceRoller = dice.GetComponent<DiceRoller>();
    }

    // This method is called evertime a the dice is rolled and will update the corrosponding players resource variable
    // depending on if the player has a houseBuilt on that node
    // type = int representation of the resource
    public void addResources(int type)
    {
        if (state.currentPlayerNumber == state.getCurrentPlayer(state.currentPlayerNumber).getPlayerNumber())
        {

            if (type == 1)
            {
                currencyLumber += 1;
            }
            else if (type == 2)
            {
                currencyGrain += 1;
            }
            else if (type == 3)
            {
                currencyBrick += 1;
            }
            else if (type == 4)
            {
                currencyOre += 1;
            }
            else if (type == 5)
            {
                currencyWool += 1;
            }
        }
        
    }

    // This method is called by a buy development card UI button. It will choose a random resource and add it to the players inventory 
    public void buyDevelopmentCard()
    {
        Debug.Log(state.getCurrentPlayer(state.currentPlayerNumber).currencyWool);
        Debug.Log(state.getCurrentPlayer(state.currentPlayerNumber).currencyOre);
        Debug.Log(state.getCurrentPlayer(state.currentPlayerNumber).currencyGrain >= 1);
        if (state.getCurrentPlayer(state.currentPlayerNumber).currencyWool >= 1 && state.getCurrentPlayer(state.currentPlayerNumber).currencyOre >= 1 && state.getCurrentPlayer(state.currentPlayerNumber).currencyGrain >= 1)
        {
            int randInt = UnityEngine.Random.Range(0, 25);
            string setCard = myboard.developmentCards[randInt];
            while (setCard == "")
            {
                randInt = UnityEngine.Random.Range(0, 25);
                setCard = myboard.developmentCards[randInt];
            }
            myboard.developmentCards[randInt] = "";

            if (setCard == "knight")
            {
                Debug.Log("You received: " + setCard);
                //Debug.Log("POS0: " + inventory[0]);
                //Debug.Log("bool: " + inventory[0] == "");
                //for (int j = 0; j < inventory.Length; j++)
                //{
                //    if (inventory[j] == "" || inventory[j] == null)
                //    {
                //        inventory[j] = setCard;
                //        latestKnightSpot = j;
                //        latestKnightSpot++;
                //    }
                //}
                //Debug.Log("HH: " + latestKnightSpot);
                state.getCurrentPlayer(state.currentPlayerNumber).avalibleKnights++;
            }
            else if (setCard == "road building")
            {
                Debug.Log("You received: " + setCard);
                Debug.Log("Please build 2 road for free");

                state.getCurrentPlayer(state.currentPlayerNumber).freeBuild = true;
            }
            else if (setCard == "yearofplenty")
            {
                Debug.Log("You received: " + setCard);
                fromBankPanel.SetActive(true);
            }
            else if (setCard == "monopoly")
            {
                Debug.Log("You received: " + setCard);
                stealAllOfOneResourcePanel.SetActive(true);
            }
            else if (setCard == "university" || setCard == "market" || setCard == "greathall" || setCard == "chapel" || setCard == "library")
            {
                Debug.Log("You received: " + setCard);
                Debug.Log("+1 victory point");
                state.getCurrentPlayer(state.currentPlayerNumber).victoryPoints++;
            }
        }
        Debug.Log("Not enough resources");
    }

    // TODO: 2 elements NOT yet implemented - roll dice to see who places first AND 2nd round of placing
    // is meant to be in reverse order


    // An important method that is called whenever a node game object is clicked (located on every hexagon corner). It will do a few things when clicked:
    // Firstly, it will check to see what hexagons are located around the clicked (RightOtherLeft / LeftRight / LeftOther / RightOther) node so it can then see if it possible to build.
    // After checking if building is possible it will check if it is the "introTurn" as if so, the buils will be free and every player will build 2 settlements and 2 roads.
    // If it's not the introTurn it will do an additional check of their resources.

    // Throughout this method, to show the settlement in game we simple enable it's MeshRenderer componant, set the house type and house colour.

    // To build a city, we once again click on a node and after seeing that the node has something built there it will check tomsee if it's of our team colour, if so
    // it indicates a city is trying to be built
    public void buildSettlementCity(GameObject g)
    {
        if (myboard.introCounter % 2 == 1 && myboard.introCounter != 0)
        {
            Debug.Log("Cannot build a settlement yet");
            return;
        }
        Node cNode = myboard.getCurrentNode(g);
        string hexes = myboard.isHex(cNode.boardLocation);
        if (hexes == "ROL")
        {
            if (cNode.houseColour == 0)
            {
                if (cNode.getNodeNorthSouth().getHouseType() == 0
                    && cNode.getNodeWest().getHouseType() == 0
                    && cNode.getNodeEast().getHouseType() == 0)
                {
                    if (myboard.introTurn)
                    {
                        if (myboard.introCounter == 0 || myboard.introCounter == 8)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 1;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 1;

                            if (myboard.introCounter == 0)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 1 please build a road");
                        }
                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 2;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 2;

                            if (myboard.introCounter == 2)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 3;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 3;

                            if (myboard.introCounter == 4)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 4;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 4;

                            if (myboard.introCounter == 6)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    // Case where we build a settlement, not apart of the intro
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber())
                            || (cNode.getNodeNorthSouth().getHouseType() == 0
                            && cNode.getNodeWest().getHouseType() == 0
                            && cNode.getNodeEast().getHouseType() == 0))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;
                            if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.white;
                            }
                            else if(state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            }
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = playerNumber;
                            myboard.boardNodes[cNode.boardLocation].houseColour = playerNumber;
                        }
                        else
                        {
                            Debug.Log("Cannot build here");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cannot build next to another player");
                }
            }
            // Checks if we can build a city on a node with ONLY a right, left and other hex
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }

                    myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = false;
                    myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().enabled = true;

                    currencyGrain = currencyGrain - 2;
                    currencyOre = currencyOre - 3;

                    victoryPoints--;
                    victoryPoints = victoryPoints + 2;
                }
                else
                {
                    Debug.Log("Not enough resources to build a city");
                } 
            }
            else
            {
                Debug.Log("Another player has already build here");
            }
        }
        else if (hexes == "LR")
        {
            if (cNode.houseColour == 0)
            {
                if (cNode.getNodeWest().getHouseType() == 0
                    && cNode.getNodeEast().getHouseType() == 0)
                {
                    if (myboard.introTurn)
                    {
                        if (myboard.introCounter == 0 || myboard.introCounter == 8)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 1;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 1;

                            if (myboard.introCounter == 0)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }

                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 1 please build a road");
                        }
                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 2;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 2;

                            if (myboard.introCounter == 2)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 3;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 3;

                            if (myboard.introCounter == 4)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 4;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 4;

                            if (myboard.introCounter == 6)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber())
                            || (cNode.getNodeNorthSouth().getHouseType() == 0
                            && cNode.getNodeWest().getHouseType() == 0
                            && cNode.getNodeEast().getHouseType() == 0))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.white;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            }
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = playerNumber;
                            myboard.boardNodes[cNode.boardLocation].houseColour = playerNumber;
                        }
                        else
                        {
                            Debug.Log("Cannot build here");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cannot build next to another player");
                }
            }
            // building a city on a node with ONLY a left and right hex
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = false;
                    myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().enabled = true;

                    currencyGrain = currencyGrain - 2;
                    currencyOre = currencyOre - 3;

                    victoryPoints--;
                    victoryPoints = victoryPoints + 2;
                }
                else
                {
                    Debug.Log("Not enough resources to build a city");
                }
            }
            else
            {
                Debug.Log("Another player has already build here");
            }
        }
        else if (hexes == "RO")
        {
            if (cNode.houseColour == 0)
            {
                if (cNode.getNodeNorthSouth().getHouseType() == 0
                    && cNode.getNodeEast().getHouseType() == 0)
                {
                    if (myboard.introTurn)
                    {
                        if (myboard.introCounter == 0 || myboard.introCounter == 8)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 1;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 1;

                            if (myboard.introCounter == 0)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 1 please build a road");
                        }

                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 2;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 2;

                            if (myboard.introCounter == 2)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 3;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 3;

                            if (myboard.introCounter == 4)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 4;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 4;

                            if (myboard.introCounter == 6)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber())
                            || (cNode.getNodeNorthSouth().getHouseType() == 0
                            && cNode.getNodeWest().getHouseType() == 0
                            && cNode.getNodeEast().getHouseType() == 0))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.white;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            }
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = playerNumber;
                            myboard.boardNodes[cNode.boardLocation].houseColour = playerNumber;
                        }
                        else
                        {
                            Debug.Log("Cannot build here");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cannot build next to another player");
                }
            }
            // building a city on a node with ONLY a right and other hex
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = false;
                    myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().enabled = true;

                    currencyGrain = currencyGrain - 2;
                    currencyOre = currencyOre - 3;

                    victoryPoints--;
                    victoryPoints = victoryPoints + 2;
                }
                else
                {
                    Debug.Log("Not enough resources to build a city");
                }
            }
            else
            {
                Debug.Log("Another player has already build here");
            }
        }
        else if (hexes == "LO")
        {
            if (cNode.houseColour == 0)
            {
                if (cNode.getNodeNorthSouth().getHouseType() == 0
                    && cNode.getNodeWest().getHouseType() == 0)
                {
                    if (myboard.introTurn)
                    {
                        if (myboard.introCounter == 0 || myboard.introCounter == 8)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 1;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 1;

                            if (myboard.introCounter == 0)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 1 please build a road");
                        }

                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 2;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 2;

                            if (myboard.introCounter == 2)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 3;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 3;

                            if (myboard.introCounter == 4)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 4;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 4;

                            if (myboard.introCounter == 6)
                            {
                                if (myboard.boardNodes[cNode.boardLocation].rHexResource != null)
                                {
                                    string initialRHexResource = myboard.boardNodes[cNode.boardLocation].rHexResource;
                                    state.setResource(initialRHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].lHexResource != null)
                                {
                                    string initialLHexResource = myboard.boardNodes[cNode.boardLocation].lHexResource;
                                    state.setResource(initialLHexResource, state.currentPlayerNumber);
                                }
                                if (myboard.boardNodes[cNode.boardLocation].oHexResource != null)
                                {
                                    string initialOHexResource = myboard.boardNodes[cNode.boardLocation].oHexResource;
                                    state.setResource(initialOHexResource, state.currentPlayerNumber);
                                }
                            }
                            victoryPoints++;
                            myboard.introCounter++;

                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber())
                            || (cNode.getNodeNorthSouth().getHouseType() == 0
                            && cNode.getNodeWest().getHouseType() == 0
                            && cNode.getNodeEast().getHouseType() == 0))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.white;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                            }
                            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                            {
                                g.GetComponent<MeshRenderer>().material.color = Color.blue;
                            }
                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = playerNumber;
                            myboard.boardNodes[cNode.boardLocation].houseColour = playerNumber;
                        }
                        else
                        {
                            Debug.Log("Cannot build hereee");
                        }
                    }
                }
                else
                {
                    Debug.Log("Cannot build next to another player");
                }
            }
            // building a city on a node with ONLY a left and other hex
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    }
                    else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
                    {
                        myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }
                    myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = false;
                    myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().enabled = true;

                    currencyGrain = currencyGrain - 2;
                    currencyOre = currencyOre - 3;

                    victoryPoints--;
                    victoryPoints = victoryPoints + 2;
                }
                else
                {
                    Debug.Log("Not enough resources to build a city");
                }
            }
            else
            {
                Debug.Log("Another player has already build here");
            }
        }
        // Builds city
        else if (currencyGrain >= 2 && currencyOre >= 3
            && cNode.houseColour == playerNumber
            && cNode.houseType == 1)
        {
            currencyGrain -= 2;
            currencyOre -= 3;

            if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 1)
            {
                myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 2)
            {
                myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 3)
            {
                myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else if (state.getCurrentPlayer(state.currentPlayerNumber).playerNumber == 4)
            {
                myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().material.color = Color.blue;
            }

            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = false;
            myboard.boardNodes[cNode.boardLocation].city.GetComponent<MeshRenderer>().enabled = true;

            victoryPoints++;
        }
        else
        {
            Debug.Log("Cannot build here");
        }
    }

    // This method has similar functionality to the buildSettlementCity method above but will build roads. Once again we have a road gameobject on every
    // side of every hexagon and will be called when is clicked. It will do the neccasary checks to see if it's the intro, if it's posible to build where clicked
    // and if not the introturn the resources are also checked

    // It's also in this method when we bring back the UI componants as we know when the intro turn is ending
    public void buildRoad(GameObject g)
    {
        if (myboard.introCounter % 2 == 0 && myboard.introTurn == true)
        {
            Debug.Log("Cannot build a road yet");
            return;
        }
        Edge cEdge = myboard.getCurrentEdge(g);
        if (cEdge.edgeColour == 0)
        {
            // If the either of the two nodes of the inputted edge has a settlement or city of the same players number
            if (cEdge.getNode1().getHouseColour() == state.currentPlayerNumber || cEdge.getNode2().getHouseColour() == state.currentPlayerNumber
                || cEdge.getNode1().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeNorthSouth.edgeColour == state.currentPlayerNumber
                || cEdge.getNode2().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeNorthSouth.edgeColour == state.currentPlayerNumber)

            {
                if (myboard.introTurn)
                {
                    if (myboard.introCounter == 1 || myboard.introCounter == 9)
                    {
                        Debug.Log("First road build");

                        g.GetComponent<MeshRenderer>().material.color = Color.white;
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].setEdgeColour(1);

                        findLongestRoad();

                        myboard.introCounter++;
                        state.SwitchState();

                        Debug.Log("Player 2 please build a Settlement");
                    }

                    else if (myboard.introCounter == 3 || myboard.introCounter == 11)
                    {
                        g.GetComponent<MeshRenderer>().material.color = Color.red;
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        findLongestRoad();

                        myboard.introCounter++;
                        state.SwitchState();

                        Debug.Log("Player 3 please build a settlement");
                    }
                    else if (myboard.introCounter == 5 || myboard.introCounter == 13)
                    {
                        g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        findLongestRoad();

                        myboard.introCounter++;
                        state.SwitchState();

                        Debug.Log("Player 4 please build a settlement");
                    }
                    else if (myboard.introCounter == 7 || myboard.introCounter == 15)
                    {
                        g.GetComponent<MeshRenderer>().material.color = Color.blue;
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        findLongestRoad();

                        myboard.introCounter++;

                        if (myboard.introCounter == 8)
                        {
                            Debug.Log("Player 1 please build a settlement");
                        }
                        else if (myboard.introCounter == 16)
                        {
                            myboard.introTurn = false;
                            Debug.Log("Intro turn ended");

                            GameObject.Find("End Turn Button").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("EndTurn").GetComponent<CanvasRenderer>().SetAlpha(1);
                            GameObject.Find("Timer").GetComponent<CanvasRenderer>().SetAlpha(1);
                            GameObject.Find("VP").GetComponent<CanvasRenderer>().SetAlpha(1);
                            GameObject.Find("BuyDevelopmentCard").GetComponent<CanvasRenderer>().SetAlpha(1);
                            GameObject.Find("Resources").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("RollDiceButton").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("PlayerTrade").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("BuildingCosts").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("BuyDevelopmentCard").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("Knight").GetComponent<CanvasGroup>().alpha = 1f;

                            GameObject.Find("CurrentPlayer").transform.position = new Vector3(750, -40, 0);
                            GameObject.Find("CurrentPlayer").GetComponent<Text>().fontSize = 20;

                            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;
                            GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = true;
                        }
                        state.SwitchState();
                    }
                }
                else
                {
                    if (freeBuild == true)
                    {
                        if (freeBuildCounter == 2)
                        {
                            freeBuild = false;
                            return;
                        }
                        if (state.currentPlayerNumber == 1)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                        }
                        else if (state.currentPlayerNumber == 2)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        else if (state.currentPlayerNumber == 3)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        }
                        else if (state.currentPlayerNumber == 4)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        findLongestRoad();

                        freeBuildCounter++;
                    }
                    else if (currencyLumber >= 1 && currencyBrick >= 1
                        && (cEdge.getNode1().getHouseColour() == getPlayerNumber()
                        || cEdge.getNode2().getHouseType() == getPlayerNumber()
                        || cEdge.getNode1().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeNorthSouth.edgeColour == state.currentPlayerNumber
                        || cEdge.getNode2().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeNorthSouth.edgeColour == state.currentPlayerNumber))

                    {
                        currencyLumber -= 1;
                        currencyBrick -= 1;

                        if (state.currentPlayerNumber == 1)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.white;
                        }
                        else if (state.currentPlayerNumber == 2)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                        else if (state.currentPlayerNumber == 3)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        }
                        else if (state.currentPlayerNumber == 4)
                        {
                            g.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = playerNumber;

                        Debug.Log("resourceBuild");
                        findLongestRoad();


                        // Instantiate road object at this node position / activate road that will already be created there (at every edge)
                    }
                    else
                    {
                        Debug.Log("Cannot build here");
                    }
                }
            }
            else
            {
                Debug.Log("You can only build a road next to your own settlement");
            }
        }
        else
        {
            Debug.Log("Another player has already build here");
        }
    }

    // Getter and setter form the player number so it could be set when each player is created
    // in the player manager and be obtained when needed
    public void setPlayerNumber(int playerNumberr)
    {
        playerNumber = playerNumberr;
    }

    public int getPlayerNumber()
    {
        return playerNumber;
    }


    public void findLongestRoad()
    {
        Node currentNode = null;
        Edge currentEdge = null;

        for (int j = 0; j < myboard.boardNodes.Length; j++)
        {
            myboard.boardNodes[j].checkedLongestRoad = false;
            myboard.edgeList[j].checkedLongestRoad = false;
        }

        for (int i = 0; i < myboard.boardNodes.Length; i++)
        {
            int currentLongestRoad = 0;
            if (myboard.boardNodes[i].houseColour == state.getCurrentPlayer(playerNumber).playerNumber && myboard.boardNodes[i].checkedLongestRoad == false)
            {
                currentNode = myboard.boardNodes[i];


                // Good to here
                //Debug.Log("Hits");
                //Debug.Log("NULL: " + currentNode.getEdgeNorthSouth() != null);
                //Debug.Log(currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber);
                //Debug.Log(currentNode.getEdgeNorthSouth().getEdgeType());
                //Debug.Log(state.getCurrentPlayer(playerNumber).playerNumber);
                //Debug.Log(currentNode.boardLocation);
                //Debug.Log(currentNode.getEdgeNorthSouth().edgeBoardLocation);
                //Debug.Log(currentNode.getEdgeNorthSouth() != currentEdge);

                while ((currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != currentEdge)
                    || (currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != currentEdge)
                    || (currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != currentEdge))
                {
                    currentNode.checkedLongestRoad = true;

                    currentLongestRoad++;
                    if ((currentNode.houseColour != 0)
                        && ((currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != currentEdge)
                        || (currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != currentEdge)
                        || (currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != currentEdge)
                        || (currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != currentEdge)
                        || (currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != currentEdge)
                        || (currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != currentEdge)))
                    {
                        Debug.Log("passes");
                        currentLongestRoad++;
                    }
                    if (currentLongestRoad > longetRoad)
                    {
                        Debug.Log("CC:" + currentLongestRoad);
                        longetRoad = currentLongestRoad;
                    }

                    Debug.Log("PN: " + playerNumber);
                    Debug.Log("CLR: " + currentLongestRoad);

                    if (currentNode.getEdgeNorthSouth() != null && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeNorthSouth() != currentEdge)
                    {
                        currentEdge = currentNode.getEdgeNorthSouth();
                        currentEdge.checkedLongestRoad = true;


                        if (currentEdge.getNode1() == currentNode)
                        {
                            currentNode = currentEdge.getNode2();
                            currentNode.checkedLongestRoad = true;
                        }
                        else if (currentEdge.getNode2() == currentNode)
                        {
                            currentNode = currentEdge.getNode1();
                            currentNode.checkedLongestRoad = true;
                        }
                    }
                    else if (currentNode.getEdgeWest() != null && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeWest() != currentEdge)
                    {
                        currentEdge = currentNode.getEdgeWest();
                        currentEdge.checkedLongestRoad = true;


                        if (currentEdge.getNode1() == currentNode)
                        {
                            currentNode = currentEdge.getNode2();
                            currentNode.checkedLongestRoad = true;
                        }
                        else if (currentEdge.getNode2() == currentNode)
                        {
                            currentNode = currentEdge.getNode1();
                            currentNode.checkedLongestRoad = true;
                        }
                    }
                    else if (currentNode.getEdgeEast() != null && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentNode.getEdgeEast() != currentEdge)
                    {
                        currentEdge = currentNode.getEdgeEast();
                        currentEdge.checkedLongestRoad = true;


                        if (currentEdge.getNode1() == currentNode)
                        {
                            currentNode = currentEdge.getNode2();
                            currentNode.checkedLongestRoad = true;
                        }
                        else if (currentEdge.getNode2() == currentNode)
                        {
                            currentNode = currentEdge.getNode1();
                            currentNode.checkedLongestRoad = true;
                        }
                    }
                }
            }
        }
    }

    //public void findLongestRoad()
    //{
    //    Node currentNode = null;
    //    Edge currentEdge = null;


    //    Debug.Log("YESSS");

    //    for (int j = 0; j < myboard.edgeList.Length; j++)
    //    {
    //        myboard.edgeList[j].checkedLongestRoad = false;
    //        myboard.boardNodes[j].checkedLongestRoad = false;
    //    }

    //    for (int i = 0; i < myboard.edgeList.Length; i++)
    //    {
    //        int currentLongestRoad = 0;
    //        if (myboard.edgeList[i].edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && myboard.edgeList[i].checkedLongestRoad == false)
    //        {
    //            currentEdge = myboard.edgeList[i];


    //            // Good to here
    //            //Debug.Log("Hits");
    //            //Debug.Log("NULL: " + currentNode.getEdgeNorthSouth() != null);
    //            //Debug.Log(currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber);
    //            //Debug.Log(currentNode.getEdgeNorthSouth().getEdgeType());
    //            //Debug.Log(state.getCurrentPlayer(playerNumber).playerNumber);
    //            //Debug.Log(currentNode.boardLocation);
    //            //Debug.Log(currentNode.getEdgeNorthSouth().edgeBoardLocation);
    //            //Debug.Log(currentNode.getEdgeNorthSouth() != currentEdge);

    //            while ((currentEdge.getNode1() != null && currentEdge.getNode1().houseColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode1() != currentNode)
    //                    || (currentEdge.getNode2() != null && currentEdge.getNode2().houseColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode2() != currentNode)
    //                    || (currentEdge.getNode1().getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode1().houseColour == 0)
    //                    || (currentEdge.getNode2().getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode2().houseColour == 0))

    //                currentEdge.checkedLongestRoad = true;

    //                currentLongestRoad++;

    //                if (currentLongestRoad > longetRoad)
    //                {
    //                    longetRoad = currentLongestRoad;
    //                }

    //                if (currentEdge.getNode1() != null && currentEdge.getNode1().houseColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode1() != currentNode)
    //                {
    //                    currentNode = currentEdge.getNode1();
    //                    currentNode.checkedLongestRoad = true;


    //                    if (currentNode.getEdgeNorthSouth() == currentEdge && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeEast().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeEast();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeNorthSouth() == currentEdge && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeWest().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeWest();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeEast() == currentEdge && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeNorthSouth().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeNorthSouth();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeEast() == currentEdge && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeWest().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeWest();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeWest() == currentEdge && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeNorthSouth().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeNorthSouth();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeWest() == currentEdge && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeEast().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeEast();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }

    //                else if (currentEdge.getNode2() != null && currentEdge.getNode1().houseColour == state.getCurrentPlayer(playerNumber).playerNumber && currentEdge.getNode2() != currentNode)
    //                {
    //                    currentNode = currentEdge.getNode2();
    //                    currentNode.checkedLongestRoad = true;


    //                    if (currentNode.getEdgeNorthSouth() == currentEdge && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeEast().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeEast();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeNorthSouth() == currentEdge && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeWest().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeWest();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeEast() == currentEdge && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeNorthSouth().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeNorthSouth();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeEast() == currentEdge && currentNode.getEdgeWest().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeWest().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeWest();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeWest() == currentEdge && currentNode.getEdgeNorthSouth().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeNorthSouth().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeNorthSouth();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                    else if (currentNode.getEdgeWest() == currentEdge && currentNode.getEdgeEast().edgeColour == state.getCurrentPlayer(playerNumber).playerNumber
    //                    && currentNode.getEdgeEast().checkedLongestRoad == false)
    //                    {
    //                        currentEdge = currentNode.getEdgeEast();
    //                        currentEdge.checkedLongestRoad = true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}



    public void setObj()
    {
        Debug.Log("called");
        if (myboard.introTurn == true)
        {
            string tempObj = previousObj;
            previousObj = currentObj;
            currentObj = tempObj;
        }
        else
        {
            if (previousObj == "road" && currentObj == "road")
            {
                previousObj = currentObj;
                currentObj = "settlement";
            }
            else if (currentObj == "settlement")
            {
                previousObj = currentObj;
                currentObj = "road";
            }
        }
    }

    public void takeTurn()
    {
        Debug.Log("LATEST: " + currentObj);
        if (myboard.introTurn == false)
        {
            diceRoller.RollDice();
        }
        if (currentObj == "road")
        {
            for (int i = 0; i < myboard.boardNodes.Length; i++)
            {
                if (myboard.boardNodes[i].houseColour == playerNumber)
                {
                    if (myboard.boardNodes[i].getEdgeNorthSouth() != null && myboard.boardNodes[i].getEdgeNorthSouth().edgeColour == 0)
                    {
                        buildRoad(myboard.boardNodes[i].getEdgeNorthSouth().road);
                        setObj();
                    }
                    else if (myboard.boardNodes[i].getEdgeWest() != null && myboard.boardNodes[i].getEdgeWest().edgeColour == 0)
                    {
                        buildRoad(myboard.boardNodes[i].getEdgeWest().road);
                        setObj();
                    }
                    else if (myboard.boardNodes[i].getEdgeEast() != null && myboard.boardNodes[i].getEdgeEast().edgeColour == 0)
                    {
                        buildRoad(myboard.boardNodes[i].getEdgeEast().road);
                        setObj();
                    }

                }
                else
                {
                    for (int j = 0; j < myboard.edgeList.Length; j++)
                    {
                        if (myboard.edgeList[j].edgeColour == playerNumber)
                        {
                            if (myboard.edgeList[j].getNode1() != null && myboard.edgeList[j].getNode1().houseColour == playerNumber)
                            {
                                if (myboard.edgeList[j].getNode2().getEdgeNorthSouth() != null && myboard.edgeList[j].getNode2().getEdgeNorthSouth().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode2().getEdgeNorthSouth().road);
                                    setObj();
                                }
                                else if (myboard.edgeList[j].getNode2().getEdgeEast() != null && myboard.edgeList[j].getNode2().getEdgeEast().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode2().getEdgeEast().road);
                                    setObj();
                                }
                                else if (myboard.edgeList[j].getNode2().getEdgeWest() != null && myboard.edgeList[j].getNode2().getEdgeWest().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode2().getEdgeWest().road);
                                    setObj();
                                }
                            }
                            else if (myboard.edgeList[j].getNode2() != null && myboard.edgeList[j].getNode2().houseColour == playerNumber)
                            {
                                if (myboard.edgeList[j].getNode1().getEdgeNorthSouth() != null && myboard.edgeList[j].getNode1().getEdgeNorthSouth().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode1().getEdgeNorthSouth().road);
                                    setObj();
                                }
                                else if (myboard.edgeList[j].getNode1().getEdgeEast() != null && myboard.edgeList[j].getNode1().getEdgeEast().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode1().getEdgeEast().road);
                                    setObj();
                                }
                                else if (myboard.edgeList[j].getNode1().getEdgeWest() != null && myboard.edgeList[j].getNode1().getEdgeWest().edgeColour != playerNumber)
                                {
                                    buildRoad(myboard.edgeList[j].getNode1().getEdgeWest().road);
                                    setObj();
                                }
                            }
                        }
                    }
                }
            }
        }
        else if (currentObj == "settlement")
        {
            for (int i = 0; i < myboard.boardNodes.Length; i++)
            {
                int randomInt = Random.Range(0, 53);
                if (myboard.boardNodes[randomInt].houseColour == 0)
                {
                    if (myboard.introTurn == true)
                    {
                        buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                        setObj();
                    }
                    else
                    {
                        if (myboard.boardNodes[randomInt].getEdgeNorthSouth() != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().edgeColour == playerColour)
                        {
                            if (myboard.boardNodes[randomInt].getEdgeNorthSouth().node1 != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().node1 == myboard.boardNodes[randomInt])
                            {
                                if (myboard.boardNodes[randomInt].getEdgeNorthSouth().node2 != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().node2.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                            else
                            {
                                if (myboard.boardNodes[randomInt].getEdgeNorthSouth().node1 != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().node1.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                        }
                        else if (myboard.boardNodes[randomInt].getEdgeEast() != null && myboard.boardNodes[randomInt].getEdgeEast().edgeColour == playerColour)
                        {
                            if (myboard.boardNodes[randomInt].getEdgeEast().node1 != null && myboard.boardNodes[randomInt].getEdgeEast().node1 == myboard.boardNodes[randomInt])
                            {
                                if (myboard.boardNodes[randomInt].getEdgeEast().node2 != null && myboard.boardNodes[randomInt].getEdgeEast().node2.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                            else
                            {
                                if (myboard.boardNodes[randomInt].getEdgeNorthSouth().node1 != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().node1.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                        }
                        else if (myboard.boardNodes[randomInt].getEdgeWest() != null && myboard.boardNodes[randomInt].getEdgeWest().edgeColour == playerColour)
                        {
                            if (myboard.boardNodes[randomInt].getEdgeWest().node1 != null && myboard.boardNodes[randomInt].getEdgeWest().node1 == myboard.boardNodes[randomInt])
                            {
                                if (myboard.boardNodes[randomInt].getEdgeWest().node2 != null && myboard.boardNodes[randomInt].getEdgeWest().node2.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                            else
                            {
                                if (myboard.boardNodes[randomInt].getEdgeNorthSouth().node1 != null && myboard.boardNodes[randomInt].getEdgeNorthSouth().node1.houseColour == 0)
                                {
                                    buildSettlementCity(myboard.boardNodes[randomInt].settlementHex);
                                    setObj();
                                }
                            }
                        }
                    }
                }
            }
        }
        //state.SwitchState();
    }
}




