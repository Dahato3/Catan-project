using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    GameObject PlayerState;
    PlayerStateManager state;

    GameObject board;
    Board myboard;

    // A single variable to hold the players points (Victory points)
    public int victoryPoints;

    public int playerNumber;
    public int playerColour;
    public int totalResources;

    public string[] inventory;
    public bool freeBuild = false;

    // A variable to store the amount of each resource individually
    // The type indicated the integer value that will internally represent each resource
    public int currencyLumber; // Type 1
    public int currencyGrain; // Type 2
    public int currencyBrick; // Type 3
    public int currencyOre; // Type 4
    public int currencyWool; // Type 5

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

        inventory = new string[14];

        board = GameObject.Find("Board");
        myboard = board.GetComponent<Board>();

        PlayerState = GameObject.Find("End Turn Button");
        state = PlayerState.GetComponent<PlayerStateManager>();
        
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

    public void buyDevelopmentCard()
    {
        if (currencyWool >= 1 && currencyOre >= 1 && currencyWool >= 1)
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
                int i = 0;
                while (inventory[i] != "")
                {
                    i++;
                }
                inventory[i] = setCard;
            }
            else if (setCard == "road")
            {
                Debug.Log("Please build 2 road for free");

                freeBuild = true;
            }
            else if (setCard == "yearofplenty")
            {
                // panel to select 2 resources from bank
            }
            else if (setCard == "monopoly")
            {
                // player choose a resource, gets all resources from all players of
                // their chose type
            }
            else
            {
                victoryPoints++;
            }
        }
        Debug.Log("Not enough resources");
    }



    // TODO: 2 elements NOT yet implemented - roll dice to see who places first AND 2nd round of placing
    // is meant to be in reverse order

    
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
                            // Instantiate a white settlement at the given node

                            myboard.boardNodes[cNode.boardLocation].settlementHex.GetComponent<MeshRenderer>().enabled = true;
                            myboard.boardNodes[cNode.boardLocation].houseType = 1;
                            myboard.boardNodes[cNode.boardLocation].houseColour = 1;

                            Debug.Log("r: " + myboard.boardNodes[cNode.boardLocation].rHexResource);
                            Debug.Log(myboard.boardNodes[cNode.boardLocation].lHexResource);
                            Debug.Log(myboard.boardNodes[cNode.boardLocation].oHexResource);

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

                            // New pop up to say "Player 1 please build a road"
                            Debug.Log("Player 1 please build a road");
                        }

                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            // Instantiate a red settlement at the given node

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

                            // New pop up to say "Player 2 please build a road"
                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            // Instantiate a yellow settlement at the given node

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

                            // New pop up to say "Player 3 please build a road"
                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            // Instantiate a blue settlement at the given node

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

                            // New pop up to say "Player 4 please build a road"
                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    // Case where we build a seetlement, not apart of the intro
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber()))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

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
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    // build city
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
                            // Instantiate a white settlement at the given node

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

                            // New pop up to say "Player 1 please build a road"
                            Debug.Log("Player 1 please build a road");
                        }
                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            // Instantiate a red settlement at the given node

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

                            // New pop up to say "Player 2 please build a road"
                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            // Instantiate a yellow settlement at the given node

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

                            // New pop up to say "Player 3 please build a road"
                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            // Instantiate a blue settlement at the given node

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

                            // New pop up to say "Player 4 please build a road"
                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeWest().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber()))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            // Instantiate settlement object at this node position / activate node that will already be created there (at every node)
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
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    // build city
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
                            // Instantiate a white settlement at the given node

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

                            // New pop up to say "Player 1 please build a road"
                            Debug.Log("Player 1 please build a road");
                        }

                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            // Instantiate a red settlement at the given node

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

                            // New pop up to say "Player 2 please build a road"
                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            // Instantiate a yellow settlement at the given node

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

                            // New pop up to say "Player 3 please build a road"
                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            // Instantiate a blue settlement at the given node

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

                            // New pop up to say "Player 4 please build a road"
                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeEast().getEdgeType() == getPlayerNumber()))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            // Instantiate settlement object at this node position / activate node that will already be created there (at every node)
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
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    // build city
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
                            // Instantiate a white settlement at the given node

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

                            // New pop up to say "Player 1 please build a road"
                            Debug.Log("Player 1 please build a road");
                        }

                        else if (myboard.introCounter == 2 || myboard.introCounter == 10)
                        {
                            // Instantiate a red settlement at the given node

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

                            // New pop up to say "Player 2 please build a road"
                            Debug.Log("Player 2 please build a road");
                        }
                        else if (myboard.introCounter == 4 || myboard.introCounter == 12)
                        {
                            // Instantiate a yellow settlement at the given node

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

                            // New pop up to say "Player 3 please build a road"
                            Debug.Log("Player 3 please build a road");
                        }
                        else if (myboard.introCounter == 6 || myboard.introCounter == 14)
                        {
                            // Instantiate a blue settlement at the given node

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

                            // New pop up to say "Player 4 please build a road"
                            Debug.Log("Player 4 please build a road");
                        }
                    }
                    else
                    {
                        if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                            && (cNode.getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                            || cNode.getEdgeWest().getEdgeType() == getPlayerNumber()))
                        {
                            currencyLumber -= 1;
                            currencyBrick -= 1;
                            currencyGrain -= 1;
                            currencyWool -= 1;

                            victoryPoints++;

                            // Instantiate settlement object at this node position / activate node that will already be created there (at every node)
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
            else if (cNode.houseColour == playerNumber && myboard.introTurn == false)
            {
                if (currencyGrain >= 2 && currencyOre >= 3)
                {
                    // build city
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

            victoryPoints++;

            // Change the model to a city from a settlement
        }
        else
        {
            Debug.Log("Cannot build here");
        }

    }

    int freeBuildCounter = 0;
    public void buildRoad(GameObject g)
    {
        if (myboard.introCounter % 2 == 0 && myboard.introTurn == true)
        {
            Debug.Log("Cannot build a road yet");
            return;
        }
        Edge cEdge = myboard.getCurrentEdge(g);
        Debug.Log("Intro counter: " + myboard.introCounter);
        // Nothing built on this node
        if (cEdge.edgeColour == 0)
        {
            //Debug.Log(cEdge.getNode1());
            //Debug.Log(cEdge.getNode2());

            //Debug.Log("N1 boardLoc: " + cEdge.getNode1().boardLocation);
            //Debug.Log("N2 boardLoc: " + cEdge.getNode2().boardLocation);

            //Debug.Log("N1 east boardLoc: " + cEdge.getNode1().getEdgeEast().edgeBoardLocation);
            //Debug.Log("N1 west boardLoc: " + cEdge.getNode1().getEdgeWest().edgeBoardLocation);
            //Debug.Log("N1 northSouth boardLoc: " + cEdge.getNode1().getEdgeNorthSouth().edgeBoardLocation);

            //Debug.Log("N1 east boardLoc: " + cEdge.getNode1().getEdgeEast().edgeBoardLocation);
            //Debug.Log("N1 west boardLoc: " + cEdge.getNode1().getEdgeWest().edgeBoardLocation);
            //Debug.Log("N1 northSouth boardLoc: " + cEdge.getNode1().getEdgeNorthSouth().edgeBoardLocation);

            //Debug.Log("N2 east: " + cEdge.getNode2().getEdgeEast().edgeColour);
            //Debug.Log("N2 west: " + cEdge.getNode2().getEdgeWest().edgeColour);
            //Debug.Log("N2 northSouth: " + cEdge.getNode2().getEdgeNorthSouth().edgeColour);

            // If the either of the two nodes of the inputted edge has a settlement or city of the same players number
            if (cEdge.getNode1().getHouseColour() == state.currentPlayerNumber || cEdge.getNode2().getHouseColour() == state.currentPlayerNumber
                || cEdge.getNode1().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeNorthSouth.edgeColour == state.currentPlayerNumber
                || cEdge.getNode2().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeNorthSouth.edgeColour == state.currentPlayerNumber)

            {
                if (myboard.introTurn)
                {
                    if (myboard.introCounter == 1 || myboard.introCounter == 9)
                    {
                        // Instantiate a white road at the given edge

                        Debug.Log("First road build");
                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        myboard.introCounter++;
                        state.SwitchState();

                        // New pop up to say "Player 2 please build a settlement"
                        Debug.Log("Player 2 please build a Settlement");
                    }

                    else if (myboard.introCounter == 3 || myboard.introCounter == 11)
                    {
                        // Instantiate a red road at the given edge

                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        myboard.introCounter++;
                        state.SwitchState();

                        // New pop up to say "Player 3 please build a settlement"
                        Debug.Log("Player 3 please build a settlement");
                    }
                    else if (myboard.introCounter == 5 || myboard.introCounter == 13)
                    {
                        // Instantiate a yellow road at the given edge

                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        myboard.introCounter++;
                        state.SwitchState();

                        // New pop up to say "Player 4 please build a settlement"
                        Debug.Log("Player 4 please build a settlement");
                    }
                    else if (myboard.introCounter == 7 || myboard.introCounter == 15)
                    {
                        // Instantiate a blue road at the given edge

                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = state.currentPlayerNumber;

                        myboard.introCounter++;

                        // New pop up to say "Player 1 please build a settlement"
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
                            GameObject.Find("Resources").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("RollDiceButton").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("PlayerTrade").GetComponent<CanvasGroup>().alpha = 1f;
                            GameObject.Find("BuildingCosts").GetComponent<CanvasGroup>().alpha = 1f;

                            GameObject.Find("CurrentPlayer").transform.position = new Vector3(750, -40, 0);
                            GameObject.Find("CurrentPlayer").GetComponent<Text>().fontSize = 20;
                        }
                        state.SwitchState();


                    }
                }
                else
                {
                    if (freeBuild == true)
                    {
                        freeBuildCounter++;
                        if (freeBuildCounter == 2)
                        {
                            freeBuild = false;
                        }
                    }

                    else if (currencyLumber >= 1 && currencyBrick >= 1
                        && (cEdge.getNode1().getHouseColour() == getPlayerNumber()
                        || cEdge.getNode2().getHouseType() == getPlayerNumber()
                        || cEdge.getNode1().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode1().edgeNorthSouth.edgeColour == state.currentPlayerNumber
                        || cEdge.getNode2().edgeEast.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeWest.edgeColour == state.currentPlayerNumber || cEdge.getNode2().edgeNorthSouth.edgeColour == state.currentPlayerNumber))

                    {
                        currencyLumber -= 1;
                        currencyBrick -= 1;

                        myboard.edgeList[cEdge.edgeBoardLocation].road.GetComponent<MeshRenderer>().enabled = true;
                        myboard.edgeList[cEdge.edgeBoardLocation].edgeColour = playerNumber;

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






    // Here are various methods to build. One for building a road, a settlement and city.

    // TODO: add code to node class to store road positions

    // Roads take two nodes as parameters, being the two nodes the road is built between
    // We check if the player has enough resources, if so we deduct and
    public void buildRoad(Node n1, Node n2)
    {
        if (currencyLumber >= 1 && currencyBrick >= 1)
        {
            //TODO: Still need to implement a check for if building a road if possible.
            // Need to also decide how nodes will hold building data - houseType, houseColour.
        }
        else
        {
            Debug.Log("Insufficent resourses");
        }
        // If building is suitable -
        currencyLumber -= 1;
        currencyBrick -= 1;
    }

    // Settlements take one node as a parameter, being the node the settlement is built on
    // We check if the player has enough resources, if so we deduct and and set the node correctly
    public void buildSettlement(Node n)
    {
        if (n.houseType != 0)
        {
            Debug.Log("A house is already build here!");
            return;
        }



        if (currencyLumber >= 0 && currencyBrick >= 0 && currencyGrain >= 0 && currencyWool >= 0)
        {


            //TODO: Still need to implement a check for if building a settlement if possible - other players built there, if you have anything buil there / nearby
            // Need to also decide how nodes will hold building data - houseType, houseColour.
            n.houseType = 1;
            n.houseColour = getPlayerNumber();
            // May need to declare the players colour aswell

            //currencyLumber -= 1;
            //currencyBrick -= 1;
            //currencyGrain -= 1;
            //currencyWool -= 1;

            //victoryPoints += 1;

        }
        else
        {
            Debug.Log("Insufficent resourses");
        }
        // If building is suitable -
        
    }

    // Citys take one node as a parameter, being the node the city is built on
    // We check if the player has enough resources, if so we deduct and and set the node correctly
    public void buildCity(Node n)
    {
        if (currencyGrain >= 2 && currencyOre >= 3)
        {
            //TODO: Still need to implement a check for if building a city if possible - no other players build here, YOU need a settlement build to upgrade to city
            // Need to also decide how nodes will hold building data - houseType, houseColour.
            n.houseType = 2;
            n.houseColour = getPlayerNumber();
            // May need to declare the players colour aswell
        }
        else
        {
            Debug.Log("Insufficent resourses");
        }
        // If building is suitable -
        currencyGrain -= 2;
        currencyOre -= 3;

        // Essentially gains 1 VP
        victoryPoints -= 1;
        victoryPoints += 2;
    }

    // 
    public void getDevelopmentCard()
    {
        if (currencyGrain >= 1 && currencyWool >= 1 && currencyOre >= 1)
        {
            //TODO: Still need to implement how development card will be created, stored and accessed
        }
        else
        {
            Debug.Log("Insufficent resourses");
        }
        currencyGrain -= 1;
        currencyWool -= 1;
        currencyOre -= 1;

        //TODO: Need to actually update the development card storage


    }
}




