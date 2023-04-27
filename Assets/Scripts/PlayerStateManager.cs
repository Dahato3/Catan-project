using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

public class PlayerStateManager : MonoBehaviour
{

    // Variables to access other relevent classes
    TimerScript timeScript;
    Board board;
    Player player;
    DiceRoller diceRoller;
    Trade trade;

    //PlayerNumScript playerNum;
    //CpuNumScript CpuNum;
    //StartUpMenu startMenu;

    //"SerializeField" means the variable is still private but is viewable in the unity editor
    // i had some problems with this however and did a slightly worse way but worked
    [SerializeField] GameObject timer;
    [SerializeField] GameObject myBoard;
    [SerializeField] GameObject dice;
    //[SerializeField] GameObject numPlayer;
    //[SerializeField] GameObject numCpu;
    //[SerializeField] GameObject menuStart;

    [SerializeField] GameObject tradePanel;
    [SerializeField] GameObject halfResourcePanel;
    [SerializeField] GameObject receivedTradePanel;
    [SerializeField] GameObject stealResourcePanel;
    [SerializeField] GameObject fromBankPanel;
    [SerializeField] GameObject stealAllOfOneResourcePanel;


    //A variable to store each player object in the game
    public Player player1; // White
    public Player player2; // Red
    public Player player3; // Yellow
    public Player player4; // Blue

    // A varaible of the current players number
    public int currentPlayerNumber;

    //public Player[] players;
    //public AI[] AIs;

    Vector3 player1OriginalLoc;
    Vector3 player2OriginalLoc;
    Vector3 player3OriginalLoc;
    Vector3 player4OriginalLoc;

    Vector3 player1UpdatedLoc;
    Vector3 player2UpdatedLoc;
    Vector3 player3UpdatedLoc;
    Vector3 player4UpdatedLoc;

    


    // When assigning componants we tend to try and do so in an awake function (Called before start)
    // Still leave variable initilization in start() though
    void Awake()
    {
        timeScript = timer.GetComponent<TimerScript>();
        board = myBoard.GetComponent<Board>();
        diceRoller = dice.GetComponent<DiceRoller>();

        //playerNum = numPlayer.GetComponent<PlayerNumScript>();
        //CpuNum = numCpu.GetComponent<CpuNumScript>();
        //startMenu = menuStart.GetComponent<StartUpMenu>();
    }

