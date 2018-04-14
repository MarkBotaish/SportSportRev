using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Base, Laser, Magic, Explosive
}


public class BallScript : StopableObject {

    public float ballThrownSpeed;
    public float fallSpeed;
    protected float spawnPosition = 8.0f;
    Color startingColor;
    public BallType type;
    protected bool isInitted = false;


    protected PlayerScript thrownPlayer;

    protected bool hasBeenPickedUp = false;
    protected bool isInAir = false;
    protected Rigidbody2D rigid;
    public int bounceCount;
    protected int startingBounceCount;
    protected float startingSpawnPosition;

    virtual public void moveSpawnPositionIn(float offset) { spawnPosition -= offset; }
    virtual public void setPickUp(bool tof) { hasBeenPickedUp = tof; }
    virtual public bool getPickUp() { return hasBeenPickedUp; }
    virtual public void setIsInAir(bool tof) { isInAir = tof; }
    virtual public bool getIsInAir() { return isInAir; }
    virtual public void throwBall(Vector2 vel) { rigid.velocity = vel * ballThrownSpeed; }
    virtual protected void doAction() { thrownPlayer = null; }
    public void setThrownPlayer(PlayerScript obj) { thrownPlayer = obj; }
    public PlayerScript getThrownPlayer() { return thrownPlayer; }

    virtual protected void Start() {
        startingSpawnPosition = spawnPosition;
        startingBounceCount = bounceCount;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        startingColor = gameObject.GetComponent<SpriteRenderer>().color;
        isInitted = true;
    }

    public virtual void Update()
    {
        if (isForzen)
            return;

        checkThrownBall();
        if (Mathf.Abs(transform.position.y) < 0.5 && !isInAir && !hasBeenPickedUp)
            respawn();

        if (Mathf.Abs(rigid.velocity.y) < fallSpeed && !isInAir && !hasBeenPickedUp)
        {
            if (transform.position.y > 0)
                rigid.velocity = new Vector2(rigid.velocity.x, -fallSpeed);
            else
                rigid.velocity = new Vector2(rigid.velocity.x, fallSpeed);
        }

        if (thrownPlayer != null && thrownPlayer.getAction())
            doAction();



    }

    protected void respawn()
    {
        int rand = Random.Range(0, 2);
        if (rigid != null)
            rigid.velocity = Vector2.zero;

        if (rand == 0)
        {
            transform.position = new Vector3(transform.position.x, spawnPosition, 0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -spawnPosition, 0);
        }

        bounceCount = startingBounceCount;
    }



    virtual protected void checkThrownBall()
    {
        if (rigid.velocity != Vector2.zero && rigid.velocity.magnitude < 0.4)
        {
            rigid.velocity = Vector2.zero;
        }

        if (isInAir)
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        else
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
    }

    override public void restart() {
        base.restart();
        isInAir = false;
        hasBeenPickedUp = false;
        respawn();
        if (isInitted)
            playerRestart();

    }

    virtual public void playerRestart()
    { 
        rigid.velocity = Vector2.zero;
    }

    public override void unfreeze()
    {
        base.unfreeze();
    }

    virtual public void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Wall" || collision.transform.tag == "Ball") && thrownPlayer != collision.gameObject)
        {
            if (!isInAir)
                return;

            bounceCount--;
            if (bounceCount <= 0)
            {
                thrownPlayer = null;
                bounceCount = startingBounceCount;
                isInAir = false;
            }

        }

    }
    virtual public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && thrownPlayer == collision.gameObject.GetComponent<PlayerScript>())
            collision.gameObject.GetComponent<PlayerScript>().LeftSpace();
    }

}
