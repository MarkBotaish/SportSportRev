using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MainMenu : MonoBehaviour {

	GameObject play;
	GameObject quit;

	public int playerId;
	Player player;

	bool playGame;

	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer (playerId);
		play = GameObject.Find ("start-menu_0001_play");
		quit = GameObject.Find ("start-menu_0000_Quit");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetButtonDown ("MoveToPlay")) {
			playGame = true;
			play.GetComponent<SpriteRenderer> ().color = Color.red * Color.yellow;
			quit.GetComponent<SpriteRenderer> ().color = Color.white;
		}

		if (player.GetButtonDown ("MoveToQuit")) {
			playGame = false;
			play.GetComponent<SpriteRenderer> ().color = Color.white;
			quit.GetComponent<SpriteRenderer> ().color = Color.blue;
		}

		if (player.GetButtonDown ("Select")) {
			if (playGame)
				SceneManager.LoadScene ("Tutorial_Test");
			else if (!playGame)
				Application.Quit ();
		}

        if (player.GetButtonDown("Interact"))
        {
            SceneManager.LoadScene("Help");
        }

    }
}
