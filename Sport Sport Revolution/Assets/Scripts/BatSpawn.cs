using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpawn : MonoBehaviour {

	public GameObject batSwarm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			Instantiate (batSwarm);
		} else if (col.gameObject.tag == "Bat") {
			Destroy (col.gameObject);
		}
	}
}
