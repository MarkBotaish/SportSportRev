using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ObjectInteraction : MonoBehaviour {

	public int playerId;
	private Player player;
	private Transform arm;

	bool canHold = false;

	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer (playerId);
		arm = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(player.GetButtonDown("Interact") && arm != null && canHold){
			transform.SetParent (arm);
			Debug.Log ("Interact");
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Arm") {
			Debug.Log ("Trigger Enter");
			canHold = true;
			arm = col.transform;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Arm") {
			Debug.Log ("Trigger Exit");
			canHold = false;
			arm = null;
		}
	}
}
