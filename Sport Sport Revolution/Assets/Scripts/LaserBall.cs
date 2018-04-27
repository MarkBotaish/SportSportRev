using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBall : BallScript {

    float laserTime = 0;

    public void setLaserTime(float time) { laserTime = time; }

    public override void throwBall(Vector2 vel, PlayerScript obj)
    {
        hasBeenPickedUp = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(waitToRespawn());
    }

    IEnumerator waitToRespawn()
    {
        yield return new WaitForSeconds(laserTime);
       
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;

        respawn();
    }

    public override void restart()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        base.restart();
    }


}
