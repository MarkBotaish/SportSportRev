using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    public float speed;

    Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //Will be reworked for DDPAD
        if (isPlayerOne)
            playerOneMovement();
        else
            playerTwoMovement();
	}

    void playerOneMovement()
    {
        Vector2 vel = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            vel += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            vel += Vector2.right;

        rigid.velocity = vel * speed;
    }

    void playerTwoMovement()
    {
        Vector2 vel = Vector2.zero;
        if (Input.GetKey(KeyCode.Keypad8))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.Keypad5))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.Keypad4))
            vel += Vector2.right;
        if (Input.GetKey(KeyCode.Keypad6))
            vel += Vector2.left;

        rigid.velocity = vel * speed;
    }
}
