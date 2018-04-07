using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour {

    public int playerId;

    bool hasPickedUp = false;
	bool isDead = false;

    public float speed;
    public float pullSpeed;

    Rigidbody2D rigid;

    GameObject ball = null;

	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

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
        if (Input.GetKey(KeyCode.Q))
            vel += (Vector2.left + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.E))
            vel += (Vector2.right + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.LeftShift))
            vel += (Vector2.left + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.C))
            vel += (Vector2.right + Vector2.down).normalized;

        rigid.velocity = vel * speed;

        pickup(KeyCode.R,1.0f);
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
        if (Input.GetKey(KeyCode.Keypad7))
            vel += (Vector2.right + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.Keypad9))
            vel += (Vector2.left + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.Keypad1))
            vel += (Vector2.right + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.Keypad3))
            vel += (Vector2.left + Vector2.up).normalized;

        rigid.velocity = vel * speed;

        pickup(KeyCode.KeypadPlus, -1.0f);

    }


	void pickup(KeyCode key, float y)
    {
		if (ball != null && !hasPickedUp && !ball.GetComponent<BallScript>().getIsInAir() && !isDead)
        {
            ball.GetComponent<BallScript>().reset();
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
        }
		else if ((Input.GetKeyDown(key))&& hasPickedUp)
        {
			Vector2 pos = gameObject.transform.position - ball.transform.position;
			pos = -pos.normalized;
			pos = rigid.velocity.normalized;
			pos.y = y;
            hasPickedUp = false;
            ball.GetComponent<BallScript>().throwBall(pos);
            ball.GetComponent<BallScript>().setPickUp(false);
            ball.GetComponent<BallScript>().setIsInAir(true);
            ball.GetComponent<BallScript>().setThrownPlayer(gameObject);
            ball = null;
            gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            print("why");
        }

        //Might need to change
        if (ball == null)
            hasPickedUp = false;

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
		if (collision.transform.tag == "Ball" && isDead && ball == collision.gameObject) {
			isDead = false;
			ball = null;
		}

        if (collision.transform.tag == "Ball" && ball == collision.gameObject)
        {
            ball = null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir() && collision.gameObject.GetComponent<BallScript>().getThrownPlayer() != gameObject)
        {
			isDead = true;
            print("DEAD");
            collision.gameObject.GetComponent<BallScript>().setIsInAir(false);
        }
    }
}
