using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{

    Board board;

    [SerializeField] GameObject myBoard;

    PlayerStateManager playerStateManager;

    [SerializeField] GameObject player;


    void Awake()
    {
        board = myBoard.GetComponent<Board>();
        playerStateManager = player.GetComponent<PlayerStateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        // initialzing out array here
        diceValues = new int[2];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int[] diceValues;
    public int diceTotal;

    public Sprite diceImageOne;
    public Sprite diceImageTwo;
    public Sprite diceImageThree;
    public Sprite diceImageFour;
    public Sprite diceImageFive;
    public Sprite diceImageSix;

    // Method to actually roll the dice, Stores 2 random integers in the range of 1 to 6 into out diceValues array.
    public void RollDice()
    {
        diceTotal = 0;
        for (int i = 0; i < diceValues.Length; i++)
        {
            diceValues[i] = Random.Range(1, 7); // Stores a random integer between 1 and 6 in the array, done twice
            diceTotal += diceValues[i]; // Adds both integers that got stored in the array

            // We have 2 children images of the dice roller, 1 for each dice roll
            // So we are going get each 1 of the children, and update its image componant with the correct one

            if(diceValues[i] == 1)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageOne;
            }
            else if (diceValues[i] == 2)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageTwo;
            }
            else if (diceValues[i] == 3)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageThree;
            }
            else if (diceValues[i] == 4)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageFour;
            }
            else if (diceValues[i] == 5)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageFive;
            }
            else if (diceValues[i] == 6)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite =
                    diceImageSix;
            }
        }
        checkIncrease();

        // Outputs to the console out most recent roll of the 2 dice and displays the total
        //Debug.Log("Rolled: " + diceValues[0] + " , " +  diceValues[1] + " (Total: " + diceTotal + ")");
    }
    public void checkIncrease()
    {
        foreach (Node n in board.boardNodes)
        {
            if (diceTotal == n.oHex)
            {
                if (n.houseColour == 1)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player1.addResources(1);
                        //playerStateManager.player1.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player1.addResources(2);
                        //playerStateManager.player1.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player1.addResources(3);
                        //playerStateManager.player1.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player1.addResources(4);
                        //playerStateManager.player1.currencyWool = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player1.addResources(5);
                        //playerStateManager.player1.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 2)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player2.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player2.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player2.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player2.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player2.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 3)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player3.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player3.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player3.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player3.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player3.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 4)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player4.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player4.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player4.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player4.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player4.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
            }
            else if (diceTotal == n.rHex)
            {
                if (n.houseColour == 1)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player1.addResources(1);
                        //playerStateManager.player1.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player1.addResources(2);
                        //playerStateManager.player1.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player1.addResources(3);
                        //playerStateManager.player1.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player1.addResources(4);
                        //playerStateManager.player1.currencyWool = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player1.addResources(5);
                        //playerStateManager.player1.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 2)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player2.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player2.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player2.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player2.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player2.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 3)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player3.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player3.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player3.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player3.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player3.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 4)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player4.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player4.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player4.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player4.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player4.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
            }
            else if (diceTotal == n.lHex)
            {
                if (n.houseColour == 1)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player1.addResources(1);
                        //playerStateManager.player1.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player1.addResources(2);
                        //playerStateManager.player1.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player1.addResources(3);
                        //playerStateManager.player1.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player1.addResources(4);
                        //playerStateManager.player1.currencyWool = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player1.addResources(5);
                        //playerStateManager.player1.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 2)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player2.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player2.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player2.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player2.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player2.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 3)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player3.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player3.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player3.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player3.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player3.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
                else if (n.houseColour == 4)
                {
                    if (n.oHexResource == "lumber")
                    {
                        playerStateManager.player4.addResources(1);
                        //playerStateManager.player2.currencyLumber = +1;
                    }
                    else if (n.oHexResource == "grain")
                    {
                        playerStateManager.player4.addResources(2);
                        //playerStateManager.player2.currencyGrain = +1;
                    }
                    else if (n.oHexResource == "brick")
                    {
                        playerStateManager.player4.addResources(3);
                        //playerStateManager.player2.currencyBrick = +1;
                    }
                    else if (n.oHexResource == "wool")
                    {
                        playerStateManager.player4.addResources(4);
                        //playerStateManager.player2.currencyWool = +1;
                    }
                    else if (n.oHexResource == "ore")
                    {
                        playerStateManager.player4.addResources(5);
                        //playerStateManager.player2.currencyOre = +1;
                    }
                }
            }
        }
    }
}
