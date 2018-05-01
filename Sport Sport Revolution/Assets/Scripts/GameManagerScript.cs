using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript code;

    [Header("UI Player One")]
    public GameObject blueCatUI_1;
    public GameObject orangeCatUI_1;
  

    [Header("UI Player Two")]
    public GameObject blueCatUI_2;
    public GameObject orangeCatUI_2;


    [Header("General")]
    public Sprite redFill;
    public Sprite blueFill;

    public GameObject readyPlayerOneText;
    public GameObject readyPlayerTwoText;

    public Image playerOneRound;
    public Image playerTwoRound;

    public GameObject panelOne;
    public GameObject panelTwo;

    public GameObject winScreenOne;
    public GameObject winScreenTwo;

    public PlayerScript playerOne;
    public PlayerScript playerTwo;

    public GameObject finalMessageP1;
    public GameObject finalMessageP2;

    public int numberOfRoundsToWin;
    public float timeToSpeedUp = 3;

    public List<StopableObject> objects;
    public List<Sprite> roundUI;


    List<BallScript> ballList;

    float time = 0;
    float speed = 0;
    int round = 0;
    int soundIndex = 0;
    int roundsWonForOne = 0;
    int roundsWonForTwo = 0;
    float startingBall = 8.0f;
   

    bool isEnding = false;
    bool changeScene = false;
    bool shouldUpdateSpeed = false;
    bool checkForStart = false;
    bool playerOneReady = false;
    bool playerTwoReady = false;

    public float AntiCampDelay;
    bool isMoving = false;
    float timer = 0;

    public float getSpeed() { return speed; }
    // Use this for initialization
    void Start () {
        code = this;
        ballList = new List<BallScript>();
        for (int i = 0; i < objects.Count; i++)
            if (objects[i].gameObject.tag == "Ball")
                ballList.Add(objects[i].GetComponent<BallScript>());


        updateRound();
        playerOne.setManager(this);
        playerTwo.setManager(this);

       
    }
	
	// Update is called once per frame
	void Update () {

        if (checkForStart)
        {
            checkStart();
            return;
        }
        
        if (isEnding)
        {
            if (!finalMessageP1.GetComponent<Animation>().isPlaying)
            {
                isEnding = false;
                StartCoroutine(waitToChangeScreens());
            }
        }else if (changeScene)
        {
            changePanel();
        }  
        else
            checkAntiCamp();

        if(shouldUpdateSpeed)
            updateTime();
    }

    void checkStart()
    {
        if (Input.GetKey(KeyCode.F))
            playerOneReady = true;


        if (Input.GetKey(KeyCode.Keypad0))
            playerTwoReady = true;



        if (playerTwoReady && playerOneReady)
        {
            for (int i = 0; i < objects.Count; i++)
                objects[i].unfreeze();

            playerOneRound.gameObject.SetActive(false);
            playerTwoRound.gameObject.SetActive(false);

            orangeCatUI_1.SetActive(false);
            orangeCatUI_2.SetActive(false);

            blueCatUI_1.SetActive(false);
            blueCatUI_2.SetActive(false);

            shouldUpdateSpeed = true;
            time = 0;

            checkForStart = false;
            playerOneReady = false;
            playerTwoReady = false;
        }
    }

    void updateTime()
    {
        time += Time.deltaTime;
        speed = Mathf.Pow((time / timeToSpeedUp), 3) + 0.2f;

        if (time >= timeToSpeedUp)
        {
            shouldUpdateSpeed = false;
            speed = 1.0f;
        }
        for (int i = 0; i < objects.Count; i++)
            objects[i].setSpeedMultiplier(speed);
            
    }

    void checkAntiCamp()
    {
        if (timer >= AntiCampDelay)
        {
            if (!isMoving)
            {
                WallManagerScript.code.spawnWalls();
                for (int i = 0; i < ballList.Count; i++)
                    ballList[i].moveSpawnPositionIn(1.5f);
            }

            isMoving = WallManagerScript.code.antiCamper();

            if (!isMoving)
            {
                timer = 0;
                startingBall -= 1.5f;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void firstPlayerWon()
    {
        roundsWonForOne++;

        orangeCatUI_1.transform.GetChild(roundsWonForOne - 1).GetComponent<Image>().sprite = redFill;
        orangeCatUI_2.transform.GetChild(roundsWonForOne - 1).GetComponent<Image>().sprite = redFill;

        playerOneRound.GetComponent<Image>().color = Color.red;
        playerTwoRound.GetComponent<Image>().color = Color.red;

        updateRound();
        
    }

    public void secondPlayerWon()
    {
        roundsWonForTwo++;

        blueCatUI_1.transform.GetChild(roundsWonForTwo - 1).GetComponent<Image>().sprite = blueFill;
        blueCatUI_2.transform.GetChild(roundsWonForTwo - 1).GetComponent<Image>().sprite = blueFill;

        playerOneRound.GetComponent<Image>().color = Color.blue;
        playerTwoRound.GetComponent<Image>().color = Color.blue;

        updateRound();
    }
    void updateRound()
    {
        round++;

        for (int i = 0; i < objects.Count; i++)
            objects[i].restart();


        if (roundsWonForTwo == numberOfRoundsToWin || roundsWonForOne == numberOfRoundsToWin)
        {
            if(roundsWonForOne == numberOfRoundsToWin)
            {
                finalMessageP1.GetComponent<TextMeshProUGUI>().text = "Winner";
                finalMessageP2.GetComponent<TextMeshProUGUI>().text = "Defeat";
            }

            isEnding = true;
            finalMessageP1.GetComponent<Animation>().Play("Final Message");
            finalMessageP2.GetComponent<Animation>().Play("Final Message");
        }
        else
        {
            roundStart();
        }
        if (round > 1)
        {
            WallManagerScript.code.resetWalls();
        }
          
       
    }

    void roundStart()
    {
        playerOneRound.sprite = roundUI[round - 1];
        playerTwoRound.sprite = roundUI[round - 1];

        playerOneRound.gameObject.SetActive(true);
        playerTwoRound.gameObject.SetActive(true);

        orangeCatUI_1.SetActive(true);
        orangeCatUI_2.SetActive(true);

        blueCatUI_1.SetActive(true);
        blueCatUI_2.SetActive(true);

        timer = 0;

        readyPlayerOneText.SetActive(true);
        readyPlayerTwoText.SetActive(true);

        checkForStart = true;

    }

    IEnumerator waitToChangeScreens()
    {
        yield return new WaitForSeconds(2.0f);
        changeScene = true;
    }

    void changePanel()
    {
        if(panelOne.GetComponent<Image>().color.a <= 1.0)
        {
            panelOne.GetComponent<Image>().color += new Color(0, 0, 0, 0.05f);
            panelTwo.GetComponent<Image>().color += new Color(0, 0, 0, 0.05f);
        }
        else
        {
            winScreenOne.SetActive(true);
            winScreenTwo.SetActive(true);

            if (roundsWonForTwo == numberOfRoundsToWin)
            {
                winScreenOne.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player 2 Wins!";
                winScreenTwo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Player 2 Wins!";
            }
               
        }
    }

    public void togglePause()
    {
        for (int i = 0; i < objects.Count; i++)
            objects[i].togglePause();
    }

    public float getBallLength() { return startingBall; }

   
}
