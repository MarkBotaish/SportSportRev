using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : StopableObject {

    public int playerId;

    bool hasPickedUp = false;
    bool isDead = false;
    bool isStunned = false;
    bool isInitted = false;
    bool hasLeftSpace = true;

    public float speed;
    public float pullSpeed;
    public float freezeTime;
    public float throwAngle;

    private Vector2 throwingAngle;

    Rigidbody2D rigid;
    GameManagerScript code;

    GameObject ball = null;

    public Color tint;
    private Color startingColor;

    private Vector3 startingPos;

    public void setManager(GameManagerScript manage) { code = manage; }

    // Use this for initialization
    void Start() {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        startingPos = gameObject.transform.position;
        startingColor = gameObject.GetComponent<SpriteRenderer>().color;
        isInitted = true;

    }

    // Update is called once per frame
    void Update() {

        if (isForzen)
            return;

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
        if (Input.GetKeyDown(KeyCode.Alpha3))
            throwingAngle = new Vector2(Mathf.Cos(throwAngle*(Mathf.PI/180)), Mathf.Sin(throwAngle * (Mathf.PI / 180)));
        if (Input.GetKeyDown(KeyCode.Alpha1))
            throwingAngle = new Vector2(Mathf.Cos((180-throwAngle) * (Mathf.PI / 180)), Mathf.Sin((180 - throwAngle) * (Mathf.PI / 180)));

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
        if (Input.GetKeyDown(KeyCode.KeypadDivide))
            throwingAngle = new Vector2(Mathf.Cos(throwAngle * (Mathf.PI / 180)), Mathf.Sin(throwAngle * (Mathf.PI / 180)));
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            throwingAngle = new Vector2(Mathf.Cos((180 - throwAngle) * (Mathf.PI / 180)), Mathf.Sin((180 - throwAngle) * (Mathf.PI / 180)));

        if (Input.GetKey(KeyCode.KeypadMinus))
            print("asdghuop");

        rigid.velocity = vel * speed;

        pickup(KeyCode.KeypadPlus, -1.0f);

    }


    void pickup(KeyCode key, float y)
    {
        if (ball != null && !hasPickedUp && !ball.GetComponent<BallScript>().getIsInAir() && !isDead)
        {
            ball.GetComponent<BallScript>().playerRestart();
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hasPickedUp = true;
            ball.GetComponent<BallScript>().setPickUp(true);
            throwingAngle = new Vector2(0,1);
            hasLeftSpace = false;   

        }
        else if ((Input.GetKeyDown(key)) && hasPickedUp)
        {
            BallScript ballScript = ball.GetComponent<BallScript>();

            if (playerId == 1)
                throwingAngle.y *= -1f;

            ballScript.throwBall(throwingAngle);
            ballScript.setPickUp(false);
            ballScript.setIsInAir(true);
            ballScript.setThrownPlayer(this);
            ball = null;
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
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir())
        {
            if(collision.gameObject.GetComponent<BallScript>().getThrownPlayer() == this && hasLeftSpace)
                hit(collision.gameObject);
        }
    }

    public void hit(GameObject col)
    {
        col.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        isDead = true;
        gameObject.GetComponent<SpriteRenderer>().color = tint;
        print("DEAD");
        col.GetComponent<BallScript>().setIsInAir(false);
        StartCoroutine(freeze());
    }


    IEnumerator freeze()
    {
        if (!isStunned)
        {
            if (playerId == 0)
                rigid.velocity = Vector2.up * pullSpeed;
            else
                rigid.velocity = Vector2.down * pullSpeed;

            isStunned = true;
            yield return new WaitForSeconds(freezeTime);
            isStunned = false;
            isDead = false;
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
        }
    }

    public override void restart()
    {
        base.restart();
        if (isInitted)
        {
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
            rigid.velocity = Vector3.zero;
            gameObject.transform.position = startingPos;
        }
    }

    public bool getAction()
    {
        if ((playerId == 0 && Input.GetKey(KeyCode.F)) || (playerId == 1 && Input.GetKey(KeyCode.KeypadMinus)))
            return true;
        return false;
    }

    public void LeftSpace() { hasLeftSpace = true; }
}
