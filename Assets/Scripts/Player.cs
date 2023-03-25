using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    
    public int victoryPoints;
    public int currencyLumber; // Type 1
    public int currencyGrain; // Type 2
    public int currencyBrick; // Type 3
    public int currencyOre; // Type 4
    public int currencyWool; // Type 5

    public int playerNumber;

    Dictionary<Node, Node> roadNodes;

    public Player()
    {
        victoryPoints = 0;
        currencyLumber = 0;
        currencyGrain = 0;
        currencyBrick = 0;
        currencyOre = 0;
        currencyWool = 0;
    }

    //private void Start()
    //{
    //    victoryPoints = 0;
    //    currencyLumber = 0;
    //    currencyGrain = 0;
    //    currencyBrick = 0;
    //    currencyOre = 0;
    //    currencyWool = 0;
    //}

    //private void Update()
    //{

    //}

    public void setPlayerNumber(int playerNumberr)
    {
        playerNumber = playerNumberr;
    }

    public int getPlayerNumber()
    {
        return playerNumber;
    }

    public void addResources(int amount, int type)
    {
        if (type == 1)
        {
            currencyLumber += amount;
        }
        else if (type == 2)
        {
            currencyGrain += amount;
        }
        else if (type == 3)
        {
            currencyBrick += amount;
        }
        else if (type == 4)
        {
            currencyOre += amount;
        }
        else if (type == 1)
        {
            currencyWool += amount;
        }
    }

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

    public void buildSettlement(Node n)
    {
        if (currencyLumber >= 0 && currencyBrick >= 0 && currencyGrain >= 0 && currencyWool >= 0)
        {
            //TODO: Still need to implement a check for if building a settlement if possible - other players built there, if you have anything buil there / nearby
            // Need to also decide how nodes will hold building data - houseType, houseColour.

            n.houseType = getPlayerNumber();
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

    public void buildCity(Node n)
    {
        if (currencyGrain >= 2 && currencyOre >= 3)
        {
            //TODO: Still need to implement a check for if building a city if possible - no other players build here, YOU need a settlement build to upgrade to city
            // Need to also decide how nodes will hold building data - houseType, houseColour.

            n.houseType = getPlayerNumber();
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




