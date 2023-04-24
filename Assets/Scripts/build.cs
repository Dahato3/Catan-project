using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class build : MonoBehaviour
{
    [SerializeField] GameObject playerState;
    PlayerStateManager state;

    [SerializeField] GameObject dice;
    DiceRoller diceRoller;

    [SerializeField] GameObject rob;
    Robber robber;


    void Awake()
    {
        playerState = GameObject.Find("End Turn Button");
        state = playerState.GetComponent<PlayerStateManager>();

        dice = GameObject.Find("DiceRolls");
        diceRoller = dice.GetComponent<DiceRoller>();

        rob = GameObject.Find("1");
        robber = rob.GetComponent<Robber>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() && gameObject.name != "settlementCity(Clone)" && gameObject.name != "road(Clone)")
            {
                return;
            }
            if (diceRoller.diceTotal == 7)
            {
                if (robber.playersAroundRobber[0].getSettlementHex() == gameObject)
                {
                    int randResource = Random.Range(0, 5);

                    if (randResource == 1)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[0].houseColour).currencyLumber--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyLumber++;
                    }
                    if (randResource == 2)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[0].houseColour).currencyGrain--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyGrain++;
                    }
                    if (randResource == 3)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[0].houseColour).currencyBrick--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyBrick++;
                    }
                    if (randResource == 4)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[0].houseColour).currencyOre--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyOre++;
                    }
                    if (randResource == 5)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[0].houseColour).currencyWool--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyWool++;
                    }
                }
                else if (robber.playersAroundRobber[1].getSettlementHex() == gameObject)
                {
                    int randResource = Random.Range(0, 5);

                    if (randResource == 1)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[1].houseColour).currencyLumber--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyLumber++;
                    }
                    if (randResource == 2)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[1].houseColour).currencyGrain--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyGrain++;
                    }
                    if (randResource == 3)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[1].houseColour).currencyBrick--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyBrick++;
                    }
                    if (randResource == 4)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[1].houseColour).currencyOre--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyOre++;
                    }
                    if (randResource == 5)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[1].houseColour).currencyWool--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyWool++;
                    }
                }
                else if (robber.playersAroundRobber[2].getSettlementHex() == gameObject)
                {
                    int randResource = Random.Range(0, 5);

                    if (randResource == 1)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[2].houseColour).currencyLumber--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyLumber++;
                    }
                    if (randResource == 2)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[2].houseColour).currencyGrain--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyGrain++;
                    }
                    if (randResource == 3)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[2].houseColour).currencyBrick--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyBrick++;
                    }
                    if (randResource == 4)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[2].houseColour).currencyOre--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyOre++;
                    }
                    if (randResource == 5)
                    {
                        state.getCurrentPlayer(robber.playersAroundRobber[2].houseColour).currencyWool--;
                        state.getCurrentPlayer(state.currentPlayerNumber).currencyWool++;
                    }
                }
            }
            if (state.currentPlayerNumber == 1)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (state.currentPlayerNumber == 2)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else if (state.currentPlayerNumber == 3)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else if (state.currentPlayerNumber == 4)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }


            if (gameObject.tag == "settlementCity")
            {
                state.getCurrentPlayer(state.currentPlayerNumber).buildSettlementCity(gameObject);
            }
            //else if (gameObject.tag == "city")
            //{
            //    state.getCurrentPlayer(state.currentPlayerNumber).buildSettlementCity(gameObject);
            //}
            else if (gameObject.tag == "road")
            {
                state.getCurrentPlayer(state.currentPlayerNumber).buildRoad(gameObject);
            }
            else
            {
                return;
            }
           
        }
    }
}
