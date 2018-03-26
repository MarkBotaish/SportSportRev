using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    bool hasPickedUp = false;

    public float speed;
    public float ballSpeed;
    public float rotateSpeed;

    Rigidbody2D rigid;

    GameObject ball = null;

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

        pickup(KeyCode.E);
        rotateAround(KeyCode.Q, rotateSpeed);

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

        pickup(KeyCode.Keypad9);
        rotateAround(KeyCode.Keypad7, rotateSpeed);

    }


    void pickup(KeyCode key)
    {
        if (Input.GetKeyDown(key) && ball != null && !hasPickedUp)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
        else if (Input.GetKeyDown(key) && hasPickedUp)
        {
            Vector2 pos = gameObject.transform.position - ball.transform.position;
            pos = -pos.normalized;
            hasPickedUp = false;
            ball.GetComponent<Rigidbody2D>().velocity = pos * ballSpeed;
            ball.GetComponent<BallScript>().setPickUp(false);
            ball.GetComponent<BallScript>().setIsInAir(true);
            ball = null;
            gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (hasPickedUp)
            ball.transform.position = gameObject.transform.GetChild(0).transform.position;
    }

    void rotateAround(KeyCode code, float theta) {
        if (Input.GetKey(code))
            gameObject.transform.eulerAngles += new Vector3(0.0f,0.0f, theta);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball" && ball == null && !hasPickedUp)
            ball = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball" && ball != null && !hasPickedUp)
            ball = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir())
        {
            print("DEAD");
            collision.gameObject.GetComponent<BallScript>().setIsInAir(false);
        }
    }
}
