﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : BallScript {
    Vector3 originalScale;

    // Update is called once per frame
    protected override void Start()
    {
        base.Start();
        originalScale = transform.localScale;
    }

    override public void throwBall(Vector2 vel, PlayerScript obj) {
        base.throwBall(vel, obj);
        rigid.velocity = vel * (ballThrownSpeed + Random.Range(-4,10));

        float rand = Random.Range(0, 2);

        if(rand == 0)
            transform.localScale *= Random.Range(0.5f, 1.0f);
        else
            transform.localScale *= Random.Range(2.0f, 5.0f);

    }

    override public void restart()
    {
        base.restart();
        if (isInitted)
        {
            rigid.velocity = Vector2.zero;
            transform.localScale = originalScale;
        }
    }

    override public void playerRestart()
    {
        rigid.velocity = Vector2.zero;
        transform.localScale = originalScale;
    }

}


