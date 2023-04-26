using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    // Variables to access other classes
    [SerializeField] GameObject player;
    [SerializeField] GameObject board;
    PlayerStateManager playerStateManager;
    Board myBoard;
    
    // Variables to implement the timer functionality
    public float timeLeft;
    public bool timerOn = false;
    public bool test = false;

    // The awake() function allows the above variable to have access to the corrosponding
    // classes script
    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
        myBoard = board.GetComponent<Board>();
    }

    // Start is the first method called to initialize various variables
    void Start()
    {
        
    }

    // Update is called once per frame
    // Decrements the timer timer internally
    void Update()
    {
        if (myBoard.introTurn == false)
        {
            timerOn = true;
            if (timerOn)
            {
                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    updateTimer(timeLeft);
                }
                else
                {
                    timerOn = false;
                    playerStateManager.SwitchState();
                    timerOn = true;
                }
            }
        }
    }

    // Updates the timer display
    public void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        GameObject.Find("Timer").GetComponent<Text>().text = string.Format("Time: " + "{0:00} : {1:00}", minutes, seconds);
    }
}
