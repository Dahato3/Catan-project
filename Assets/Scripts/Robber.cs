using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Robber : MonoBehaviour
{
    [SerializeField] GameObject board;
    Board myBoard;

    [SerializeField] GameObject dice;
    DiceRoller diceRoller;

    [SerializeField] GameObject player;
    PlayerStateManager playerState;

    public GameObject robber;
    public GameObject robberN;

    //[SerializeField] GameObject robber;


    public void Awake()
    {
        robber = (GameObject)Resources.Load("robber");
        robberN = (GameObject)Resources.Load("robber");

        board = GameObject.Find("Board");
        myBoard = board.GetComponent<Board>();

        dice = GameObject.Find("DiceRolls");
        diceRoller = dice.GetComponent<DiceRoller>();

        player = GameObject.Find("End Turn Button");
        playerState = player.GetComponent<PlayerStateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {

        if (GameObject.Find("initialRobber") == null)
        {
            GameObject robberT = Instantiate(robber, new Vector3(0, 0, 0), Quaternion.identity);
            robberT.name = "initialRobber";
            robberT.transform.position = new Vector3(0, 17.5f, 0);
            robberT.transform.localScale = new Vector3(30, 1.75f, 30);
            robberT.GetComponent<MeshRenderer>().enabled = true;
        }

        //gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Node[] playersAroundRobber = new Node[3];
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (myBoard.introTurn == true || diceRoller.diceTotal != 7)
            {
                return;
            }
            if (GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled == true)
            {
                GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled = false;
            }
            Debug.Log(gameObject.name + " Clicked!");

            if (GameObject.Find("robberMain") == null)
            {
                robberN = Instantiate(robber);
                robberN.GetComponent<MeshRenderer>().enabled = true;
                robberN.name = "robberMain";
                
            }

            robberN.transform.position = gameObject.transform.position;

            Vector3 temp = robberN.transform.position;
            Debug.Log(temp);
            temp[0] = temp[0] - 100;
            temp[1] = temp[1] + 17.5f;

            robberN.transform.position = temp;
            robberN.transform.localScale = new Vector3(30, 1.75f, 30);


            int hexPosition = int.Parse(gameObject.name);

            

            
            int pCounter = 0;

            for (int i = 0; i < myBoard.boardNodes.Length; i++)
            {
                Debug.Log("HexPosition" + hexPosition);
                Debug.Log(myBoard.boardNodes[i].lHexLocation);
                Debug.Log(myBoard.boardNodes[i].rHexLocation);
                Debug.Log(myBoard.boardNodes[i].oHexLocation);
                if (myBoard.boardNodes[i].getSettlementHex().GetComponent<MeshRenderer>().enabled == true
                    && (myBoard.boardNodes[i].lHexLocation == hexPosition
                    || myBoard.boardNodes[i].rHexLocation == hexPosition
                    || myBoard.boardNodes[i].oHexLocation == hexPosition))
                {
                    Debug.Log("passed");
                    myBoard.boardNodes[i].hasRobber = true;

                    if (myBoard.boardNodes[i].houseColour != playerState.getCurrentPlayer(playerState.currentPlayerNumber).playerNumber)
                    {
                        Debug.Log("pCounter: " + pCounter);
                        playersAroundRobber[pCounter] = myBoard.boardNodes[i];
                        pCounter++;
                    }
                   
                }
                myBoard.boardNodes[i].hasRobber = false;
            }
            Debug.Log("POS0: " + playersAroundRobber[0]);
            Debug.Log("POS1: " + playersAroundRobber[1]);
            Debug.Log("POS2: " + playersAroundRobber[2]);
            if (playersAroundRobber[1] == null)
            {
                int randResource = Random.Range(0, 5);

                if (randResource == 1)
                {
                    playerState.getCurrentPlayer(playersAroundRobber[0].houseColour).currencyLumber--;
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyLumber++;
                }
                if (randResource == 2)
                {
                    playerState.getCurrentPlayer(playersAroundRobber[0].houseColour).currencyGrain--;
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyGrain++;
                }
                if (randResource == 3)
                {
                    playerState.getCurrentPlayer(playersAroundRobber[0].houseColour).currencyBrick--;
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyBrick++;
                }
                if (randResource == 4)
                {
                    playerState.getCurrentPlayer(playersAroundRobber[0].houseColour).currencyOre--;
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyOre++;
                }
                if (randResource == 5)
                {
                    playerState.getCurrentPlayer(playersAroundRobber[0].houseColour).currencyWool--;
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyWool++;
                }

            }



            Debug.Log("Select a player to steal a random resource");



            // Got the base functionality working
            // BUT hasRobber doesnt stop resources being allocated
            // AND still need to be able to move robber again

            // Need to also implement the stealing 1 resource when placed


        }
    }
}
