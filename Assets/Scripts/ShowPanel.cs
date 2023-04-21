using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    //Trade trade;
    //[SerializeField] GameObject gameTrade;

    public GameObject panel;

    //void Awake()
    //{
    //    gameTrade = GameObject.Find("LumberOffer");
    //    trade = gameTrade.GetComponent<Trade>();
    //}

    private void Start()
    {
        panel.SetActive(false);
    }

    //public void openPanel()
    //{
        
    //    if (panel != null)
    //    {
    //        if(panel.activeInHierarchy == false)
    //        {
    //            panel.SetActive(true);
    //        }
    //        else
    //        {
    //            panel.SetActive(false);
    //        }
    //    }
    //}
}
