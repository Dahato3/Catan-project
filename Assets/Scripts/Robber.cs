using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Robber : MonoBehaviour
{
    // Variables to help access properties from other classes
    [SerializeField] GameObject board;
    [SerializeField] GameObject dice;
    [SerializeField] GameObject player;
    Board myBoard;
    DiceRoller diceRoller;
    PlayerStateManager playerState;

    public GameObject robber;
    public GameObject robberN;

    public GameObject stealResourcePanel;

    public GameObject stealPlayer1Button;
    public GameObject stealPlayer2Button;
    public GameObject stealPlayer3Button;
    public GameObject stealPlayer4Button;

    public Node[] playersAroundRobber = new Node[3];

    static int robberMovedCount = 0;
    static int previousRobberMovedCountValue = 0;
    int randResource;

    // Awake function called before start to initialse the GameObject we use to access other classes
    public void Awake()
    {
        stealResourcePanel = GameObject.Find("StealResourcePanel");

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
    // Here, we check to see if we have already instantiated the initial robber game object, if not, we instantiate it adn if we have we skip this part.
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method detects if the gameobject the script is linked to has had the mouse / cursor move over it, it then only proceeds if the mouse when clicked.
    // If so a few things happen, we check which robber is being created (initial / new) and if it's new one we destroy the origninal, instantiate a new one and
    // take the position for it to be moved to from the clicked object.
    // Finally, we identify if and who has settlements / cities around the hex the robber has been moved to so we can easily identify them later.
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (myBoard.robberMoved == true)
            {
                return;
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                if (myBoard.introTurn == true || diceRoller.diceTotal != 7)
                {
                    return;
                }
                if (GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled == true)
                {
                    GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled = false;
                }

                if (GameObject.Find("robberMain") == null)
                {
                    robberN = Instantiate(robber);
                    robberN.GetComponent<MeshRenderer>().enabled = true;
                    robberN.name = "robberMain";

                }
                else if (GameObject.Find("robberMain") != null && robberMovedCount > previousRobberMovedCountValue)
                {
                    Destroy(GameObject.Find("robberMain"));
                    robberN = Instantiate(robber);
                    robberN.GetComponent<MeshRenderer>().enabled = true;
                    robberN.name = "robberMain";
                }

                Vector3 temp = gameObject.transform.position;

                temp[0] = temp[0] - 100;
                temp[1] = temp[1] + 17.5f;

                robberN.transform.position = temp;

                robberN.transform.localScale = new Vector3(30, 1.75f, 30);

                previousRobberMovedCountValue = robberMovedCount;
                robberMovedCount++;

                myBoard.robberMoved = true;

                int hexPosition = int.Parse(gameObject.name);
                int pCounter = 0;

                GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;
                GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = false;

                for (int i = 0; i < myBoard.boardNodes.Length; i++)
                {
                    if (myBoard.boardNodes[i].getSettlementHex().GetComponent<MeshRenderer>().enabled == true
                        && (myBoard.boardNodes[i].lHexLocation == hexPosition
                        || myBoard.boardNodes[i].rHexLocation == hexPosition
                        || myBoard.boardNodes[i].oHexLocation == hexPosition))
                    {
                        myBoard.boardNodes[i].hasRobber = true;

                        if (myBoard.boardNodes[i].houseColour != playerState.getCurrentPlayer(playerState.currentPlayerNumber).playerNumber)
                        {
                            playersAroundRobber[pCounter] = myBoard.boardNodes[i];
                            pCounter++;
                        }
                    }
                    else
                    {
                        myBoard.boardNodes[i].hasRobber = false;
                    }
                }

                // 1 player surrounding robber
                if (playersAroundRobber[1] == null && playersAroundRobber[0] != null)
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
                // 2 players surrounding robber
                else if (playersAroundRobber[2] == null && playersAroundRobber[0] != null && playersAroundRobber[1] != null)
                {
                    Debug.Log("Select a player to steal a random resource");

                    stealResourcePanel.SetActive(true);

                    int playerPos1 = playersAroundRobber[0].houseColour;
                    int playerPos2 = playersAroundRobber[1].houseColour;

                    if (playerPos1 != 1 && playerPos2 != 1)
                    {
                        stealPlayer1Button.SetActive(false);
                    }
                    if (playerPos1 != 2 && playerPos2 != 2)
                    {
                        stealPlayer2Button.SetActive(false);
                    }
                    if (playerPos1 != 3 && playerPos2 != 3)
                    {
                        stealPlayer3Button.SetActive(false);
                    }
                    if (playerPos1 != 4 && playerPos2 != 4)
                    {
                        stealPlayer4Button.SetActive(false);
                    }
                }
                // 3 players surrounding robber
                else if (playersAroundRobber[2] != null)
                {
                    stealResourcePanel.SetActive(true);

                    int playerPos1 = playersAroundRobber[0].houseColour;
                    int playerPos2 = playersAroundRobber[1].houseColour;
                    int playerPos3 = playersAroundRobber[2].houseColour;

                    if (playerPos1 != 1 || playerPos2 != 1 || playerPos3 != 1)
                    {
                        stealPlayer1Button.SetActive(false);
                    }
                    if (playerPos1 != 2 || playerPos2 != 2 || playerPos3 != 2)
                    {
                        stealPlayer2Button.SetActive(false);
                    }
                    if (playerPos1 != 3 || playerPos2 != 3 || playerPos3 != 3)
                    {
                        stealPlayer3Button.SetActive(false);
                    }
                    if (playerPos1 != 4 || playerPos2 != 4 || playerPos3 != 4)
                    {
                        stealPlayer4Button.SetActive(false);
                    }
                }
            }
        }
    }

   
    public void stealResource()
    {
        player = GameObject.Find("End Turn Button");
        playerState = player.GetComponent<PlayerStateManager>();

        randResource = Random.Range(1, 5);

        string clickedPlayerNumString = EventSystem.current.currentSelectedGameObject.name;

        string clickedPlayerNumSubString = clickedPlayerNumString.Substring(6, 1);

        int clickedPlayerNumInt;
        int.TryParse(clickedPlayerNumSubString, out clickedPlayerNumInt);

        if (playerState.getCurrentPlayer(clickedPlayerNumInt).currencyLumber == 0
            && randResource == 1)
        {
            while (randResource != 1)
            {
                randResource = Random.Range(1, 5);
            }
        }
        else if (playerState.getCurrentPlayer(clickedPlayerNumInt).currencyGrain == 0
            && randResource == 2)
        {
            while (randResource != 2)
            {
                randResource = Random.Range(1, 5);
            }
        }
        else if (playerState.getCurrentPlayer(clickedPlayerNumInt).currencyBrick == 0
            && randResource == 3)
        {
            while (randResource != 3)
            {
                randResource = Random.Range(1, 5);
            }
        }
        else if (playerState.getCurrentPlayer(clickedPlayerNumInt).currencyOre == 0
            && randResource == 4)
        {
            while (randResource != 4)
            {
                randResource = Random.Range(1, 5);
            }
        }
        else if (playerState.getCurrentPlayer(clickedPlayerNumInt).currencyWool == 0
            && randResource == 5)
        {
            while (randResource != 5)
            {
                randResource = Random.Range(1, 5);
            }
        }

        if (randResource == 1)
        {
            playerState.getCurrentPlayer(clickedPlayerNumInt).currencyLumber--;
            playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyLumber++;
        }
        if (randResource == 2)
        {
            playerState.getCurrentPlayer(clickedPlayerNumInt).currencyGrain--;
            playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyGrain++;
        }
        if (randResource == 3)
        {
            playerState.getCurrentPlayer(clickedPlayerNumInt).currencyBrick--;
            playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyBrick++;
        }
        if (randResource == 4)
        {
            playerState.getCurrentPlayer(clickedPlayerNumInt).currencyOre--;
            playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyOre++;
        }
        if (randResource == 5)
        {
            playerState.getCurrentPlayer(clickedPlayerNumInt).currencyWool--;
            playerState.getCurrentPlayer(playerState.currentPlayerNumber).currencyWool++;
        }

        stealPlayer1Button.SetActive(true);
        stealPlayer2Button.SetActive(true);
        stealPlayer3Button.SetActive(true);
        stealPlayer4Button.SetActive(true);


        stealResourcePanel.SetActive(false);
    }

    // This method is used when someone wants to play their robber development card, it will make sure they have one avalible and allow them to move the
    // robber if they do. It will then remove it from the players "inventory" and increase their used property
    public void moveRobber()
    {
        diceRoller.diceTotal = 7;
        myBoard.robberMoved = false;

        playerState.getCurrentPlayer(playerState.currentPlayerNumber).usedKnights++;
        playerState.getCurrentPlayer(playerState.currentPlayerNumber).avalibleKnights--;

        if (playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory[0] == "" || playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory[0] == null)
        {
            Debug.Log("No knights in your inventory");
        }
        else
        {
            for (int i = 0; i < playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory.Length; i++)
            {
                if (playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory[i] == "" || playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory[i] == null)
                {
                    playerState.getCurrentPlayer(playerState.currentPlayerNumber).inventory[i - 1] = "";
                }
            }
           
        }
    }
}
