using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStateManager : MonoBehaviour
{

    ShowPanel showPanel;
    TimerScript timeScript;
    Board board;

    //"SerializeField" means the variable is still private but is viewable in the unity editor
    // i had some problems with this however and did a slightly worse way but worked
    [SerializeField] GameObject timer;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject myBoard;



    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;


    public int currentPlayerNumber;


    // When assigning componants we tend to try and do so in an awake function (Called before start)
    // Still leave variable initilization in start() though
    void Awake()
    {
        timeScript = timer.GetComponent<TimerScript>();
        showPanel = panel.GetComponent<ShowPanel>();
        board = myBoard.GetComponent<Board>();

    }

    // Start is called before the first frame update
    void Start()
    {
        player1 = new Player();
        player1.setPlayerNumber(1);
        player1.buildSettlement(board.boardNodes[0]);
        //player1.currencyLumber = 1;
        //player1.currencyGrain = 1;
        //player1.currencyBrick = 1;
        //player1.currencyOre = 1;
        //player1.currencyWool = 1;
        player2 = new Player();
        player2.setPlayerNumber(2);
        player2.buildSettlement(board.boardNodes[4]);
        //player2.currencyLumber = 2;
        //player2.currencyGrain = 2;
        //player2.currencyBrick = 2;
        //player2.currencyOre = 2;
        //player2.currencyWool = 2;
        player3 = new Player();
        player3.setPlayerNumber(3);
        player3.buildSettlement(board.boardNodes[7]);
        //player3.currencyLumber = 3;
        //player3.currencyGrain = 3;
        //player3.currencyBrick = 3;
        //player3.currencyOre = 3;
        //player3.currencyWool = 3;
        player4 = new Player();
        player4.setPlayerNumber(4);
        player4.buildSettlement(board.boardNodes[8]);
        //player4.currencyLumber = 4;
        //player4.currencyGrain = 4;
        //player4.currencyBrick = 4;
        //player4.currencyOre = 4;
        //player4.currencyWool = 4;


        currentPlayerNumber = 1;

        
    }

    // Update is called once per frame
    void Update()
    {
        updateResources();
        updateOtherPlayersResources();
    }

    public void SwitchState()
    {

        timeScript.timeLeft = 300;

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

}