    // Start is called before the first frame update
    // Players instantiated here
    // current player number is intialized to 1 as that will be the first player
    void Start()
    {
        //players = new Player[4];
        //AIs = new AI[3];


        //if (playerNum.NumHumanPlayers == 4)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //    Player player3 = new Player();
        //    player3.playerNumber = 3;
        //    player3.playerColour = 3;

        //    Player player4 = new Player();
        //    player4.playerNumber = 4;
        //    player4.playerColour = 4;
        //}
        //else if (playerNum.NumHumanPlayers == 3)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //    Player player3 = new Player();
        //    player3.playerNumber = 3;
        //    player3.playerColour = 3;

        //}
        //else if (playerNum.NumHumanPlayers == 2)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //}
        //else if (playerNum.NumHumanPlayers == 1)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;
        //}


        //if (playerNum.NumHumanPlayers == 4)
        //{
        //    AI player1 = new AI();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //    Player player3 = new Player();
        //    player3.playerNumber = 3;
        //    player3.playerColour = 3;

        //    Player player4 = new Player();
        //    player4.playerNumber = 4;
        //    player4.playerColour = 4;
        //}
        //else if (playerNum.NumHumanPlayers == 3)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //    Player player3 = new Player();
        //    player3.playerNumber = 3;
        //    player3.playerColour = 3;

        //}
        //else if (playerNum.NumHumanPlayers == 2)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;

        //    Player player2 = new Player();
        //    player2.playerNumber = 2;
        //    player2.playerColour = 2;

        //}
        //else if (playerNum.NumHumanPlayers == 1)
        //{
        //    Player player1 = new Player();
        //    player1.playerNumber = 1;
        //    player1.playerColour = 1;
        //}






        //for (int i = 0; i < startMenu.TotalPlayerNum; i++)
        //{

        //    Player player1 = new Player();



        //    players[i] = new Player();
        //    players[i].playerNumber = i;
        //    players[i].playerNumber++;
        //    players[i].playerColour = i;
        //    players[i].playerColour++;
        //}
        //for (int i = 0; i < CpuNum.NumCpuPlayers; i++)
        //{
        //    AIs[i] = new AI();
        //    AIs[i].playerNumber = startMenu.TotalPlayerNum;
        //    AIs[i].playerNumber += i;
        //    AIs[i].playerNumber++;
        //    AIs[i].playerColour = startMenu.TotalPlayerNum;
        //    AIs[i].playerNumber += i;
        //    AIs[i].playerColour++;
        //}





        player1 = new Player();
        player1.playerNumber = 1;
        player1.playerColour = 1;
        player1.currencyLumber = 0;
        player1.currencyGrain = 0;
        player1.currencyBrick = 0;
        player1.currencyOre = 0;
        player1.currencyWool = 0;


        player2 = new Player();
        player2.playerNumber = 2;
        player2.playerColour = 2;
        player2.currencyLumber = 0;
        player2.currencyGrain = 0;
        player2.currencyBrick = 0;
        player2.currencyOre = 0;
        player2.currencyWool = 0;

        player3 = new Player();
        player3.playerNumber = 3;
        player3.playerColour = 3;
        player3.currencyLumber = 0;
        player3.currencyGrain = 0;
        player3.currencyBrick = 0;
        player3.currencyOre = 0;
        player3.currencyWool = 0;

        player4 = new Player();
        player4.playerNumber = 4;
        player4.playerColour = 4;
        player4.currencyLumber = 0;
        player4.currencyGrain = 0;
        player4.currencyBrick = 0;
        player4.currencyOre = 0;
        player4.currencyWool = 0;

        currentPlayerNumber = 1;

        player1OriginalLoc = GameObject.Find("Player1 stats").transform.position;
        player2OriginalLoc = GameObject.Find("Player2 stats").transform.position;
        player3OriginalLoc = GameObject.Find("Player3 stats").transform.position;
        player4OriginalLoc = GameObject.Find("Player4 stats").transform.position;

        player1UpdatedLoc = GameObject.Find("Player1 stats").transform.position;
        player2UpdatedLoc = GameObject.Find("Player2 stats").transform.position;
        player3UpdatedLoc = GameObject.Find("Player3 stats").transform.position;
        player4UpdatedLoc = GameObject.Find("Player4 stats").transform.position;

        player1UpdatedLoc[1] = player1UpdatedLoc[1] - 425;
        player2UpdatedLoc[1] = player2UpdatedLoc[1] - 425;
        player3UpdatedLoc[1] = player3UpdatedLoc[1] - 425;
        player4UpdatedLoc[1] = player4UpdatedLoc[1] - 425;
    }

