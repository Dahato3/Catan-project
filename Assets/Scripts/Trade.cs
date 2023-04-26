using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Trade : MonoBehaviour
{
    // Some gameobject variables to help us access panels and properties from other classes
    [SerializeField] GameObject theInitialTradePanel;
    [SerializeField] GameObject receivedTradePanel;
    [SerializeField] GameObject fromBankPanel;
    [SerializeField] GameObject player;

    PlayerStateManager playerStateManager;
    ShowPanel tradePanel;

    // Various variables used in the trade functionality
    public static int initiatedTradePlayerNum;
    public static int calledCounterPlayerNum;

    public int totalSeclectedFromBank;
    public int totalToSteal;

    // Amount of resources being offered from a player either initializing the trade or counter offering
    public static int offerLumber = 0;
    public static int offerGrain = 0;
    public static int offerBrick = 0;
    public static int offerOre = 0;
    public static int offerWool = 0;

    // The amount of resources that the player would like to receive from their trade / counter offer
    public static int receiveLumber = 0;
    public static int receiveGrain = 0;
    public static int receiveBrick = 0;
    public static int receiveOre = 0;
    public static int receiveWool = 0;

    public static int fromBankLumber = 0;
    public static int fromBankGrain = 0;
    public static int fromBankBrick = 0;
    public static int fromBankOre = 0;
    public static int fromBankWool = 0;

    public static bool isCounterOffer = false;

    // Awake function called before start to initialse the GameObject we use to access other classes
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
        totalSeclectedFromBank = fromBankLumber + fromBankGrain + fromBankBrick + fromBankOre + fromBankWool;
    }

    // This is the method called when we want to send the trade out to players. It will send the trade to all players in order, they get a chance to accept the trade,
    // decline the trade or send back a counter trade to the player who initially sent the trade
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

    // This method is called when a player received a trade a confirms the trade. In this case the corrosponding resoruces are either incremented
    // or decremented depending on the trade itself
    public void confirmTrade()
    {
        if (totalSeclectedFromBank != 2)
        {
            Debug.Log("Please select 2 resources");
        }
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

    // Simialr to the method above this is an option when a player receives a trade but simple wants to decline the trade. It will very simple switch the states
    // so the next player has the same options but will check if the player it swapped to is the original layer who intialized and in this case removed the panel
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

    // This method is called when a player receieved a trade and decided to send a counter offer back to the original player. It will reset the variables
    // and swap the panel to the sending trad panel and allowds this player to set up a new trade. Once sent, it will only be sent to the player who started thje
    // original trade
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

    // This method simple opens the trade panel if it's not currenrtly open and will close it if it is. It's linked to a "player trade" UI button
    public void openTradePanel()
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

    // This method increments offered resources and to be receieved resources depending on which button on the trade panel is clicked
    public void incrementTrade()
    {
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

        else if (gameObject.name == "FBLumberSelected")
        {
            fromBankLumber++;
            if (totalSeclectedFromBank > 1)
            {
                fromBankLumber = 0;
            }
            GameObject.Find("FBLumberAmount").GetComponent<Text>().text = "" + fromBankLumber;
        }
        else if (gameObject.name == "FBGrainSelected")
        {
            fromBankGrain++;
            if (totalSeclectedFromBank > 1)
            {
                fromBankGrain = 0;
            }
            GameObject.Find("FBGrainAmount").GetComponent<Text>().text = "" + fromBankGrain;
        }
        else if (gameObject.name == "FBBrickSelected")
        {
            fromBankBrick++;
            if (totalSeclectedFromBank > 1)
            {
                fromBankBrick = 0;
            }
            GameObject.Find("FBBrickAmount").GetComponent<Text>().text = "" + fromBankBrick;
        }
        else if (gameObject.name == "FBOreSelected")
        {
            fromBankOre++;
            if (totalSeclectedFromBank > 1)
            {
                fromBankOre = 0;
            }
            GameObject.Find("FBOreAmount").GetComponent<Text>().text = "" + fromBankOre;
        }
        else if (gameObject.name == "FBWoolSelected")
        {
            fromBankWool++;
            if (totalSeclectedFromBank > 1)
            {
                fromBankWool = 0;
            }
            GameObject.Find("FBWoolAmount").GetComponent<Text>().text = "" + fromBankWool;
        }
    }

    // This is a simple method called by the confirm button on the panel that appears when someone obtains the yearofplenty development card
    // It will increase the corrosponding players chosen resources, reset thhe variables for next time and hide the panel.
    public void confirmSelectedFromBank()
    {
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber += fromBankLumber;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain += fromBankGrain;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick += fromBankBrick;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre += fromBankOre;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool += fromBankWool;

        fromBankLumber = 0;
        fromBankGrain = 0;
        fromBankBrick = 0;
        fromBankOre = 0;
        fromBankWool = 0;

        fromBankPanel.SetActive(false);
    }

    // Similar to the method above this is called by the confirm button on the panel that appears when someone obtains the monoply development card
    // It will check to see the current player so it knows who to deduct and increase from and then check the resource.
    public void stealAllOfOneResource()
    {
        totalToSteal = 0;
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == 1)
        {
            if (gameObject.name == "FBLumberSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(2).currencyLumber + playerStateManager.getCurrentPlayer(3).currencyLumber
                    + playerStateManager.getCurrentPlayer(4).currencyLumber;

                playerStateManager.getCurrentPlayer(1).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(2).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(3).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(4).currencyLumber = 0;

            }
            else if (gameObject.name == "FBGrainSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(2).currencyGrain + playerStateManager.getCurrentPlayer(3).currencyGrain
                    + playerStateManager.getCurrentPlayer(4).currencyGrain;

                playerStateManager.getCurrentPlayer(1).currencyGrain += totalToSteal;

                playerStateManager.getCurrentPlayer(2).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(3).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(4).currencyGrain = 0;
            }
            else if (gameObject.name == "FBBrickSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(2).currencyBrick + playerStateManager.getCurrentPlayer(3).currencyBrick
                    + playerStateManager.getCurrentPlayer(4).currencyBrick;

                playerStateManager.getCurrentPlayer(1).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(2).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(3).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(4).currencyBrick = 0;
            }
            else if (gameObject.name == "FBOreSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(2).currencyOre + playerStateManager.getCurrentPlayer(3).currencyOre
                    + playerStateManager.getCurrentPlayer(4).currencyOre;

                playerStateManager.getCurrentPlayer(1).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(2).currencyOre = 0;
                playerStateManager.getCurrentPlayer(3).currencyOre = 0;
                playerStateManager.getCurrentPlayer(4).currencyOre = 0;
            }
            else if (gameObject.name == "FBWoolSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(2).currencyWool + playerStateManager.getCurrentPlayer(3).currencyWool
                    + playerStateManager.getCurrentPlayer(4).currencyWool;

                playerStateManager.getCurrentPlayer(1).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(2).currencyWool = 0;
                playerStateManager.getCurrentPlayer(3).currencyWool = 0;
                playerStateManager.getCurrentPlayer(4).currencyWool = 0;
            }
        }
        else if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == 2)
        {
            if (gameObject.name == "FBLumberSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyLumber + playerStateManager.getCurrentPlayer(3).currencyLumber
                    + playerStateManager.getCurrentPlayer(4).currencyLumber;

                playerStateManager.getCurrentPlayer(2).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(3).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(4).currencyLumber = 0;

            }
            else if (gameObject.name == "FBGrainSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyGrain + playerStateManager.getCurrentPlayer(3).currencyGrain
                    + playerStateManager.getCurrentPlayer(4).currencyGrain;

                playerStateManager.getCurrentPlayer(2).currencyGrain += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(3).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(4).currencyGrain = 0;
            }
            else if (gameObject.name == "FBBrickSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyBrick + playerStateManager.getCurrentPlayer(3).currencyBrick
                    + playerStateManager.getCurrentPlayer(4).currencyBrick;

                playerStateManager.getCurrentPlayer(2).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(3).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(4).currencyBrick = 0;
            }
            else if (gameObject.name == "FBOreSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyOre + playerStateManager.getCurrentPlayer(3).currencyOre
                    + playerStateManager.getCurrentPlayer(4).currencyOre;

                playerStateManager.getCurrentPlayer(2).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyOre = 0;
                playerStateManager.getCurrentPlayer(3).currencyOre = 0;
                playerStateManager.getCurrentPlayer(4).currencyOre = 0;
            }
            else if (gameObject.name == "FBWoolSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyWool + playerStateManager.getCurrentPlayer(3).currencyWool
                    + playerStateManager.getCurrentPlayer(4).currencyWool;

                playerStateManager.getCurrentPlayer(2).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyWool = 0;
                playerStateManager.getCurrentPlayer(3).currencyWool = 0;
                playerStateManager.getCurrentPlayer(4).currencyWool = 0;
            }
        }
        else if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == 3)
        {
            if (gameObject.name == "FBLumberSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyLumber + playerStateManager.getCurrentPlayer(2).currencyLumber
                    + playerStateManager.getCurrentPlayer(4).currencyLumber;

                playerStateManager.getCurrentPlayer(3).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(2).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(4).currencyLumber = 0;

            }
            else if (gameObject.name == "FBGrainSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyGrain + playerStateManager.getCurrentPlayer(2).currencyGrain
                    + playerStateManager.getCurrentPlayer(4).currencyGrain;

                playerStateManager.getCurrentPlayer(3).currencyGrain += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(2).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(4).currencyGrain = 0;
            }
            else if (gameObject.name == "FBBrickSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyBrick + playerStateManager.getCurrentPlayer(2).currencyBrick
                    + playerStateManager.getCurrentPlayer(4).currencyBrick;

                playerStateManager.getCurrentPlayer(3).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(2).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(4).currencyBrick = 0;
            }
            else if (gameObject.name == "FBOreSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyOre + playerStateManager.getCurrentPlayer(2).currencyOre
                    + playerStateManager.getCurrentPlayer(4).currencyOre;

                playerStateManager.getCurrentPlayer(3).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyOre = 0;
                playerStateManager.getCurrentPlayer(2).currencyOre = 0;
                playerStateManager.getCurrentPlayer(4).currencyOre = 0;
            }
            else if (gameObject.name == "FBWoolSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyWool + playerStateManager.getCurrentPlayer(2).currencyWool
                    + playerStateManager.getCurrentPlayer(4).currencyWool;

                playerStateManager.getCurrentPlayer(3).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyWool = 0;
                playerStateManager.getCurrentPlayer(2).currencyWool = 0;
                playerStateManager.getCurrentPlayer(4).currencyWool = 0;
            }
        }
        else if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == 4)
        {
            if (gameObject.name == "FBLumberSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyLumber + playerStateManager.getCurrentPlayer(2).currencyLumber
                    + playerStateManager.getCurrentPlayer(3).currencyLumber;

                playerStateManager.getCurrentPlayer(4).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(2).currencyLumber = 0;
                playerStateManager.getCurrentPlayer(3).currencyLumber = 0;

            }
            else if (gameObject.name == "FBGrainSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyGrain + playerStateManager.getCurrentPlayer(2).currencyGrain
                    + playerStateManager.getCurrentPlayer(3).currencyGrain;

                playerStateManager.getCurrentPlayer(4).currencyGrain += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(2).currencyGrain = 0;
                playerStateManager.getCurrentPlayer(3).currencyGrain = 0;
            }
            else if (gameObject.name == "FBBrickSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyBrick + playerStateManager.getCurrentPlayer(2).currencyBrick
                    + playerStateManager.getCurrentPlayer(3).currencyBrick;

                playerStateManager.getCurrentPlayer(4).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(2).currencyBrick = 0;
                playerStateManager.getCurrentPlayer(3).currencyBrick = 0;
            }
            else if (gameObject.name == "FBOreSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyOre + playerStateManager.getCurrentPlayer(2).currencyOre
                    + playerStateManager.getCurrentPlayer(3).currencyOre;

                playerStateManager.getCurrentPlayer(4).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyOre = 0;
                playerStateManager.getCurrentPlayer(2).currencyOre = 0;
                playerStateManager.getCurrentPlayer(3).currencyOre = 0;
            }
            else if (gameObject.name == "FBWoolSelected")
            {
                totalToSteal = playerStateManager.getCurrentPlayer(1).currencyWool + playerStateManager.getCurrentPlayer(2).currencyWool
                    + playerStateManager.getCurrentPlayer(2).currencyWool;

                playerStateManager.getCurrentPlayer(4).currencyLumber += totalToSteal;

                playerStateManager.getCurrentPlayer(1).currencyWool = 0;
                playerStateManager.getCurrentPlayer(2).currencyWool = 0;
                playerStateManager.getCurrentPlayer(3).currencyWool = 0;
            }
        }

    }
}
