using System;
using UnityEngine;

public class Player
{
    PlayerStateManager state;
    Board myboard;

    // A single variable to hold the players points (Victory points)
    public int victoryPoints;

    // A player numbner vaiable to differentiate the players
    public int playerNumber;

    

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

    int settlementIntroCounter = 0;
    public void buildSettlementCity(int nodeIndex)
    {
        Debug.Log("METHOD CALLED");
        if (settlementIntroCounter < 8)
        {
            settlementIntroCounter++;
        }
        // Nothing built on this node
        if (myboard.boardNodes[nodeIndex].houseColour == 0)
        {
            if (myboard.boardNodes[nodeIndex].getNodeNorthSouth().getHouseType() == 0
                && myboard.boardNodes[nodeIndex].getNodeWest().getHouseType() == 0
                && myboard.boardNodes[nodeIndex].getNodeEast().getHouseType() == 0)
            {
                if (myboard.introTurn)
                {
                    if (settlementIntroCounter == 8)
                    {
                        // Instantiate final blue intro settlement here

                        victoryPoints++;

                        // New pop up to say "Player 4 please build a road"
                        Debug.Log("Player 4 please build a road");

                        myboard.placedIntroSettlements = true;
                    }
                    // Instantiate settlement object at this node position

                    else if (settlementIntroCounter == 1 || settlementIntroCounter == 5)
                    {
                        // Instantiate a white settlement at the given node

                        victoryPoints++;

                        // New pop up to say "Player 1 please build a road"
                        Debug.Log("Player 1 please build a road");
                    }

                    else if (settlementIntroCounter == 2 || settlementIntroCounter == 6)
                    {
                        // Instantiate a red settlement at the given node

                        victoryPoints++;

                        // New pop up to say "Player 2 please build a road"
                        Debug.Log("Player 2 please build a road");
                    }
                    else if (settlementIntroCounter == 3 || settlementIntroCounter == 7)
                    {
                        // Instantiate a yellow settlement at the given node

                        victoryPoints++;

                        // New pop up to say "Player 3 please build a road"
                        Debug.Log("Player 3 please build a road");
                    }
                    else if (settlementIntroCounter == 4)
                    {
                        // Instantiate a blue settlement at the given node

                        victoryPoints++;

                        // New pop up to say "Player 4 please build a road"
                        Debug.Log("Player 4 please build a road");
                    }
                }
                else
                {
                    if (currencyLumber >= 1 && currencyBrick >= 1 && currencyGrain >= 1 && currencyWool >= 1
                        && (myboard.boardNodes[nodeIndex].getEdgeNorthSouth().getEdgeType() == getPlayerNumber()
                        || myboard.boardNodes[nodeIndex].getEdgeWest().getEdgeType() == getPlayerNumber()
                        || myboard.boardNodes[nodeIndex].getEdgeEast().getEdgeType() == getPlayerNumber()))
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
        }
        // Builds city
        else if (currencyGrain >= 2 && currencyOre >= 3
            && myboard.boardNodes[nodeIndex].houseColour == playerNumber
            && myboard.boardNodes[nodeIndex].houseType == 1)
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

    int roadIntroCounter = 0;
    public void buildRoad(Edge e)
    {
        if (roadIntroCounter < 8)
        {
            roadIntroCounter++;
        }
        // Nothing built on this node
        if (e.edgeColour == 0)
        {
            // If the either of the two nodes of the inputted edge has a settlement or city of the same players number
            if (e.getNode1().getHouseColour() == getPlayerNumber() || e.getNode2().getHouseType() == getPlayerNumber())
            {
                if (myboard.introTurn)
                {
                    if (roadIntroCounter == 8)
                    {
                        // Instantiate final blue intro road here

                        myboard.placedIntroRoads = true;
                        myboard.introTurn = false;
                    }
                    // Instantiate road object at this node position

                    else if (roadIntroCounter == 1 || roadIntroCounter == 5)
                    {
                        // Instantiate a white road at the given edge

                        state.SwitchState();

                        // New pop up to say "Player 2 please build a settlement"
                        Debug.Log("Player 2 please build a Settlement");
                    }

                    else if (roadIntroCounter == 2 || roadIntroCounter == 6)
                    {
                        // Instantiate a red road at the given edge

                        // New pop up to say "Player 3 please build a settlement"
                        Debug.Log("Player 3 please build a settlement");
                    }
                    else if (roadIntroCounter == 3 || roadIntroCounter == 7)
                    {
                        // Instantiate a yellow road at the given edge

                        // New pop up to say "Player 4 please build a settlement"
                        Debug.Log("Player 4 please build a settlement");
                    }
                    else if (roadIntroCounter == 4)
                    {
                        // Instantiate a blue road at the given edge

                        // New pop up to say "Player 1 please build a settlement"
                        Debug.Log("Player 1 please build a settlement");
                    }
                }
                else
                {
                    if (currencyLumber >= 1 && currencyBrick >= 1
                        && (e.getNode1().getHouseColour() == getPlayerNumber()
                        || e.getNode2().getHouseType() == getPlayerNumber()))
                        
                    {
                        currencyLumber -= 1;
                        currencyBrick -= 1;
                       

                        // Instantiate road object at this node position / activate road that will already be created there (at every edge)
                    }
                    else
                    {
                        Debug.Log("Cannot build here");
                    }
                }
            }
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




