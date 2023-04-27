using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUpMenu : MonoBehaviour
{
    public int TotalPlayerNum = 1;
    public Button PlayButton;
    PlayerNumScript playerNumScript; //referencing both scripts to have access to data inside it
    CpuNumScript cpuNumScript;


    void Awake()
    {
        playerNumScript = GameObject.Find("StartUpMenu").GetComponent<PlayerNumScript>(); 
        cpuNumScript = GameObject.Find("StartUpMenu").GetComponent<CpuNumScript>();

    }
    
    void Start()
    {
        Time.timeScale = 1;
        PlayButton.interactable = false;
    }

    void Update()
    {
        TotalPlayerNum = cpuNumScript.NumCpuPlayers + playerNumScript.NumHumanPlayers;
        if ((TotalPlayerNum < 2) || (TotalPlayerNum > 4))
        {
            PlayButton.interactable = false; //play button can't be clicked and won't work on click
        }
        else
        {
            PlayButton.interactable = true; //play button can be clicked and works on click
        }

    }

    
    //Load Scene
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
        GameObject.Find("StartUpMenu").SetActive(false);
        Debug.Log("Game Starts");
    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player quit from startup menu");
    }
}
