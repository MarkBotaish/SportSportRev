using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDodgeBall : BallScript {

    // Use this for initialization
   

    // Update is called once per frame
    void Update()
    {
        checkThrownBall();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Ball")
        {
            isInAir = false;
        }

    }
}
