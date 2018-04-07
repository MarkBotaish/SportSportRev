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
    public PlayerScript playerOne;
    public PlayerScript playerTwo;

    int round = 0;
    int roundsWonForOne = 0;
    int roundsWonForTwo = 0;

    // Use this for initialization
    void Start () {
        code = this;
        updateRound();
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

        if (round <= 1)
            return;
        playerOne.resetPlayer();
        playerTwo.resetPlayer();
    }
}
