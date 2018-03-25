using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    bool hasPickedUp = false;

    public float speed;
    public float ballSpeed;

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

        pickup(KeyCode.E, Vector2.up);

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
        pickup(KeyCode.Keypad9, Vector2.down);

    }


    void pickup(KeyCode key, Vector2 direction)
    {
        if (Input.GetKeyDown(key) && ball != null && !hasPickedUp)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
        else if (Input.GetKeyDown(key) && hasPickedUp)
        {
            hasPickedUp = false;
            ball.GetComponent<Rigidbody2D>().velocity = direction * ballSpeed;
            ball.GetComponent<BallScript>().setPickUp(false);
            ball.GetComponent<BallScript>().setIsInAir(true);
            ball = null;
        }

        if (hasPickedUp)
            ball.transform.position = gameObject.transform.GetChild(0).transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball" && ball == null && !hasPickedUp && !collision.GetComponent<BallScript>().getIsInAir())
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
