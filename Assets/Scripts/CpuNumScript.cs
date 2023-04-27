using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CpuNumScript : MonoBehaviour
{
    public int NumCpuPlayers = 0;

    public static CpuNumScript instance;

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void HandleInputData(int val) //handles TMPdropdown menu values
    {
        if (val == 0)
        {
            NumCpuPlayers = 0;
            Debug.Log("CPU = "+NumCpuPlayers);
        }

        if (val == 1)
        {
            NumCpuPlayers = 1;
            Debug.Log("CPU = "+NumCpuPlayers);
        }

        if (val == 2)
        {
            NumCpuPlayers = 2;
            Debug.Log("CPU = "+NumCpuPlayers);
        }

        if (val == 3)
        {
            NumCpuPlayers = 3;
            Debug.Log("CPU = "+NumCpuPlayers);
        }
    }
}
