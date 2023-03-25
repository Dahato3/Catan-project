using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    PlayerStateManager playerStateManager;
    [SerializeField] GameObject player;

    public float timeLeft;
    public bool timerOn = false;
    public bool test = false;

    public Text timerText;

    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
    }

    void Start()
    {
        timeLeft = 300;
        timerOn = true; 
    }

    // Update is called once per frame
    void Update()
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

    public void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("Time: " + "{0:00} : {1:00}", minutes, seconds);
    }
}
