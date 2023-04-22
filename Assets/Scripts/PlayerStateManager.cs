using UnityEngine;
using UnityEngine.UI;


public class PlayerStateManager : MonoBehaviour
{
    // Variables to access other relevent classes
    TimerScript timeScript;
    Board board;

    Player player;

    //"SerializeField" means the variable is still private but is viewable in the unity editor
    // i had some problems with this however and did a slightly worse way but worked
    [SerializeField] GameObject timer;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject myBoard;


    // A variable to store each player object in the game
    public Player player1; // White
    public Player player2; // Red
    public Player player3; // Yellow
    public Player player4; // Blue

    // A varaible of the current players number
    public int currentPlayerNumber;


    // When assigning componants we tend to try and do so in an awake function (Called before start)
    // Still leave variable initilization in start() though
    void Awake()
    {
        timeScript = timer.GetComponent<TimerScript>();
        //showPanel = panel.GetComponent<ShowPanel>();
        board = myBoard.GetComponent<Board>();

    }

    // Start is called before the first frame update
    // Players instantiated here
    // current player number is intialized to 1 as that will be the first player
    void Start()
    {
        player1 = new Player();
        player1.setPlayerNumber(1);
        player1.playerColour = 1;
        //player1.setPlayerNumber(1);
        //player1.buildSettlement(board.boardNodes[12]);
        player1.currencyLumber = 1;
        player1.currencyGrain = 0;
        player1.currencyBrick = 0;
        player1.currencyOre = 1;
        player1.currencyWool = 1;


        player2 = new Player();
        player2.playerNumber = 2;
        player2.playerColour = 2;
        //player2.setPlayerNumber(2);
        //player2.buildSettlement(board.boardNodes[4]);
        player2.currencyLumber = 1;
        player2.currencyGrain = 1;
        player2.currencyBrick = 0;
        player2.currencyOre = 0;
        player2.currencyWool = 1;


        player3 = new Player();
        player3.playerNumber = 3;
        player3.playerColour = 3;
        //player3.setPlayerNumber(3);
        //player3.buildSettlement(board.boardNodes[7]);
        player3.currencyLumber = 1;
        player3.currencyGrain = 1;
        player3.currencyBrick = 0;
        player3.currencyOre = 0;
        player3.currencyWool = 1;


        player4 = new Player();
        player4.playerNumber = 4;
        player4.playerColour = 4;
        //player4.setPlayerNumber(4);
        //player4.buildSettlement(board.boardNodes[8]);
        player4.currencyLumber = 0;
        player4.currencyGrain = 0;
        player4.currencyBrick = 1;
        player4.currencyOre = 1;
        player4.currencyWool = 1;


        currentPlayerNumber = 1;
    }

    // Update is called once per frame
    void Update()
    {
        updateResources();

        player1.totalResources = player1.currencyLumber + player1.currencyGrain + player1.currencyBrick + player1.currencyOre + player1.currencyWool;
        player2.totalResources = player2.currencyLumber + player2.currencyGrain + player2.currencyBrick + player2.currencyOre + player2.currencyWool;
        player3.totalResources = player3.currencyLumber + player3.currencyGrain + player3.currencyBrick + player3.currencyOre + player3.currencyWool;
        player4.totalResources = player4.currencyLumber + player4.currencyGrain + player4.currencyBrick + player4.currencyOre + player4.currencyWool;
    }

    //public void callBuildSettlementCity(int i)
    //{
    //    player.buildSettlementCity(i);
    //}

    // This method is called when the 'end turn' button on screen is clicked. It will update
    // the current player variable to the next players number
    public void SwitchState()
    {
        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;
        GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = true;

        Debug.Log(GameObject.Find("End Turn Button").GetComponent<Button>().interactable);

        if (board.introTurn == false)
        {
            // On screen time set to 300 seconds on every new players turn
            timeScript.timeLeft = 300;
        }
        

        if (currentPlayerNumber == 1)
        {
            currentPlayerNumber = 2;

            //player2.EnterState(this);
        }
        else if (currentPlayerNumber == 2)
        {
            currentPlayerNumber = 3;

            //player3.EnterState(this);
        }
        else if (currentPlayerNumber == 3)
        {
            currentPlayerNumber = 4;

            //player4.EnterState(this);
        }
        else if (currentPlayerNumber == 4)
        {
            currentPlayerNumber = 1;

            //player1.EnterState(this);
        }
    }

    // This method is called every frame to keep track of when a players resource value has changed and can
    // therefore be also changed on screen
    public void updateResources()
    {
        if (currentPlayerNumber == 1)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player1.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player1.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player1.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player1.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player1.currencyWool;
        }
        if (currentPlayerNumber == 2)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player2.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player2.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player2.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player2.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player2.currencyWool;
        }
        if (currentPlayerNumber == 3)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player3.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player3.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player3.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player3.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player3.currencyWool;
        }
        if (currentPlayerNumber == 4)
        {
            GameObject.Find("MyLumberAmount").GetComponent<Text>().text = "" + player4.currencyLumber;
            GameObject.Find("MyGrainAmount").GetComponent<Text>().text = "" + player4.currencyGrain;
            GameObject.Find("MyBrickAmount").GetComponent<Text>().text = "" + player4.currencyBrick;
            GameObject.Find("MyOreAmount").GetComponent<Text>().text = "" + player4.currencyOre;
            GameObject.Find("MyWoolAmount").GetComponent<Text>().text = "" + player4.currencyWool;
        }
    }

    // This method does the same as the one above but updates the panel which shows other players resources
    public void updateOtherPlayersResources()
    {
        if(panel.activeInHierarchy == true)
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

}


