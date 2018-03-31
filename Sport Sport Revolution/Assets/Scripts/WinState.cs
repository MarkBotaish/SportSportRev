using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class WinState : MonoBehaviour {

    public GameObject panel;
    public GameObject playerOne;
    public GameObject playerTwo;

	Player player;
    Player player2;
    public int numOfObjects = 0;
	public bool canRestart = false;

	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer (0);
        player2 = ReInput.players.GetPlayer(1);
    }
	
	// Update is called once per frame
	void Update () {
		if ((player.GetButtonDown ("Interact") || player2.GetButtonDown("Interact")) && canRestart) 
			SceneManager.LoadScene ("MainMenu");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Chest")
		{
			numOfObjects++;
			if(numOfObjects >= 2)
			{
				GameObject.Find ("Timer").GetComponent<Timer> ().enabled = false;
				GameObject.Find ("WinText").GetComponent<Text> ().enabled = true;
				canRestart = true;
                panel.SetActive(true);
                playerOne.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                playerTwo.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                playerOne.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                playerTwo.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Chest")
		{
			numOfObjects--;
		}
	}
}