    // Update is called once per frame
    void Update()
    {
        updateResources();

        int numCPU = CpuNumScript.instance.NumCpuPlayers;


        if (numCPU == 1)
        {
            if (currentPlayerNumber == 4)
            {
                Debug.Log("CALLED");
                player4.takeTurn();
            }
        }
        else if (numCPU == 2)
        {
            if (currentPlayerNumber == 4)
            {
                Debug.Log("CALLED1");
                player4.takeTurn();
            }
            if (currentPlayerNumber == 3)
            {
                Debug.Log("CALLED2");
                player4.takeTurn();
            }
        }
        else if (numCPU == 3)
        {
            if (currentPlayerNumber == 4)
            {
                Debug.Log("CALLED11");
                player4.takeTurn();
            }
            else if (currentPlayerNumber == 4)
            {
                Debug.Log("CALLED22");
                player4.takeTurn();
            }
            else if (numCPU == 4)
            {
                Debug.Log("CALLED33");
                player4.takeTurn();
            }
        }


        player1.totalResources = player1.currencyLumber + player1.currencyGrain + player1.currencyBrick + player1.currencyOre + player1.currencyWool;
        player2.totalResources = player2.currencyLumber + player2.currencyGrain + player2.currencyBrick + player2.currencyOre + player2.currencyWool;
        player3.totalResources = player3.currencyLumber + player3.currencyGrain + player3.currencyBrick + player3.currencyOre + player3.currencyWool;
        player4.totalResources = player4.currencyLumber + player4.currencyGrain + player4.currencyBrick + player4.currencyOre + player4.currencyWool;

        if (currentPlayerNumber == 1)
        {
            GameObject.Find("avalibleKnights").GetComponent<Text>().text = "avalible: " + player1.avalibleKnights;
            GameObject.Find("usedKnights").GetComponent<Text>().text = "used: " + player1.usedKnights;
        }
        else if (currentPlayerNumber == 2)
        {
            GameObject.Find("avalibleKnights").GetComponent<Text>().text = "avalible: " + player2.avalibleKnights;
            GameObject.Find("usedKnights").GetComponent<Text>().text = "used: " + player2.usedKnights;
        }
        else if (currentPlayerNumber == 3)
        {
            GameObject.Find("avalibleKnights").GetComponent<Text>().text = "avalible: " + player3.avalibleKnights;
            GameObject.Find("usedKnights").GetComponent<Text>().text = "used: " + player3.usedKnights;
        }
        else if (currentPlayerNumber == 4)
        {
            GameObject.Find("avalibleKnights").GetComponent<Text>().text = "avalible: " + player4.avalibleKnights;
            GameObject.Find("usedKnights").GetComponent<Text>().text = "used: " + player4.usedKnights;
        }

        // Updates "other player" VP and total resources
        GameObject.Find("Player1 stats").GetComponent<Text>().text = "Player 1\nVP: " + player1.victoryPoints + "\nLongest road: " + player1.longetRoad + "\nKnight: " + player1.usedKnights + "\nTotal resources: " + player1.totalResources;
        GameObject.Find("Player2 stats").GetComponent<Text>().text = "Player 2\nVP: " + player2.victoryPoints + "\nLongest road: " + player2.longetRoad + "\nKnight: " + player2.usedKnights + "\nTotal resources: " + player2.totalResources; 
        GameObject.Find("Player3 stats").GetComponent<Text>().text = "Player 3\nVP: " + player3.victoryPoints + "\nLongest road: " + player3.longetRoad + "\nKnight: " + player3.usedKnights + "\nTotal resources: " + player3.totalResources;
        GameObject.Find("Player4 stats").GetComponent<Text>().text = "Player 4\nVP: " + player4.victoryPoints + "\nLongest road: " + player4.longetRoad + "\nKnight: " + player4.usedKnights + "\nTotal resources: " + player4.totalResources;

        //Debug.Log(getCurrentPlayer(currentPlayerNumber).avalibleKnights);
        GameObject.Find("avalibleKnights").GetComponent<Text>().text = "" + getCurrentPlayer(currentPlayerNumber).avalibleKnights;
        GameObject.Find("usedKnights").GetComponent<Text>().text = "" + getCurrentPlayer(currentPlayerNumber).usedKnights;

        // Shows other players stats and keeps them updated
        if (board.introTurn == false)
        {
            if (halfResourcePanel.activeInHierarchy == true || tradePanel.activeInHierarchy == true
                || halfResourcePanel.activeInHierarchy == true || receivedTradePanel.activeInHierarchy == true
                || fromBankPanel.activeInHierarchy == true || stealAllOfOneResourcePanel.activeInHierarchy == true)
            {
                GameObject.Find("Player1 stats").transform.position = player1UpdatedLoc;
                GameObject.Find("Player2 stats").transform.position = player2UpdatedLoc;
                GameObject.Find("Player3 stats").transform.position = player3UpdatedLoc;
                GameObject.Find("Player4 stats").transform.position = player4UpdatedLoc;
            }
            else if (currentPlayerNumber == 1)
            {
                GameObject.Find("Player1 stats").GetComponent<CanvasRenderer>().SetAlpha(0);

                GameObject.Find("Player2 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player2 stats").transform.position = new Vector3(880, 320, 0);

                GameObject.Find("Player3 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player3 stats").transform.position = new Vector3(880, 235, 0);

                GameObject.Find("Player4 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player4 stats").transform.position = new Vector3(880, 150, 0);
            }
            else if (currentPlayerNumber == 2)
            {
                GameObject.Find("Player1 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player1 stats").transform.position = new Vector3(880, 320, 0);

                GameObject.Find("Player2 stats").GetComponent<CanvasRenderer>().SetAlpha(0);

                GameObject.Find("Player3 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player3 stats").transform.position = new Vector3(880, 235, 0);

                GameObject.Find("Player4 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player4 stats").transform.position = new Vector3(880, 150, 0);
            }
            else if (currentPlayerNumber == 3)
            {
                GameObject.Find("Player1 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player1 stats").transform.position = new Vector3(880, 320, 0);

                GameObject.Find("Player2 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player2 stats").transform.position = new Vector3(880, 235, 0);

                GameObject.Find("Player3 stats").GetComponent<CanvasRenderer>().SetAlpha(0);

                GameObject.Find("Player4 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player4 stats").transform.position = new Vector3(880, 150, 0);
            }
            else if (currentPlayerNumber == 4)
            {
                GameObject.Find("Player1 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player1 stats").transform.position = new Vector3(880, 320, 0);

                GameObject.Find("Player2 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player2 stats").transform.position = new Vector3(880, 235, 0);

                GameObject.Find("Player3 stats").GetComponent<CanvasRenderer>().SetAlpha(1);
                GameObject.Find("Player3 stats").transform.position = new Vector3(880, 150, 0);

                GameObject.Find("Player4 stats").GetComponent<CanvasRenderer>().SetAlpha(0);
            }
        }

        if (getCurrentPlayer(1).usedKnights > getCurrentPlayer(2).usedKnights
            && getCurrentPlayer(1).usedKnights > getCurrentPlayer(3).usedKnights
            && getCurrentPlayer(1).usedKnights > getCurrentPlayer(4).usedKnights)
        {
            getCurrentPlayer(1).HASLargestArmy = true;
            //getCurrentPlayer(1).HADLargestArmy = true;
            getCurrentPlayer(1).victoryPoints += 2;

            if (getCurrentPlayer(2).HASLargestArmy == true)
            {
                getCurrentPlayer(2).HASLargestArmy = false;
                getCurrentPlayer(2).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(3).HASLargestArmy == true)
            {
                getCurrentPlayer(3).HASLargestArmy = false;
                getCurrentPlayer(3).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(4).HASLargestArmy == true)
            {
                getCurrentPlayer(4).HASLargestArmy = false;
                getCurrentPlayer(4).victoryPoints -= 2;
            }
            
        }
        else if (getCurrentPlayer(2).usedKnights > getCurrentPlayer(1).usedKnights
            && getCurrentPlayer(2).usedKnights > getCurrentPlayer(3).usedKnights
            && getCurrentPlayer(2).usedKnights > getCurrentPlayer(4).usedKnights)
        {
            getCurrentPlayer(2).HASLargestArmy = true;
            //getCurrentPlayer(2).HADLargestArmy = true;
            getCurrentPlayer(2).victoryPoints += 2;

            if (getCurrentPlayer(1).HASLargestArmy == true)
            {
                getCurrentPlayer(1).HASLargestArmy = false;
                getCurrentPlayer(1).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(3).HASLargestArmy == true)
            {
                getCurrentPlayer(3).HASLargestArmy = false;
                getCurrentPlayer(3).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(4).HASLargestArmy == true)
            {
                getCurrentPlayer(4).HASLargestArmy = false;
                getCurrentPlayer(4).victoryPoints -= 2;
            }
        }
        else if (getCurrentPlayer(3).usedKnights > getCurrentPlayer(1).usedKnights
            && getCurrentPlayer(3).usedKnights > getCurrentPlayer(2).usedKnights
            && getCurrentPlayer(3).usedKnights > getCurrentPlayer(4).usedKnights)
        {
            getCurrentPlayer(3).HASLargestArmy = true;
            //getCurrentPlayer(3).HADLargestArmy = true;
            getCurrentPlayer(3).victoryPoints += 2;

            if (getCurrentPlayer(1).HASLargestArmy == true)
            {
                getCurrentPlayer(1).HASLargestArmy = false;
                getCurrentPlayer(1).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(2).HASLargestArmy == true)
            {
                getCurrentPlayer(2).HASLargestArmy = false;
                getCurrentPlayer(2).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(4).HASLargestArmy == true)
            {
                getCurrentPlayer(4).HASLargestArmy = false;
                getCurrentPlayer(4).victoryPoints -= 2;
            }
        }
        else if (getCurrentPlayer(4).usedKnights > getCurrentPlayer(1).usedKnights
            && getCurrentPlayer(4).usedKnights > getCurrentPlayer(2).usedKnights
            && getCurrentPlayer(4).usedKnights > getCurrentPlayer(3).usedKnights)
        {
            getCurrentPlayer(4).HASLargestArmy = true;
            //getCurrentPlayer(4).HADLargestArmy = true;
            getCurrentPlayer(4).victoryPoints += 2;

            if (getCurrentPlayer(1).HASLargestArmy == true)
            {
                getCurrentPlayer(1).HASLargestArmy = false;
                getCurrentPlayer(1).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(2).HASLargestArmy == true)
            {
                getCurrentPlayer(2).HASLargestArmy = false;
                getCurrentPlayer(2).victoryPoints -= 2;
            }
            else if (getCurrentPlayer(3).HASLargestArmy == true)
            {
                getCurrentPlayer(3).HASLargestArmy = false;
                getCurrentPlayer(3).victoryPoints -= 2;
            }
        }

        //Debug.Log("LLL:" + player1.longetRoad);

    }

