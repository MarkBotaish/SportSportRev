using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript code;

    public Text roundTextP1;
    public Text roundTextP2;
    public Text playerOneText;
    public Text playerTwoText;
    public Text playerOneRound;
    public Text playerTwoRound;

    public GameObject panelOne;
    public GameObject panelTwo;

    public GameObject winScreenOne;
    public GameObject winScreenTwo;

    public PlayerScript playerOne;
    public PlayerScript playerTwo;

    public GameObject finalMessageP1;
    public GameObject finalMessageP2;

    public int numberOfRoundsToWin;

    public List<StopableObject> objects;
    public List<AudioClip> roundSounds;

    List<BallScript> ballList;

    int round = 0;
    int soundIndex = 0;
    int roundsWonForOne = 0;
    int roundsWonForTwo = 0;
    float startingBall = 8.0f;

    bool isEnding = false;
    bool changeScene = false;

    AudioSource audio;

    public float AntiCampDelay;
    bool isMoving = false;
    float timer = 0;


    // Use this for initialization
    void Start () {
        code = this;
        audio = GetComponent<AudioSource>();
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
        playerOneText.text = "Won: " + roundsWonForOne + "/" + round;
        playerTwoText.text = "Won: " + roundsWonForTwo + "/" + round;
        updateRound();
        
    }

    public void secondPlayerWon()
    {
        roundsWonForTwo++;
        playerTwoText.text = "Won: " + roundsWonForTwo + "/" + round;
        playerOneText.text = "Won: " + roundsWonForOne + "/" + round;
        updateRound();
    }
    void updateRound()
    {
        round++;
        roundTextP1.text = "Round #: " + round;
        roundTextP2.text = "Round #: " + round;

      
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
            audio.clip = roundSounds[soundIndex++];
            audio.Play();
            StartCoroutine(roundStart());
        }
        if (round > 1)
        {
            WallManagerScript.code.resetWalls();
        }
          
       
    }

    IEnumerator roundStart()
    {
        playerOneRound.text = "Round " + round;
        playerTwoRound.text = "Round " + round;

        playerOneRound.gameObject.SetActive(true);
        playerTwoRound.gameObject.SetActive(true);
        timer = 0;
        yield return new WaitForSeconds(audio.clip.length - 1);
        for (int i = 0; i < objects.Count; i++)
            objects[i].unfreeze();

        playerOneRound.gameObject.SetActive(false);
        playerTwoRound.gameObject.SetActive(false);
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
