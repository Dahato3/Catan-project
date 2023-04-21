using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class build : MonoBehaviour
{
    [SerializeField] GameObject playerState;
    PlayerStateManager state;


    void Awake()
    {
        playerState = GameObject.Find("End Turn Button");
        state = playerState.GetComponent<PlayerStateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state.currentPlayerNumber == 1)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            }
            else if (state.currentPlayerNumber == 2)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else if (state.currentPlayerNumber == 3)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            }
            else if (state.currentPlayerNumber == 4)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            }


            if (gameObject.tag == "settlementCity")
            {
                state.getCurrentPlayer(state.currentPlayerNumber).buildSettlementCity(gameObject);
            }
            else if (gameObject.tag == "road")
            {
                state.getCurrentPlayer(state.currentPlayerNumber).buildRoad(gameObject);
            }
            else
            {
                return;
            }
           
        }
    }
}