    // This method is called when the 'end turn' button on screen is clicked. It will update
    // the current player variable to the next players number
    public void SwitchState()
    {
        if (board.robberMoved == true)
        {
            board.robberMoved = false;
        }

        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;
        GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = true;


        if (board.introTurn == false)
        {
            // On screen time set to 300 seconds on every new players turn
            timeScript.timeLeft = 300;
        }

        if (currentPlayerNumber == 1)
        {
            currentPlayerNumber = 2;
        }
        else if (currentPlayerNumber == 2)
        {
            currentPlayerNumber = 3;
        }
        else if (currentPlayerNumber == 3)
        {
            currentPlayerNumber = 4;
        }
        else if (currentPlayerNumber == 4)
        {
            currentPlayerNumber = 1;
        }
    }

    // This method is called every frame to keep track of when a players resource value has changed and can
    // therefore be also changed on screen
    public void updateResources()
    {

        checkNegativeResources();

        if (currentPlayerNumber == 1)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player1.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player1.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player1.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player1.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player1.currencyWool;

            GameObject.Find("VP").GetComponent<Text>().text = "Victory points: " +  player1.victoryPoints;
        }
        if (currentPlayerNumber == 2)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player2.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player2.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player2.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player2.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player2.currencyWool;

            GameObject.Find("VP").GetComponent<Text>().text = "Victory points: " + player2.victoryPoints;
        }
        if (currentPlayerNumber == 3)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player3.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player3.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player3.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player3.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player3.currencyWool;

            GameObject.Find("VP").GetComponent<Text>().text = "Victory points: " + player3.victoryPoints;
        }
        if (currentPlayerNumber == 4)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player4.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player4.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player4.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player4.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player4.currencyWool;

            GameObject.Find("VP").GetComponent<Text>().text = "Victory points: " + player4.victoryPoints;
        }
    }

    // This method does the same as the one above but updates the panel which shows other players resources
    public void updateOtherPlayersResources()
    {
        if (tradePanel.activeInHierarchy == true)
        {
            // If it's currently player 1's turn
            if (currentPlayerNumber == 1)
            {
                // Player 2's resources
                GameObject.Find("LumberAmount(A)").GetComponent<Text>().text = "" + player2.currencyLumber;
                GameObject.Find("GrainAmount(A)").GetComponent<Text>().text = "" + player2.currencyGrain;
                GameObject.Find("BrickAmount(A)").GetComponent<Text>().text = "" + player2.currencyBrick;
                GameObject.Find("OreAmount(A)").GetComponent<Text>().text = "" + player2.currencyOre;
                GameObject.Find("WoolAmount(A)").GetComponent<Text>().text = "" + player2.currencyWool;


                // Player 3's resources
                GameObject.Find("LumberAmount(B)").GetComponent<Text>().text = "" + player3.currencyLumber;
                GameObject.Find("GrainAmount(B)").GetComponent<Text>().text = "" + player3.currencyGrain;
                GameObject.Find("BrickAmount(B)").GetComponent<Text>().text = "" + player3.currencyBrick;
                GameObject.Find("OreAmount(B)").GetComponent<Text>().text = "" + player3.currencyOre;
                GameObject.Find("WoolAmount(B)").GetComponent<Text>().text = "" + player3.currencyWool;

                // Player 4's resources
                GameObject.Find("LumberAmount(C)").GetComponent<Text>().text = "" + player4.currencyLumber;
                GameObject.Find("GrainAmount(C)").GetComponent<Text>().text = "" + player4.currencyGrain;
                GameObject.Find("BrickAmount(C)").GetComponent<Text>().text = "" + player4.currencyBrick;
                GameObject.Find("OreAmount(C)").GetComponent<Text>().text = "" + player4.currencyOre;
                GameObject.Find("WoolAmount(C)").GetComponent<Text>().text = "" + player4.currencyWool;

                GameObject.Find("OtherPlayer(A) header").GetComponent<Text>().text = "Player 2";
                GameObject.Find("OtherPlayer(B) header").GetComponent<Text>().text = "Player 3";
                GameObject.Find("OtherPlayer(C) header").GetComponent<Text>().text = "Player 4";
            }
            else if (currentPlayerNumber == 2)
            {
                // Player 2's resources
                GameObject.Find("LumberAmount(A)").GetComponent<Text>().text = "" + player1.currencyLumber;
                GameObject.Find("GrainAmount(A)").GetComponent<Text>().text = "" + player1.currencyGrain;
                GameObject.Find("BrickAmount(A)").GetComponent<Text>().text = "" + player1.currencyBrick;
                GameObject.Find("OreAmount(A)").GetComponent<Text>().text = "" + player1.currencyOre;
                GameObject.Find("WoolAmount(A)").GetComponent<Text>().text = "" + player1.currencyWool;

                // Player 3's resources
                GameObject.Find("LumberAmount(B)").GetComponent<Text>().text = "" + player3.currencyLumber;
                GameObject.Find("GrainAmount(B)").GetComponent<Text>().text = "" + player3.currencyGrain;
                GameObject.Find("BrickAmount(B)").GetComponent<Text>().text = "" + player3.currencyBrick;
                GameObject.Find("OreAmount(B)").GetComponent<Text>().text = "" + player3.currencyOre;
                GameObject.Find("WoolAmount(B)").GetComponent<Text>().text = "" + player3.currencyWool;

                // Player 4's resources
                GameObject.Find("LumberAmount(C)").GetComponent<Text>().text = "" + player4.currencyLumber;
                GameObject.Find("GrainAmount(C)").GetComponent<Text>().text = "" + player4.currencyGrain;
                GameObject.Find("BrickAmount(C)").GetComponent<Text>().text = "" + player4.currencyBrick;
                GameObject.Find("OreAmount(C)").GetComponent<Text>().text = "" + player4.currencyOre;
                GameObject.Find("WoolAmount(C)").GetComponent<Text>().text = "" + player4.currencyWool;

                GameObject.Find("OtherPlayer(A) header").GetComponent<Text>().text = "Player 1";
                GameObject.Find("OtherPlayer(B) header").GetComponent<Text>().text = "Player 3";
                GameObject.Find("OtherPlayer(C) header").GetComponent<Text>().text = "Player 4";
            }
            else if (currentPlayerNumber == 3)
            {
                // Player 2's resources
                GameObject.Find("LumberAmount(A)").GetComponent<Text>().text = "" + player1.currencyLumber;
                GameObject.Find("GrainAmount(A)").GetComponent<Text>().text = "" + player1.currencyGrain;
                GameObject.Find("BrickAmount(A)").GetComponent<Text>().text = "" + player1.currencyBrick;
                GameObject.Find("OreAmount(A)").GetComponent<Text>().text = "" + player1.currencyOre;
                GameObject.Find("WoolAmount(A)").GetComponent<Text>().text = "" + player1.currencyWool;

                // Player 3's resources
                GameObject.Find("LumberAmount(B)").GetComponent<Text>().text = "" + player2.currencyLumber;
                GameObject.Find("GrainAmount(B)").GetComponent<Text>().text = "" + player2.currencyGrain;
                GameObject.Find("BrickAmount(B)").GetComponent<Text>().text = "" + player2.currencyBrick;
                GameObject.Find("OreAmount(B)").GetComponent<Text>().text = "" + player2.currencyOre;
                GameObject.Find("WoolAmount(B)").GetComponent<Text>().text = "" + player2.currencyWool;

                // Player 4's resources
                GameObject.Find("LumberAmount(C)").GetComponent<Text>().text = "" + player4.currencyLumber;
                GameObject.Find("GrainAmount(C)").GetComponent<Text>().text = "" + player4.currencyGrain;
                GameObject.Find("BrickAmount(C)").GetComponent<Text>().text = "" + player4.currencyBrick;
                GameObject.Find("OreAmount(C)").GetComponent<Text>().text = "" + player4.currencyOre;
                GameObject.Find("WoolAmount(C)").GetComponent<Text>().text = "" + player4.currencyWool;

                GameObject.Find("OtherPlayer(A) header").GetComponent<Text>().text = "Player 1";
                GameObject.Find("OtherPlayer(B) header").GetComponent<Text>().text = "Player 2";
                GameObject.Find("OtherPlayer(C) header").GetComponent<Text>().text = "Player 4";
            }
            else if (currentPlayerNumber == 4)
            {
                // Player 2's resources
                GameObject.Find("LumberAmount(A)").GetComponent<Text>().text = "" + player1.currencyLumber;
                GameObject.Find("GrainAmount(A)").GetComponent<Text>().text = "" + player1.currencyGrain;
                GameObject.Find("BrickAmount(A)").GetComponent<Text>().text = "" + player1.currencyBrick;
                GameObject.Find("OreAmount(A)").GetComponent<Text>().text = "" + player1.currencyOre;
                GameObject.Find("WoolAmount(A)").GetComponent<Text>().text = "" + player1.currencyWool;

                // Player 3's resources
                GameObject.Find("LumberAmount(B)").GetComponent<Text>().text = "" + player2.currencyLumber;
                GameObject.Find("GrainAmount(B)").GetComponent<Text>().text = "" + player2.currencyGrain;
                GameObject.Find("BrickAmount(B)").GetComponent<Text>().text = "" + player2.currencyBrick;
                GameObject.Find("OreAmount(B)").GetComponent<Text>().text = "" + player2.currencyOre;
                GameObject.Find("WoolAmount(B)").GetComponent<Text>().text = "" + player2.currencyWool;

                // Player 4's resources
                GameObject.Find("LumberAmount(C)").GetComponent<Text>().text = "" + player3.currencyLumber;
                GameObject.Find("GrainAmount(C)").GetComponent<Text>().text = "" + player3.currencyGrain;
                GameObject.Find("BrickAmount(C)").GetComponent<Text>().text = "" + player3.currencyBrick;
                GameObject.Find("OreAmount(C)").GetComponent<Text>().text = "" + player3.currencyOre;
                GameObject.Find("WoolAmount(C)").GetComponent<Text>().text = "" + player3.currencyWool;

                GameObject.Find("OtherPlayer(A) header").GetComponent<Text>().text = "Player 1";
                GameObject.Find("OtherPlayer(B) header").GetComponent<Text>().text = "Player 2";
                GameObject.Find("OtherPlayer(C) header").GetComponent<Text>().text = "Player 3";
            }
        }

    }

    // This method is used to not allow players resoruces to go below 0
    // Primarily used in the stealing resource functionality of the robber
    public void checkNegativeResources()
    {
        if (player1.currencyLumber < 0)
        {
            Debug.Log("Player 1 lumber went below 0");
            player1.currencyLumber = 0;
        }
        else if (player1.currencyGrain < 0)
        {
            Debug.Log("Player 1 grain went below 0");
            player1.currencyGrain = 0;
        }
        else if (player1.currencyBrick < 0)
        {
            Debug.Log("Player 1 brick went below 0");
            player1.currencyBrick = 0;
        }
        else if (player1.currencyOre < 0)
        {
            Debug.Log("Player 1 ore went below 0");
            player1.currencyOre = 0;
        }
        else if (player1.currencyWool < 0)
        {
            Debug.Log("Player 1 wool went below 0");
            player1.currencyWool = 0;
        }

        else if (player2.currencyLumber < 0)
        {
            Debug.Log("Player 2 lumber went below 0");
            player2.currencyLumber = 0;
        }
        else if (player2.currencyGrain < 0)
        {
            Debug.Log("Player 2 grain went below 0");
            player2.currencyGrain = 0;
        }
        else if (player2.currencyBrick < 0)
        {
            Debug.Log("Player 2 brick went below 0");
            player2.currencyBrick = 0;
        }
        else if (player2.currencyOre < 0)
        {
            Debug.Log("Player 2 ore went below 0");
            player2.currencyOre = 0;
        }
        else if (player2.currencyWool < 0)
        {
            Debug.Log("Player 2 wool went below 0");
            player2.currencyWool = 0;
        }

        else if (player3.currencyLumber < 0)
        {
            Debug.Log("Player 3 lumber went below 0");
            player3.currencyLumber = 0;
        }
        else if (player3.currencyGrain < 0)
        {
            Debug.Log("Player 3 grain went below 0");
            player3.currencyGrain = 0;
        }
        else if (player3.currencyBrick < 0)
        {
            Debug.Log("Player 3 brick went below 0");
            player3.currencyBrick = 0;
        }
        else if (player3.currencyOre < 0)
        {
            Debug.Log("Player 3 ore went below 0");
            player3.currencyOre = 0;
        }
        else if (player3.currencyWool < 0)
        {
            Debug.Log("Player 3 wool went below 0");
            player3.currencyWool = 0;
        }

        else if (player4.currencyLumber < 0)
        {
            Debug.Log("Player 4 lumber went below 0");
            player4.currencyLumber = 0;
        }
        else if (player4.currencyGrain < 0)
        {
            Debug.Log("Player 4 grain went below 0");
            player4.currencyGrain = 0;
        }
        else if (player4.currencyBrick < 0)
        {
            Debug.Log("Player 4 brick went below 0");
            player4.currencyBrick = 0;
        }
        else if (player4.currencyOre < 0)
        {
            Debug.Log("Player 4 ore went below 0");
            player4.currencyOre = 0;
        }
        else if (player4.currencyWool < 0)
        {
            Debug.Log("Player 4 wool went below 0");
            player4.currencyWool = 0;
        }
    }

    // This method will simple take in a string corrospondin to a resource and a player number so we know which player to incrment
    // It's used during the first round of the intro turn to allocates a players first set of resources
    public void setResource(string resource, int playerNum)
    {
        Player player = getCurrentPlayer(playerNum);

        if (player == player1)
        {
            if (resource == "lumber")
            {
                player1.currencyLumber++;
            }
            else if (resource == "grain")
            {
                player1.currencyGrain++;
            }
            else if (resource == "brick")
            {
                player1.currencyBrick++;
            }
            else if (resource == "ore")
            {
                player1.currencyOre++;
            }
            else if (resource == "wool")
            {
                player1.currencyWool++;
            }
        }
        else if (player == player2)
        {
            if (resource == "lumber")
            {
                player2.currencyLumber++;
            }
            else if (resource == "grain")
            {
                player2.currencyGrain++;
            }
            else if (resource == "brick")
            {
                player2.currencyBrick++;
            }
            else if (resource == "ore")
            {
                player2.currencyOre++;
            }
            else if (resource == "wool")
            {
                player2.currencyWool++;
            }
        }
        else if (player == player3)
        {
            if (resource == "lumber")
            {
                player3.currencyLumber++;
            }
            else if (resource == "grain")
            {
                player3.currencyGrain++;
            }
            else if (resource == "brick")
            {
                player3.currencyBrick++;
            }
            else if (resource == "ore")
            {
                player3.currencyOre++;
            }
            else if (resource == "wool")
            {
                player3.currencyWool++;
            }
        }
        else if (player == player4)
        {
            if (resource == "lumber")
            {
                player4.currencyLumber++;
            }
            else if (resource == "grain")
            {
                player4.currencyGrain++;
            }
            else if (resource == "brick")
            {
                player4.currencyBrick++;
            }
            else if (resource == "ore")
            {
                player4.currencyOre++;
            }
            else if (resource == "wool")
            {
                player4.currencyWool++;
            }
        }
    }

    // A method that takes an int and will return the corrosponding player object
    public Player getCurrentPlayer(int current)
    {
        if (current == 1)
        {
            return player1;
        }
        else if (current == 2)
        {
            return player2;
        }
        else if (current == 3)
        {
            return player3;
        }
        else if (current == 4)
        {
            return player4;
        }
        else
        {
            return null;
        }
    }

    public void callBuyDevelopmentCard()
    {
        if (currentPlayerNumber == 1)
        {
            player1.buyDevelopmentCard();
        }
        else if (currentPlayerNumber == 2)
        {
            player2.buyDevelopmentCard();
        }
        else if (currentPlayerNumber == 3)
        {
            player3.buyDevelopmentCard();
        }
        else if (currentPlayerNumber == 4)
        {
            player4.buyDevelopmentCard();
        }

    }
}


