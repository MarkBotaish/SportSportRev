using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : BallScript {
    PlayerScript hitPlayer;
    ExplosiveBall ballToSpawn;

    public void setExplosive(ExplosiveBall ball) { ballToSpawn = ball; }

    override protected void Start()
    {
        if (isInitted)
            return;
        spawnPosition = 8.0f;
        startingSpawnPosition = spawnPosition;
        startingBounceCount = bounceCount;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        isInitted = true;
    }

    protected override void doAction()
    {
        base.doAction();
        recentlyThrownPlayer = null;
        activatePlayer = null;
        if (hitPlayer != null)
            hitPlayer.hit(gameObject);
        StartCoroutine(childEnable());

    }

    public override void Update()
    {
        base.Update();
        float rotation = 0;
        rotation += 360 - Mathf.Acos(rigid.velocity.normalized.y) * (180 / Mathf.PI);
        if (rigid.velocity.normalized.x < 0)
            rotation *= -1;

        gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
    }

    override public void throwBall(Vector2 vel, PlayerScript obj)
    {
        if(!isInitted)
            Start();

        hasBeenPickedUp = false;
        isInAir = true;
        rigid.velocity = vel * ballThrownSpeed;
        activatePlayer = obj;
        recentlyThrownPlayer = obj.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player One" || collision.gameObject.tag == "Player Two")
            hitPlayer = collision.gameObject.GetComponent<PlayerScript>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player One" || collision.gameObject.tag == "Player Two")
            hitPlayer = null;
    }

    IEnumerator childEnable()
    {
        
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        ballToSpawn.spawnFromRocket();
        
        Destroy(gameObject);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.transform.tag == "Wall" || collision.transform.tag == "Ball" || collision.transform.tag == "BackWall") && activatePlayer != collision.gameObject && isInAir)
        {
            if (recentlyThrownPlayer != null)
                recentlyThrownPlayer = null;

            if (!isInAir)
                return;

            bounceCount--;
            if (bounceCount <= 0)
            {
                activatePlayer = null;
                gameObject.layer = 8;
                stunMultiplier = 1.0f;
                doAction();
            }

        }
        if (collision.transform.tag == "Player" && recentlyThrownPlayer != collision.gameObject)
            doAction();

    }

    protected override void checkThrownBall()
    {
        if (rigid.velocity != Vector2.zero && rigid.velocity.magnitude < 0.4)
        {
            rigid.velocity = Vector2.zero;
        }
    }
}
