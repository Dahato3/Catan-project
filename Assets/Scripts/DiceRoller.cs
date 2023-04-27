using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class DiceRoller : MonoBehaviour
{
    // Variables to help us access properties from other classes
    [SerializeField] GameObject myBoard;
    [SerializeField] GameObject player;
    [SerializeField] GameObject halfResourcePanelG;
    Board board;
    PlayerStateManager playerStateManager;

    // These are sprite variables used to show the on screen representation of which dice values are rolled
    public Sprite diceImageOne;
    public Sprite diceImageTwo;
    public Sprite diceImageThree;
    public Sprite diceImageFour;
    public Sprite diceImageFive;
    public Sprite diceImageSix;

    // Variabes to help with the dice functionality
    public bool sevenRolled;
    public bool rolled = false;
    public static int initialPlayerToRemove;
    public static int totalSelected;
    public static int toRemove;
    public int diceTotal;

    public int[] diceValues;

    public static int lumberSelected;
    public static int grainSelected;
    public static int brickSelected;
    public static int oreSelected;
    public static int woolSelected;

    // Awake function called before start to initialse the GameObject we use to access other classes
    void Awake()
    {
        board = myBoard.GetComponent<Board>();
        playerStateManager = player.GetComponent<PlayerStateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        diceValues = new int[2];
    }

    // Update is called once per frame
    void Update()
    {
        // Keeps the totalSelected variables updated at all times
        totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;

        GameObject.Find("DiceRollTotal").GetComponent<Text>().text = "= " + diceTotal;
    }

    // Method to actually roll the dice, Stores 2 random integers in the range of 1 to 6 into out diceValues array, sets the corrosponding on screen
    // sprites, adds them together and displays the total

    // NOTE- it's in this method where we operate some of the robber functionality as we are able to easil identify when a 7 is rolled
    public void RollDice()
    {
        Debug.Log("rll dice called");
        GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = false;
        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;

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
        if (diceTotal == 7)
        {
            halfResources();
            
        }
        else
        {
            checkIncrease();
            
        }
    }

    // This is the method that's linked to the buttons for a player who rolled a 7 to select half their resources to remove.
    // It was important for the player to be able to choose this
    public void incrementSelected()
    {
        if (gameObject.name == "LumberSelected")
        {
            if (lumberSelected == playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber
                || totalSelected == toRemove)
            {
                lumberSelected = 0;
                GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
                totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
                return;
            }
            if (totalSelected < playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources
                && totalSelected < toRemove)
            {
                lumberSelected++;
            }
            totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
            GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
        }
        else if (gameObject.name == "GrainSelected")
        {
            if (grainSelected == playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain
                || totalSelected == toRemove)
            {
                grainSelected = 0;
                GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
                return;
            }
            if (totalSelected < playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources
                && totalSelected < toRemove)
            {
                grainSelected++;
            }
            totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
            GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
        }
        else if (gameObject.name == "BrickSelected")
        {
            if (brickSelected == playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick
                || totalSelected == toRemove)
            {
                brickSelected = 0;
                GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + brickSelected;
                totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
                return;
            }
            if (totalSelected < playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources
                && totalSelected < toRemove)
            {
                brickSelected++;
            }
            totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
            GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + brickSelected;
        }
        else if (gameObject.name == "OreSelected")
        {
            if (oreSelected == playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre
                || totalSelected == toRemove)
            {
                oreSelected = 0;
                GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + oreSelected;
                totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
                return;
            }
            if (totalSelected < playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources
                && totalSelected < toRemove)
            {
                oreSelected++;
            }
            totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
            GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + oreSelected;
        }
        else if (gameObject.name == "WoolSelected")
        {
            if (woolSelected == playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool
                || totalSelected == toRemove)
            {
                woolSelected = 0;
                GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + woolSelected;
                totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
                return;
            }
            if (totalSelected < playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources
                && totalSelected < toRemove)
            {
                woolSelected++;
            }
            totalSelected = lumberSelected + grainSelected + brickSelected + oreSelected + woolSelected;
            GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + woolSelected;
        }
    }

    // This method will be linked to the "confirm" button on the panel where the player selects half their resources to be removed.
    public void confirmSelected()
    {
        if (toRemove != totalSelected)
        {
            return;
        }
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyLumber -= lumberSelected;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyGrain -= grainSelected;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyBrick -= brickSelected;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyOre -= oreSelected;
        playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).currencyWool -= woolSelected;

        playerStateManager.SwitchState();

        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == initialPlayerToRemove)
        {

            lumberSelected = 0;
            grainSelected = 0;
            brickSelected = 0;
            oreSelected = 0;
            woolSelected = 0;

            GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
            GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

            halfResourcePanelG.SetActive(false);

            return;
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            Debug.Log(playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources);
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            lumberSelected = 0;
            grainSelected = 0;
            brickSelected = 0;
            oreSelected = 0;
            woolSelected = 0;

            GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
            GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

            return;
        }
        else
        {
            playerStateManager.SwitchState();
            if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == initialPlayerToRemove)
            { 

                lumberSelected = 0;
                grainSelected = 0;
                brickSelected = 0;
                oreSelected = 0;
                woolSelected = 0;

                GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
                GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

                halfResourcePanelG.SetActive(false);

                return;
            }
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            lumberSelected = 0;
            grainSelected = 0;
            brickSelected = 0;
            oreSelected = 0;
            woolSelected = 0;

            GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
            GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

            return;
        }
        else
        {
            playerStateManager.SwitchState();
            if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == initialPlayerToRemove)
            {
               

                lumberSelected = 0;
                grainSelected = 0;
                brickSelected = 0;
                oreSelected = 0;
                woolSelected = 0;

                GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
                GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

                halfResourcePanelG.SetActive(false);

                return;
            }
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            Debug.Log(playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources);
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            lumberSelected = 0;
            grainSelected = 0;
            brickSelected = 0;
            oreSelected = 0;
            woolSelected = 0;

            GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
            GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
            GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

            return;
        }
        else
        {
            playerStateManager.SwitchState();
            if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).playerNumber == initialPlayerToRemove)
            {

                lumberSelected = 0;
                grainSelected = 0;
                brickSelected = 0;
                oreSelected = 0;
                woolSelected = 0;

                GameObject.Find("LumberAmountSelected").GetComponent<Text>().text = "" + lumberSelected;
                GameObject.Find("GrainAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("BrickAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("OreAmountSelected").GetComponent<Text>().text = "" + grainSelected;
                GameObject.Find("WoolAmountSelected").GetComponent<Text>().text = "" + grainSelected;

                halfResourcePanelG.SetActive(false);

                return;
            }
        }
    }

    // This method is called right at the end of the roll dice method, it is not called when a 7 is rolled as no resources are allocated in this scenario.
    // On any other dice roll it's called, it checks every node to see if the dice total matches a hexagon value adjacent to a players settlemnet / city
    public void checkIncrease()
    {
        for (int i = 0; i < board.boardNodes.Length; i++)
        {
            if (board.boardNodes[i].hasRobber == false)
            {
                if (diceTotal == board.boardNodes[i].oHex)
                {
                    if (board.boardNodes[i].houseColour == 1)
                    {
                        if (board.boardNodes[i].oHexResource == "lumber")
                        {
                            playerStateManager.player1.addResources(1);
                        }
                        else if (board.boardNodes[i].oHexResource == "grain")
                        {
                            playerStateManager.player1.addResources(2);
                        }
                        else if (board.boardNodes[i].oHexResource == "brick")
                        {
                            playerStateManager.player1.addResources(3);
                        }
                        else if (board.boardNodes[i].oHexResource == "ore")
                        {
                            playerStateManager.player1.addResources(4);
                        }
                        else if (board.boardNodes[i].oHexResource == "wool")
                        {
                            playerStateManager.player1.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 2)
                    {
                        if (board.boardNodes[i].oHexResource == "lumber")
                        {
                            playerStateManager.player2.addResources(1);
                        }
                        else if (board.boardNodes[i].oHexResource == "grain")
                        {
                            playerStateManager.player2.addResources(2);
                        }
                        else if (board.boardNodes[i].oHexResource == "brick")
                        {
                            playerStateManager.player2.addResources(3);
                        }
                        else if (board.boardNodes[i].oHexResource == "ore")
                        {
                            playerStateManager.player2.addResources(4);
                        }
                        else if (board.boardNodes[i].oHexResource == "wool")
                        {
                            playerStateManager.player2.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 3)
                    {
                        if (board.boardNodes[i].oHexResource == "lumber")
                        {
                            playerStateManager.player3.addResources(1);
                        }
                        else if (board.boardNodes[i].oHexResource == "grain")
                        {
                            playerStateManager.player3.addResources(2);
                        }
                        else if (board.boardNodes[i].oHexResource == "brick")
                        {
                            playerStateManager.player3.addResources(3);
                        }
                        else if (board.boardNodes[i].oHexResource == "ore")
                        {
                            playerStateManager.player3.addResources(4);
                        }
                        else if (board.boardNodes[i].oHexResource == "wool")
                        {
                            playerStateManager.player3.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 4)
                    {
                        if (board.boardNodes[i].oHexResource == "lumber")
                        {
                            playerStateManager.player4.addResources(1);
                        }
                        else if (board.boardNodes[i].oHexResource == "grain")
                        {
                            playerStateManager.player4.addResources(2);
                        }
                        else if (board.boardNodes[i].oHexResource == "brick")
                        {
                            playerStateManager.player4.addResources(3);
                        }
                        else if (board.boardNodes[i].oHexResource == "ore")
                        {
                            playerStateManager.player4.addResources(4);
                        }
                        else if (board.boardNodes[i].oHexResource == "wool")
                        {
                            playerStateManager.player4.addResources(5);
                        }
                    }
                }
                if (diceTotal == board.boardNodes[i].rHex)
                {
                    if (board.boardNodes[i].houseColour == 1)
                    {
                        if (board.boardNodes[i].rHexResource == "lumber")
                        {
                            playerStateManager.player1.addResources(1);
                        }
                        else if (board.boardNodes[i].rHexResource == "grain")
                        {
                            playerStateManager.player1.addResources(2);
                        }
                        else if (board.boardNodes[i].rHexResource == "brick")
                        {
                            playerStateManager.player1.addResources(3);
                        }
                        else if (board.boardNodes[i].rHexResource == "ore")
                        {
                            playerStateManager.player1.addResources(4);
                        }
                        else if (board.boardNodes[i].rHexResource == "wool")
                        {
                            playerStateManager.player1.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 2)
                    {
                        if (board.boardNodes[i].rHexResource == "lumber")
                        {
                            playerStateManager.player2.addResources(1);
                        }
                        else if (board.boardNodes[i].rHexResource == "grain")
                        {
                            playerStateManager.player2.addResources(2);
                        }
                        else if (board.boardNodes[i].rHexResource == "brick")
                        {
                            playerStateManager.player2.addResources(3);
                        }
                        else if (board.boardNodes[i].rHexResource == "ore")
                        {
                            playerStateManager.player2.addResources(4);
                        }
                        else if (board.boardNodes[i].rHexResource == "wool")
                        {
                            playerStateManager.player2.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 3)
                    {
                        if (board.boardNodes[i].rHexResource == "lumber")
                        {
                            playerStateManager.player3.addResources(1);
                        }
                        else if (board.boardNodes[i].rHexResource == "grain")
                        {
                            playerStateManager.player3.addResources(2);
                        }
                        else if (board.boardNodes[i].rHexResource == "brick")
                        {
                            playerStateManager.player3.addResources(3);
                        }
                        else if (board.boardNodes[i].rHexResource == "ore")
                        {
                            playerStateManager.player3.addResources(4);
                        }
                        else if (board.boardNodes[i].rHexResource == "wool")
                        {
                            playerStateManager.player3.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 4)
                    {
                        if (board.boardNodes[i].rHexResource == "lumber")
                        {
                            playerStateManager.player4.addResources(1);
                        }
                        else if (board.boardNodes[i].rHexResource == "grain")
                        {
                            playerStateManager.player4.addResources(2);
                        }
                        else if (board.boardNodes[i].rHexResource == "brick")
                        {
                            playerStateManager.player4.addResources(3);
                        }
                        else if (board.boardNodes[i].rHexResource == "ore")
                        {
                            playerStateManager.player4.addResources(4);
                        }
                        else if (board.boardNodes[i].rHexResource == "wool")
                        {
                            playerStateManager.player4.addResources(5);
                        }
                    }
                }
                if (diceTotal == board.boardNodes[i].lHex)
                {
                    if (board.boardNodes[i].houseColour == 1)
                    {
                        if (board.boardNodes[i].lHexResource == "lumber")
                        {
                            playerStateManager.player1.addResources(1);
                        }
                        else if (board.boardNodes[i].lHexResource == "grain")
                        {
                            playerStateManager.player1.addResources(2);
                        }
                        else if (board.boardNodes[i].lHexResource == "brick")
                        {
                            playerStateManager.player1.addResources(3);
                        }
                        else if (board.boardNodes[i].lHexResource == "ore")
                        {
                            playerStateManager.player1.addResources(4);
                        }
                        else if (board.boardNodes[i].lHexResource == "wool")
                        {
                            playerStateManager.player1.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 2)
                    {
                        if (board.boardNodes[i].lHexResource == "lumber")
                        {
                            playerStateManager.player2.addResources(1);
                        }
                        else if (board.boardNodes[i].lHexResource == "grain")
                        {
                            playerStateManager.player2.addResources(2);
                        }
                        else if (board.boardNodes[i].lHexResource == "brick")
                        {
                            playerStateManager.player2.addResources(3);
                        }
                        else if (board.boardNodes[i].lHexResource == "ore")
                        {
                            playerStateManager.player2.addResources(4);
                        }
                        else if (board.boardNodes[i].lHexResource == "wool")
                        {
                            playerStateManager.player2.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 3)
                    {
                        if (board.boardNodes[i].lHexResource == "lumber")
                        {
                            playerStateManager.player3.addResources(1);
                        }
                        else if (board.boardNodes[i].lHexResource == "grain")
                        {
                            playerStateManager.player3.addResources(2);
                        }
                        else if (board.boardNodes[i].lHexResource == "brick")
                        {
                            playerStateManager.player3.addResources(3);
                        }
                        else if (board.boardNodes[i].lHexResource == "ore")
                        {
                            playerStateManager.player3.addResources(4);
                        }
                        else if (board.boardNodes[i].lHexResource == "wool")
                        {
                            playerStateManager.player3.addResources(5);
                        }
                    }
                    else if (board.boardNodes[i].houseColour == 4)
                    {
                        if (board.boardNodes[i].lHexResource == "lumber")
                        {
                            playerStateManager.player4.addResources(1);
                        }
                        else if (board.boardNodes[i].lHexResource == "grain")
                        {
                            playerStateManager.player4.addResources(2);
                        }
                        else if (board.boardNodes[i].lHexResource == "brick")
                        {
                            playerStateManager.player4.addResources(3);
                        }
                        else if (board.boardNodes[i].lHexResource == "ore")
                        {
                            playerStateManager.player4.addResources(4);
                        }
                        else if (board.boardNodes[i].lHexResource == "wool")
                        {
                            playerStateManager.player4.addResources(5);
                        }
                    }
                }
            }
        }  
    }

    public void halfResources()
    {
        // We remember which player rolled the 7
        GameObject.Find("DiceRollTotal").GetComponent<Text>().text = "= " + diceTotal;
        initialPlayerToRemove = playerStateManager.currentPlayerNumber;
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            if (halfResourcePanelG != null)
            {
                if (halfResourcePanelG.activeInHierarchy == false)
                {
                    halfResourcePanelG.SetActive(true);
                    return;
                }
            }
        }
        else
        {
            playerStateManager.SwitchState();
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            if (halfResourcePanelG.activeInHierarchy == false)
            {
                halfResourcePanelG.SetActive(true);
                return;
            }
        }
        else
        {
            playerStateManager.SwitchState();
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            if (halfResourcePanelG.activeInHierarchy == false)
            {
                halfResourcePanelG.SetActive(true);
                return;
            }
        }
        else
        {
            playerStateManager.SwitchState();
        }
        if (playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources > 7)
        {
            toRemove = playerStateManager.getCurrentPlayer(playerStateManager.currentPlayerNumber).totalResources / 2;
            if (halfResourcePanelG.activeInHierarchy == false)
            {
                halfResourcePanelG.SetActive(true);
                return;
            }
        }
        else
        {
            playerStateManager.SwitchState();
        }
        GameObject.Find("RollDiceButton").GetComponent<Button>().interactable = false;
        Debug.Log("Please relocate the robber");
        GameObject.Find("End Turn Button").GetComponent<Button>().interactable = false;
    }
}

