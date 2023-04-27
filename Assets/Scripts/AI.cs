using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
public class AI : Player
{

    GameObject PlayerState;
    PlayerStateManager state;

    GameObject board;
    Board myboard;

    public AI()
    {
        // Initalizies each variable
        victoryPoints = 0;
        currencyLumber = 0;
        currencyGrain = 0;
        currencyBrick = 0;
        currencyOre = 0;
        currencyWool = 0;

        board = GameObject.Find("Board");
        myboard = board.GetComponent<Board>();

        PlayerState = GameObject.Find("End Turn Button");
        state = PlayerState.GetComponent<PlayerStateManager>();

    }


    
}
