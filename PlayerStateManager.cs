using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{

    TimerScript timeScript;

    //"SerializeField" means the variable is still private but is viewable in the unity editor
    // i had some problems with this however and did a slightly worse way but worked
    [SerializeField] GameObject timer;


    public int currentPlayerNumber;


    // When assigning componants we tend to try and do so in an awake function (Called before start)
    // Still leave variable initilization in start() though
    void Awake()
    {
        timeScript = timer.GetComponent<TimerScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player1 player1 = new Player1();
        player1.setPlayerNumber(1);
        Player1 player2 = new Player1();
        player1.setPlayerNumber(2);
        Player1 player3 = new Player1();
        player1.setPlayerNumber(3);
        Player1 player4 = new Player1();
        player1.setPlayerNumber(4);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchState()
    {

        //timeScript.timeLeft = 5;

        //if (currentState == player1)
        //{
        //    currentState = player2;
        //    currentPlayerNumber = 2;

        //    //player2.EnterState(this);
        //}
        //else if (currentState == player2)
        //{
        //    currentState = player3;
        //    currentPlayerNumber = 3;

        //    //player3.EnterState(this);
        //}
        //else if (currentState == player3)
        //{
        //    currentState = player4;
        //    currentPlayerNumber = 4;

        //    //player4.EnterState(this);
        //}
        //else if (currentState == player4)
        //{
        //    currentState = player1;
        //    currentPlayerNumber = 1;

        //    //player1.EnterState(this);
        //}
    }


}
