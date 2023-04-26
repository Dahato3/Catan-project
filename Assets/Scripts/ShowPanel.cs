using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowPanel : MonoBehaviour
{
    // Gameobject that links to panel the script is attached to
    public GameObject panel;

    // Start is the first method called to initialize various variables
    // Simple sets the linked panel to "not active" at the start of the game
    private void Start()
    {
        panel.SetActive(false);
    }

}
