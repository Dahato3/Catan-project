using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Robber : MonoBehaviour
{
    [SerializeField] GameObject board;
    Board myBoard;

    public GameObject robber;
    public GameObject robberN;

    //[SerializeField] GameObject robber;


    public void Awake()
    {
        robber = (GameObject)Resources.Load("robber");
        robberN = (GameObject)Resources.Load("robber");
        board = GameObject.Find("Board");
        myBoard = board.GetComponent<Board>();
    }
    // Start is called before the first frame update
    void Start()
    {

        if (GameObject.Find("initialRobber") == null)
        {
            GameObject robberT = Instantiate(robber, new Vector3(0, 0, 0), Quaternion.identity);
            robberT.name = "initialRobber";
            robberT.transform.position = new Vector3(0, 17.5f, 0);
            robberT.transform.localScale = new Vector3(30, 1.75f, 30);
            robberT.GetComponent<MeshRenderer>().enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled == true)
            {
                GameObject.Find("initialRobber").GetComponent<MeshRenderer>().enabled = false;
            }
            Debug.Log(gameObject.name + " Clicked!");

            if (GameObject.Find("robberMain") == null)
            {
                robberN = Instantiate(robber);
                robberN.GetComponent<MeshRenderer>().enabled = true;
                robberN.name = "robberMain";
                
            }

            robberN.transform.position = gameObject.transform.position;

            Vector3 temp = robberN.transform.position;
            Debug.Log(temp);
            temp[0] = temp[0] - 100;
            temp[1] = temp[1] + 17.5f;

            robberN.transform.position = temp;
            robberN.transform.localScale = new Vector3(30, 1.75f, 30);


            int hexPosition = int.Parse(gameObject.name);

            Debug.Log(hexPosition);

            for (int i = 0; i < myBoard.boardNodes.Length; i++)
            {
                Debug.Log("node: " + i);
                Debug.Log("lHexLocation: " + myBoard.boardNodes[i].lHexLocation);
                Debug.Log("rHexLocation: " + myBoard.boardNodes[i].rHexLocation);
                Debug.Log("oHexLocation: " + myBoard.boardNodes[i].oHexLocation);
                if (myBoard.boardNodes[i].lHexLocation == hexPosition
                    || myBoard.boardNodes[i].rHexLocation == hexPosition
                    || myBoard.boardNodes[i].oHexLocation == hexPosition)
                {
                    
                    myBoard.boardNodes[i].hasRobber = true;
                }
                myBoard.boardNodes[i].hasRobber = false;
            }

            


            // Got the base functionality working
            // BUT hasRobber doesnt stop resources being allocated
            // AND still need to be able to move robber again

            // Need to also implement the stealing 1 resource when placed


        }
    }
}
