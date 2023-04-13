using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    // Variables to access other classes
    PlayerStateManager playerStateManager;
    Board myBoard;
    [SerializeField] GameObject player;
    [SerializeField] GameObject board;

    // Variables to implement the timer functionality
    public float timeLeft;
    public bool timerOn = false;
    public bool test = false;

    public Text timerText;

    // The awake() function allows the above variable to have access to the corrosponding
    // classes script
    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
        myBoard = board.GetComponent<Board>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame

    // Decrements the timer timer internally
    void Update()
    {
        if (myBoard.introTurn == true)
        {
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

        timerText.text = string.Format("Time: " + "{0:00} : {1:00}", minutes, seconds);
    }
}
