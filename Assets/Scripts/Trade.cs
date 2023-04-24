using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trade : MonoBehaviour
{
    PlayerStateManager playerStateManager;
    [SerializeField] GameObject player;

    ShowPanel tradePanel;
    [SerializeField] GameObject theInitialTradePanel;
    [SerializeField] GameObject receivedTradePanel;

    public static int initiatedTradePlayerNum;
    public static int calledCounterPlayerNum;

    public static int offerLumber = 0;
    public static int offerGrain = 0;
    public static int offerBrick = 0;
    public static int offerOre = 0;
    public static int offerWool = 0;

    public static int receiveLumber = 0;
    public static int receiveGrain = 0;
    public static int receiveBrick = 0;
    public static int receiveOre = 0;
    public static int receiveWool = 0;

    public static bool isCounterOffer = false;

    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
        tradePanel = theInitialTradePanel.GetComponent<ShowPanel>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void trade()
    {

        if (isCounterOffer == true)
        {
            while (playerStateManager.currentPlayerNumber != initiatedTradePlayerNum)
            {
                playerStateManager.SwitchState();
            }



            theInitialTradePanel.SetActive(false);
            receivedTradePanel.gameObject.SetActive(true);

            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;

            GameObject.Find("LumberAmountFrom").GetComponent<Text>().text = "" + offerLumber;
            GameObject.Find("GrainAmountFrom").GetComponent<Text>().text = "" + offerGrain;
            GameObject.Find("BrickAmountFrom").GetComponent<Text>().text = "" + offerBrick;
            GameObject.Find("OreAmountFrom").GetComponent<Text>().text = "" + offerOre;
            GameObject.Find("WoolAmountFrom").GetComponent<Text>().text = "" + offerWool;

            GameObject.Find("LumberAmountReturn").GetComponent<Text>().text = "" + receiveLumber;
            GameObject.Find("GrainAmountReturn").GetComponent<Text>().text = "" + receiveGrain;
            GameObject.Find("BrickAmountReturn").GetComponent<Text>().text = "" + receiveBrick;
            GameObject.Find("OreAmountReturn").GetComponent<Text>().text = "" + receiveOre;
            GameObject.Find("WoolAmountReturn").GetComponent<Text>().text = "" + receiveWool;

        }
        else
        {

            theInitialTradePanel.SetActive(false);
            receivedTradePanel.gameObject.SetActive(true);

            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;

            initiatedTradePlayerNum = playerStateManager.currentPlayerNumber;

            playerStateManager.SwitchState();

            GameObject.Find("LumberAmountFrom").GetComponent<Text>().text = "" + offerLumber;
            GameObject.Find("GrainAmountFrom").GetComponent<Text>().text = "" + offerGrain;
            GameObject.Find("BrickAmountFrom").GetComponent<Text>().text = "" + offerBrick;
            GameObject.Find("OreAmountFrom").GetComponent<Text>().text = "" + offerOre;
            GameObject.Find("WoolAmountFrom").GetComponent<Text>().text = "" + offerWool;

            GameObject.Find("LumberAmountReturn").GetComponent<Text>().text = "" + receiveLumber;
            GameObject.Find("GrainAmountReturn").GetComponent<Text>().text = "" + receiveGrain;
            GameObject.Find("BrickAmountReturn").GetComponent<Text>().text = "" + receiveBrick;
            GameObject.Find("OreAmountReturn").GetComponent<Text>().text = "" + receiveOre;
            GameObject.Find("WoolAmountReturn").GetComponent<Text>().text = "" + receiveWool;
        }

    }

    public void confirmTrade()
    {
        if (isCounterOffer == true)
        {
            isCounterOffer = false;

            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber += offerLumber;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain += offerGrain;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick += offerBrick;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre += offerOre;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool += offerWool;

            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber -= receiveLumber;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain -= receiveGrain;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick -= receiveBrick;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre -= receiveOre;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool -= receiveWool;

            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyLumber -= offerLumber;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyGrain -= offerGrain;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyBrick -= offerBrick;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyOre -= offerOre;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyWool -= offerWool;

            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyLumber += receiveLumber;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyGrain += receiveGrain;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyBrick += receiveBrick;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyOre += receiveOre;
            playerStateManager.getCurrentPlayer(calledCounterPlayerNum).currencyWool += receiveWool;

        }
        else
        {
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber += offerLumber;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain += offerGrain;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick += offerBrick;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre += offerOre;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool += offerWool;

            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber -= receiveLumber;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain -= receiveGrain;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick -= receiveBrick;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre -= receiveOre;
            playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool -= receiveWool;

            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyLumber -= offerLumber;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyGrain -= offerGrain;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyBrick -= offerBrick;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyOre -= offerOre;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyWool -= offerWool;

            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyLumber += receiveLumber;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyGrain += receiveGrain;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyBrick += receiveBrick;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyOre += receiveOre;
            playerStateManager.getCurrentPlayer(initiatedTradePlayerNum).currencyWool += receiveWool;
        }

        offerLumber = 0;
        offerGrain = 0;
        offerBrick = 0;
        offerOre = 0;
        offerWool = 0;

        receiveLumber = 0;
        receiveGrain = 0;
        receiveBrick = 0;
        receiveOre = 0;
        receiveWool = 0;



        receivedTradePanel.SetActive(false);

        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;

        while (playerStateManager.currentPlayerNumber != initiatedTradePlayerNum)
        {
            playerStateManager.SwitchState();
        }
    }

    public void declineTrade()
    {
        if (isCounterOffer == true)
        {
            receivedTradePanel.SetActive(false);
            GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;
            isCounterOffer = false;
        }
        else
        {

            playerStateManager.SwitchState();
            if (playerStateManager.currentPlayerNumber == initiatedTradePlayerNum)
            {
                offerLumber = 0;
                offerGrain = 0;
                offerBrick = 0;
                offerOre = 0;
                offerWool = 0;

                receiveLumber = 0;
                receiveGrain = 0;
                receiveBrick = 0;
                receiveOre = 0;
                receiveWool = 0;

                receivedTradePanel.SetActive(false);
                GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;
            }
        }
    }

    public void counterTrade()
    {
        isCounterOffer = true;

        calledCounterPlayerNum = playerStateManager.currentPlayerNumber;

        offerLumber = 0;
        offerGrain = 0;
        offerBrick = 0;
        offerOre = 0;
        offerWool = 0;

        receiveLumber = 0;
        receiveGrain = 0;
        receiveBrick = 0;
        receiveOre = 0;
        receiveWool = 0;

        GameObject.Find("LumberAmountFrom").GetComponent<Text>().text = "" + offerLumber;
        GameObject.Find("GrainAmountFrom").GetComponent<Text>().text = "" + offerGrain;
        GameObject.Find("BrickAmountFrom").GetComponent<Text>().text = "" + offerBrick;
        GameObject.Find("OreAmountFrom").GetComponent<Text>().text = "" + offerOre;
        GameObject.Find("WoolAmountFrom").GetComponent<Text>().text = "" + offerWool;

        GameObject.Find("LumberAmountReturn").GetComponent<Text>().text = "" + receiveLumber;
        GameObject.Find("GrainAmountReturn").GetComponent<Text>().text = "" + receiveGrain;
        GameObject.Find("BrickAmountReturn").GetComponent<Text>().text = "" + receiveBrick;
        GameObject.Find("OreAmountReturn").GetComponent<Text>().text = "" + receiveOre;
        GameObject.Find("WoolAmountReturn").GetComponent<Text>().text = "" + receiveWool;

        receivedTradePanel.SetActive(false);
        theInitialTradePanel.SetActive(true);

        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;

        GameObject.Find("LumberAmount(O)").GetComponent<Text>().text = "" + offerLumber;
        GameObject.Find("GrainAmount(O)").GetComponent<Text>().text = "" + offerGrain;
        GameObject.Find("BrickAmount(O)").GetComponent<Text>().text = "" + offerBrick;
        GameObject.Find("OreAmount(O)").GetComponent<Text>().text = "" + offerOre;
        GameObject.Find("WoolAmount(O)").GetComponent<Text>().text = "" + offerWool;

        GameObject.Find("LumberAmount(R)").GetComponent<Text>().text = "" + receiveLumber;
        GameObject.Find("GrainAmount(R)").GetComponent<Text>().text = "" + receiveGrain;
        GameObject.Find("BrickAmount(R)").GetComponent<Text>().text = "" + receiveBrick;
        GameObject.Find("OreAmount(R)").GetComponent<Text>().text = "" + receiveOre;
        GameObject.Find("WoolAmount(R)").GetComponent<Text>().text = "" + receiveWool;


    }

    public void openTradePanel()
    {

        Debug.Log(EventSystem.current.currentSelectedGameObject);


        offerLumber = 0;
        offerGrain = 0;
        offerBrick = 0;
        offerOre = 0;
        offerWool = 0;

        receiveLumber = 0;
        receiveGrain = 0;
        receiveBrick = 0;
        receiveOre = 0;
        receiveWool = 0;

        if (theInitialTradePanel != null)
        {
            if (theInitialTradePanel.activeInHierarchy == false)
            {
                theInitialTradePanel.SetActive(true);
                GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;

                GameObject.Find("LumberAmount(O)").GetComponent<Text>().text = "" + offerLumber;
                GameObject.Find("GrainAmount(O)").GetComponent<Text>().text = "" + offerGrain;
                GameObject.Find("BrickAmount(O)").GetComponent<Text>().text = "" + offerBrick;
                GameObject.Find("OreAmount(O)").GetComponent<Text>().text = "" + offerOre;
                GameObject.Find("WoolAmount(O)").GetComponent<Text>().text = "" + offerWool;

                GameObject.Find("LumberAmount(R)").GetComponent<Text>().text = "" + receiveLumber;
                GameObject.Find("GrainAmount(R)").GetComponent<Text>().text = "" + receiveGrain;
                GameObject.Find("BrickAmount(R)").GetComponent<Text>().text = "" + receiveBrick;
                GameObject.Find("OreAmount(R)").GetComponent<Text>().text = "" + receiveOre;
                GameObject.Find("WoolAmount(R)").GetComponent<Text>().text = "" + receiveWool;
            }
            else
            {
                theInitialTradePanel.SetActive(false);
                GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;
            }
        }
    }


    public void incrementTrade()
    {
        Debug.Log(gameObject.name);

        if (gameObject.name == "LumberOffer")
        {
            offerLumber++;
            if (offerLumber > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber)
            {
                offerLumber = 0;
            }
            GameObject.Find("LumberAmount(O)").GetComponent<Text>().text = "" + offerLumber;
        }
        else if (gameObject.name == "GrainOffer")
        {
            offerGrain++;
            if (offerGrain > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain)
            {
                offerGrain = 0;
            }
            GameObject.Find("GrainAmount(O)").GetComponent<Text>().text = "" + offerGrain;
        }
        else if (gameObject.name == "BrickOffer")
        {
            offerBrick++;
            if (offerBrick > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick)
            {
                offerBrick = 0;
            }
            GameObject.Find("BrickAmount(O)").GetComponent<Text>().text = "" + offerBrick;
        }
        else if (gameObject.name == "OreOffer")
        {
            offerOre++;
            if (offerOre > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre)
            {
                offerOre = 0;
            }
            GameObject.Find("OreAmount(O)").GetComponent<Text>().text = "" + offerOre;
        }
        else if (gameObject.name == "WoolOffer")
        {
            offerWool++;
            if (offerWool > playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool)
            {
                offerWool = 0;
            }
            GameObject.Find("WoolAmount(O)").GetComponent<Text>().text = "" + offerWool;
        }

        else if (gameObject.name == "LumberReceive")
        {
            receiveLumber++;
            if (receiveLumber > 19)
            {
                receiveLumber = 0;
            }
            GameObject.Find("LumberAmount(R)").GetComponent<Text>().text = "" + receiveLumber;
        }
        else if (gameObject.name == "GrainReceive")
        {
            receiveGrain++;
            if (receiveGrain > 19)
            {
                receiveGrain = 0;
            }
            GameObject.Find("GrainAmount(R)").GetComponent<Text>().text = "" + receiveGrain;
        }
        else if (gameObject.name == "BrickReceive")
        {
            receiveBrick++;
            if (receiveBrick > 19)
            {
                receiveBrick = 0;
            }
            GameObject.Find("BrickAmount(R)").GetComponent<Text>().text = "" + receiveBrick;
        }
        else if (gameObject.name == "OreReceive")
        {
            receiveOre++;
            if (receiveOre > 19)
            {
                receiveOre = 0;
            }
            GameObject.Find("OreAmount(R)").GetComponent<Text>().text = "" + receiveOre;
        }
        else if (gameObject.name == "WoolReceive")
        {
            receiveWool++;
            if (receiveWool > 19)
            {
                receiveWool = 0;
            }
            GameObject.Find("WoolAmount(R)").GetComponent<Text>().text = "" + receiveWool;
        }
    }
}
