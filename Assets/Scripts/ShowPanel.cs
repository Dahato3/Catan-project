using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void openPanel()
    {
        if (panel != null)
        {
            if(panel.activeInHierarchy == false)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
}