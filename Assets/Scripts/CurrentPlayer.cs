using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayer : MonoBehaviour
{
    
    PlayerStateManager playerStateManager;
    public GameObject player;

    void Awake()
    {
        playerStateManager = player.GetComponent<PlayerStateManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Current Player = " + playerStateManager.currentPlayerNumber;
    }
}
