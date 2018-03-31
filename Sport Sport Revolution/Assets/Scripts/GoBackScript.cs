using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GoBackScript : MonoBehaviour {

    Player player;

    // Use this for initialization
    void Start () {
        player = ReInput.players.GetPlayer(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetButtonDown("GoBack"))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}
