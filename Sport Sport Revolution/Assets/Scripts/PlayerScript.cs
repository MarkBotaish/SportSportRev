using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerScript : MonoBehaviour {

    public int playerId;

    bool hasPickedUp = false;

    public float speed;
    public float ballSpeed;
    public float rotateSpeed;

    Rigidbody2D rigid;

    GameObject ball = null;

    Player player;

	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        player = ReInput.players.GetPlayer(playerId);
	}
	
	// Update is called once per frame
	void Update () {

        devsInput();
       checkInput();
       checkBall();
	}

    void checkInput()
    {
        Vector2 vel = Vector2.zero;
        if (player.GetButton("Left"))
            vel += Vector2.left;
        if (player.GetButton("Right"))
            vel += Vector2.right;
        if (player.GetButton("Up"))
            vel += Vector2.up;
        if (player.GetButton("down"))
            vel += Vector2.down;

        if (playerId == 1)
            vel = -vel;
        rigid.velocity = vel * speed;
    }

    void checkBall()
    {
        if (player.GetButton("X") && ball != null && !hasPickedUp)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
        else if (player.GetButton("Circle") && hasPickedUp)
        {
            Vector2 pos = gameObject.transform.position - ball.transform.position;
            pos = -pos.normalized;
            pos += rigid.velocity.normalized;
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

    void devsInput()
    {
        if (playerId == 0)
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

        pickup(KeyCode.E, KeyCode.Q);
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

        pickup(KeyCode.Keypad9, KeyCode.Keypad7);

    }


    void pickup(KeyCode key, KeyCode key2)
    {
        if (Input.GetKeyDown(key2) && ball != null && !hasPickedUp)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
        else if (Input.GetKeyDown(key) && hasPickedUp)
        {
            Vector2 pos = gameObject.transform.position - ball.transform.position;
            pos = -pos.normalized;
            pos += rigid.velocity.normalized ;
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
