using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class CurrentPlayer : MonoBehaviour
{
    // Variables to help us access properties from other classes
    PlayerStateManager playerStateManager;
    public GameObject player;

    // Awake function called before start to initialse the GameObject we use to access other classes
    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    // Simple keeps the on screen current player componant updated to the current player
    void Update()
    {
        GetComponent<Text>().text = "Current Player = " + playerStateManager.currentPlayerNumber;
    }
}
