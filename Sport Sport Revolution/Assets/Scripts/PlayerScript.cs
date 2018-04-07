using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour {

    public int playerId;

    bool hasPickedUp = false;
    bool isDead = false;
    bool isFrozen = false;

    public float speed;
    public float pullSpeed;
    public float freezeTime;

    Rigidbody2D rigid;
    GameManagerScript code;

    GameObject ball = null;

    private Vector3 startingPos;

    // Use this for initialization
    void Start() {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        startingPos = gameObject.transform.position;
        code = GameManagerScript.code;
    }

    // Update is called once per frame
    void Update() {

        if (playerId == 0 && !isDead)
            playerOneMovement();
        else if (playerId == 0 && isDead)
            rigid.velocity = Vector2.up * pullSpeed * speed;
        else if (playerId == 1 && !isDead)
            playerTwoMovement();
        else
            rigid.velocity = Vector2.down * pullSpeed * speed;


        if (Mathf.Abs(gameObject.transform.position.y) < 0.6)
        {
            if (playerId == 1)
                code.firstPlayerWon();
            else
                code.secondPlayerWon();
        }

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

        pickup(KeyCode.R, 1.0f);
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
        else if ((Input.GetKeyDown(key)) && hasPickedUp)
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

        if (collision.transform.tag == "Ball" && ball == collision.gameObject)
        {
            ball = null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir() && collision.gameObject.GetComponent<BallScript>().getThrownPlayer() != gameObject)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			isDead = true;
            //print("DEAD");
            collision.gameObject.GetComponent<BallScript>().setIsInAir(false);
            StartCoroutine( freeze());
        }
    }


    IEnumerator freeze()
    {
        if(playerId == 0)
            rigid.velocity = Vector2.up * pullSpeed;
        else
            rigid.velocity = Vector2.down * pullSpeed;

        isFrozen = true;
        yield return new WaitForSeconds(freezeTime);
        isFrozen = false;
        isDead = false;
    }

    public void resetPlayer()
    {
        rigid.velocity = Vector3.zero;
        gameObject.transform.position = startingPos;
    }
}
