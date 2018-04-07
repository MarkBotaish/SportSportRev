using System.Collections;
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

    override public void throwBall(Vector2 vel) {
        rigid.velocity = vel * (ballThrownSpeed + Random.Range(-4,10));
        transform.localScale *=  4.0f / Random.Range(1, 8);
    }

    override public void reset()
    {
        rigid.velocity = Vector2.zero;
        transform.localScale = originalScale;
    }

}


