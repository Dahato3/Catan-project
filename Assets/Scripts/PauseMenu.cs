using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false; //indicates game is not paused
    public GameObject PauseMenuAnchor; 
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Paused)
            {
                Resume();
            }
            else
            {
                Stop();
            }
        }

    }

    void Stop()
    {
        PauseMenuAnchor.SetActive(true);
        Time.timeScale = 0; //pauses application
        Paused = true;
        Debug.Log("Paused");

    }

    public void Resume()
    {
        PauseMenuAnchor.SetActive(false);
        Time.timeScale = 1; //resumes application
        Paused = false;
        Debug.Log("Unpaused");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1; //resumes application so when loading main menu it shouldn't freeze
        SceneManager.LoadScene("StartUpMenu"); //Loads StartUpMenu 
        Debug.Log("Return to menu");
    }

}

