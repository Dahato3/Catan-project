using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNumScript : MonoBehaviour
{
    public int NumHumanPlayers = 1;

    public void HandleInputData(int val) //handles TMPdropdown menu values
    {
        if (val == 0)
        {
            NumHumanPlayers = 1;
            Debug.Log("Human = "+NumHumanPlayers);
        }

        if (val == 1)
        {
            NumHumanPlayers = 2;
            Debug.Log("Human = "+NumHumanPlayers);
        }

        if (val == 2)
        {
            NumHumanPlayers = 3;
            Debug.Log("Human = "+NumHumanPlayers);
        }

        if (val == 3)
        {
            NumHumanPlayers = 4;
            Debug.Log("Human = "+NumHumanPlayers);
        }
    }
}
