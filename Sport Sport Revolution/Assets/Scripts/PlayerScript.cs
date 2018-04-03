using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerScript : MonoBehaviour {

    public int playerId;

    bool hasPickedUp = false;
	bool isDead = false;

    public float speed;
    public float pullSpeed;

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
        Vector2 vel = Vector2.up * pullSpeed;
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
		if (ball != null && !hasPickedUp && !ball.GetComponent<BallScript>().getIsInAir() && !isDead)
        {
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
		else if ((player.GetButton("Circle") || player.GetButton("X")) && hasPickedUp)
        {
            Vector2 pos = gameObject.transform.position - ball.transform.position;
            pos = -pos.normalized;
            pos = rigid.velocity.normalized;
			if (playerId == 0)
				pos.y = 1.0f;
			else
				pos.y = -1.0f;
            hasPickedUp = false;
            ball.GetComponent<BallScript>().throwBall(pos);
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
		Vector2 vel = Vector2.up * pullSpeed;
        if (Input.GetKey(KeyCode.W))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            vel += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            vel += Vector2.right;

        rigid.velocity = vel * speed;

        pickup(KeyCode.E, KeyCode.Q,1.0f);
    }

    void playerTwoMovement()
    {
		Vector2 vel = Vector2.down * pullSpeed;
        if (Input.GetKey(KeyCode.Keypad8))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.Keypad5))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.Keypad4))
            vel += Vector2.right;
        if (Input.GetKey(KeyCode.Keypad6))
            vel += Vector2.left;

        rigid.velocity = vel * speed;

        pickup(KeyCode.Keypad9, KeyCode.Keypad7, -1.0f);

    }


	void pickup(KeyCode key, KeyCode key2, float y)
    {
		if (ball != null && !hasPickedUp && !ball.GetComponent<BallScript>().getIsInAir() && !isDead)
        {
            ball.GetComponent<BallScript>().reset();
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
		else if ((Input.GetKeyDown(key) || Input.GetKeyDown(key2))&& hasPickedUp)
        {
			Vector2 pos = gameObject.transform.position - ball.transform.position;
			pos = -pos.normalized;
			pos = rigid.velocity.normalized;
			pos.y = y;
            hasPickedUp = false;
            ball.GetComponent<BallScript>().throwBall(pos);
            ball.GetComponent<BallScript>().setPickUp(false);
            ball.GetComponent<BallScript>().setIsInAir(true);
            ball = null;
            gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (hasPickedUp)
            ball.transform.position = gameObject.transform.GetChild(0).transform.position;


        //Might need to change
        if (ball == null)
            hasPickedUp = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball" && ball == null && !hasPickedUp)
            ball = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.transform.tag == "Ball" && isDead && ball == collision.gameObject) {
			isDead = false;
			ball = null;
		}

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir())
        {
			isDead = true;
            print("DEAD");
            collision.gameObject.GetComponent<BallScript>().setIsInAir(false);
        }
    }
}
