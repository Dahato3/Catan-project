using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankTrade : MonoBehaviour
{

    GameObject board;
    public Board myboard;
    PlayerStateManager playerStateManager;
    [SerializeField] GameObject player;
    [SerializeField] GameObject tradePanel;

    GameObject increaseLumber;
    GameObject decreaseLumber;
    GameObject increaseWool;
    GameObject decreaseWool;
    GameObject increaseBrick;
    GameObject decreaseBrick;
    GameObject increaseGrain;
    GameObject decreaseGrain;
    GameObject increaseOre;
    GameObject decreaseOre;

    GameObject selectLumber;
    GameObject selectBrick;
    GameObject selectWool;
    GameObject selectGrain;
    GameObject selectOre;

    [SerializeField] GameObject lumberNumber;
    [SerializeField] GameObject brickNumber;
    [SerializeField] GameObject oreNumber;
    [SerializeField] GameObject grainNumber;
    [SerializeField] GameObject woolNumber;


    int tradeLumber;
    int tradeWool;
    int tradeGrain;
    int tradeBrick;
    int tradeOre;


    public bool freeBuild = false;
    public bool is3to1 = false;
    public bool isWool = false;
    public bool isBrick = false;
    public bool isLumber = false;
    public bool isOre = false;
    public bool isGrain = false;

    private void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
        board = GameObject.Find("Board");
        myboard = board.GetComponent<Board>();

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lumberNumber.GetComponent<Text>().text = "" + tradeLumber;

        grainNumber.GetComponent<Text>().text = "" + tradeGrain;

        oreNumber.GetComponent<Text>().text = "" + tradeOre;

        woolNumber.GetComponent<Text>().text = "" + tradeWool;

        brickNumber.GetComponent<Text>().text = "" + tradeBrick;

    }

    public void SetPanel()
    {
        Debug.Log("Clicked");
        tradePanel.SetActive(true);

    }


    public void Trade()
    {
        HasTradePost();
        if (is3to1)
        {
            while (tradeBrick >= 3 || tradeGrain >= 3 || tradeLumber >= 3 || tradeOre >= 3 || tradeWool >= 3)
            {

            }
        }
        else if (isWool)
        {
            while (tradeBrick >= 4 || tradeGrain >= 4 || tradeLumber >= 4 || tradeOre >= 4 || tradeWool >= 2)
            {

            }
        }
        else if (isOre)
        {
            while (tradeBrick >= 4 || tradeGrain >= 4 || tradeLumber >= 4 || tradeOre >= 2 || tradeWool >= 4)
            {

            }
        }
        else if (isBrick)
        {
            while (tradeBrick >= 2 || tradeGrain >= 4 || tradeLumber >= 4 || tradeOre >= 4 || tradeWool >= 4)
            {

            }
        }
        else if (isGrain)
        {
            while (tradeBrick >= 4 || tradeGrain >= 2 || tradeLumber >= 4 || tradeOre >= 4 || tradeWool >= 4)
            {

            }
        }
        else if (isLumber)
        {
            while (tradeBrick >= 4 || tradeGrain >= 4 || tradeLumber >= 2 || tradeOre >= 4 || tradeWool >= 4)
            {

            }
        }
        else
        {
            while (tradeBrick >= 4 || tradeGrain >= 4 || tradeLumber >= 4 || tradeOre >= 4 || tradeWool >= 4)
            {

            }
        }
    }

    public void HasTradePost()
    {
        if (myboard.boardNodes[0].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[1].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[3].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[5].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[26].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[32].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[47].getHouseColour() == playerStateManager.currentPlayerNumber ||
            myboard.boardNodes[51].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            is3to1 = true;
        }
        else if (myboard.boardNodes[10].getHouseColour() == playerStateManager.currentPlayerNumber
            || myboard.boardNodes[15].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            isWool = true;
        }
        else if (myboard.boardNodes[11].getHouseColour() == playerStateManager.currentPlayerNumber
            || myboard.boardNodes[16].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            isBrick = true;
        }
        else if (myboard.boardNodes[33].getHouseColour() == playerStateManager.currentPlayerNumber
            || myboard.boardNodes[38].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            isLumber = true;
        }
        else if (myboard.boardNodes[42].getHouseColour() == playerStateManager.currentPlayerNumber
            || myboard.boardNodes[46].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            isOre = true;
        }
        else if (myboard.boardNodes[49].getHouseColour() == playerStateManager.currentPlayerNumber
            || myboard.boardNodes[52].getHouseColour() == playerStateManager.currentPlayerNumber)
        {
            isGrain = true;
        }


    }
    public void Offer()
    {
        if (GameObject.Find("IncreaseBrick"))
        {
            if (tradeBrick > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick)
                tradeBrick++;
        }
        else if (GameObject.Find("DecreaseBrick"))
        {
            if (tradeBrick > 0)
                tradeBrick--;
        }
        else if (GameObject.Find("IncreaseWool"))
        {
            if (tradeWool > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool)
                tradeWool++;
        }
        else if (GameObject.Find("DecreaseWool"))
        {
            if (tradeWool > 0)
                tradeWool--;
        }
        else if (GameObject.Find("IncreaseGrain"))
        {
            if (tradeGrain > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain)
                tradeGrain++;
        }
        else if (GameObject.Find("DecreaseGrain"))
        {
            if (tradeGrain > 0)
                tradeGrain--;
        }
        else if (GameObject.Find("IncreaseOre"))
        {
            if (tradeOre > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre)
                tradeOre++;
        }
        else if (GameObject.Find("DecreaseOre"))
        {
            if (tradeOre > 0)
                tradeOre--;
        }
        else if (GameObject.Find("IncreaseLumber"))
        {
            if (tradeLumber > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber)
                tradeLumber++;
        }
        else if (GameObject.Find("IncreaseLumber"))
        {
            if (tradeLumber > 0)
                tradeLumber--;
        }

    }
}