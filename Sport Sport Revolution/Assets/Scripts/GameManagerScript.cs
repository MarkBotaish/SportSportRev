using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript code;

    public Text roundTextP1;
    public Text roundTextP2;
    public Text playerOneText;
    public Text playerTwoText;
    public Text playerOneRound;
    public Text playerTwoRound;
    public PlayerScript playerOne;
    public PlayerScript playerTwo;
    public int numberOfRoundsToWin;

    public List<StopableObject> objects;

    int round = 0;
    int roundsWonForOne = 0;
    int roundsWonForTwo = 0;

    // Use this for initialization
    void Start () {
        code = this;
        updateRound();
        playerOne.setManager(this);
        playerTwo.setManager(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void firstPlayerWon()
    {
        roundsWonForOne++;
        playerOneText.text = "Won: " + roundsWonForOne;
        updateRound();
        
    }

    public void secondPlayerWon()
    {
        roundsWonForTwo++;
        playerTwoText.text = "Won: " + roundsWonForTwo;
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
            print("Game Over");
        else
            StartCoroutine(roundStart());
    }

    IEnumerator roundStart()
    {
        playerOneRound.text = "Round " + round;
        playerTwoRound.text = "Round " + round;

        playerOneRound.gameObject.SetActive(true);
        playerTwoRound.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < objects.Count; i++)
            objects[i].unfreeze();
        playerOneRound.gameObject.SetActive(false);
        playerTwoRound.gameObject.SetActive(false);
    }
}
