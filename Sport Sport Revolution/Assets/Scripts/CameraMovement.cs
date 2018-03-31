using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public GameObject playerToFollow;
    public float yOffset;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(playerToFollow.transform.position.x, playerToFollow.transform.position.y + yOffset, -10.0f);
	}
}
