using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Laser, Magic, Explosive
}


public class BallScript : StopableObject {

    public float ballThrownSpeed;
    public float fallSpeed;
    protected float spawnPosition;
    public BallType type;
    protected bool isInitted = false;
    protected float stunMultiplier = 1.0f;


    protected PlayerScript activatePlayer;
    protected GameObject recentlyThrownPlayer;

    protected bool hasBeenPickedUp = false;
    protected bool isInAir = false;
    protected Rigidbody2D rigid;
    public int bounceCount;
    protected int startingBounceCount;
    protected float startingSpawnPosition;
    protected Vector2 currentVelocity;

    virtual public void moveSpawnPositionIn(float offset) { spawnPosition -= offset; }
    virtual public void setPickUp(bool tof) { hasBeenPickedUp = tof; }
    virtual public bool getPickUp() { return hasBeenPickedUp; }
    virtual public void setIsInAir(bool tof) { isInAir = tof; }
    virtual public bool getIsInAir() { return isInAir; }
   
    public PlayerScript getActivatePlayer() { return activatePlayer; }
    public GameObject getRecentlyThrownPlayer() { return recentlyThrownPlayer; }
    public float getStunMulit() { return stunMultiplier; }
    public BallType getBallType() { return type; }

    virtual public void throwBall(Vector2 vel, PlayerScript obj) {
        hasBeenPickedUp = false;
        isInAir = true;
        rigid.velocity = vel * ballThrownSpeed;
        activatePlayer = obj;
        recentlyThrownPlayer = obj.gameObject;
    }

    public void setSunMulti(int num)
    {
        if (num == 1)
            stunMultiplier = 1.25f;
        else if (num == 2)
            stunMultiplier = 1.75f;
        else stunMultiplier = 1.0f;

    }


    public void setActivatePlayer(PlayerScript obj)
    {
        activatePlayer = obj;
        recentlyThrownPlayer = obj.gameObject;
    }

    virtual protected void doAction()
    {
        recentlyThrownPlayer = null;
        activatePlayer = null;
    }

    virtual protected void Start() {
        spawnPosition = 8.0f;
        startingSpawnPosition = spawnPosition;
        startingBounceCount = bounceCount;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        isInitted = true;
    }

    public virtual void Update()
    {
        if (isForzen || isPaused)
            return;

        currentVelocity = rigid.velocity;

        checkThrownBall();
        if (Mathf.Abs(transform.position.y) < 0.5 && !isInAir && !hasBeenPickedUp)
            respawn();

        if (Mathf.Abs(rigid.velocity.y) < fallSpeed* speedMultiplier && !isInAir && !hasBeenPickedUp)
        {
            if (transform.position.y > 0)
                rigid.velocity = new Vector2(rigid.velocity.x, -fallSpeed * speedMultiplier);
            else
                rigid.velocity = new Vector2(rigid.velocity.x, fallSpeed * speedMultiplier);
        }

        if (activatePlayer != null && activatePlayer.getAction())
            doAction();
    }

    protected virtual void respawn()
    {
        int rand = 0;
        if(SideManagerScript.code != null)
            rand = SideManagerScript.code.getSpawnSide();
        float xPosRand = Random.Range(-4.0f, 4.0f);
        if (rigid != null)
            rigid.velocity = Vector2.zero;

        if (rand == 0)
        {
            transform.position = new Vector3(xPosRand, -spawnPosition, 0);
        }
        else
        {
            transform.position = new Vector3(xPosRand, spawnPosition, 0);
        }

        if(Mathf.Abs(transform.position.x) > 4)
        {
            if (rand == 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
            }
        }

        bounceCount = startingBounceCount;
    }



    virtual protected void checkThrownBall()
    {
        if (rigid.velocity != Vector2.zero && rigid.velocity.magnitude < 0.4)
        {
            rigid.velocity = Vector2.zero;
        }
    }

    override public void restart() {
        base.restart();
        isInAir = false;
        hasBeenPickedUp = false;
        respawn();
        spawnPosition = startingSpawnPosition;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        if (isInitted)
            playerRestart();

    }

    public override void togglePause()
    {
        base.togglePause();

        if (isPaused)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            rigid.velocity = currentVelocity;
        }
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
        if ((collision.transform.tag == "Wall" || collision.transform.tag == "Ball" || collision.transform.tag == "BackWall") && activatePlayer != collision.gameObject && isInAir)
        {
            bounceCount--;
            if (bounceCount <= 1 && recentlyThrownPlayer != null)
                recentlyThrownPlayer = null;

            if (!isInAir)
                return;

            
            if (bounceCount <= 0)
            {
                activatePlayer = null;
                bounceCount = startingBounceCount;
                isInAir = false;
                gameObject.layer = 8;
                stunMultiplier = 1.0f;
            }

        }

    }

}
