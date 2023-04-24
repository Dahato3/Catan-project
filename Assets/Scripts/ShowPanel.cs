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

}
