using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RollTotal : MonoBehaviour
{

    DiceRoller theDiceRoller;

    // Start is called before the first frame update
    void Start()
    {

        theDiceRoller = GameObject.FindAnyObjectByType<DiceRoller>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "= " + theDiceRoller.diceTotal;
    }
}
