using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    public float ballThrownSpeed;
    public Color startingColor; 

    protected bool hasBeenPickedUp = false;
    protected bool isInAir = false;
    protected Rigidbody2D rigid;

    virtual protected void Start(){ rigid = gameObject.GetComponent<Rigidbody2D>();}

    virtual public void setPickUp(bool tof) { hasBeenPickedUp = tof; }
    virtual public void setIsInAir(bool tof) { isInAir = tof; }
    virtual public bool getIsInAir() { return isInAir; }
    virtual public void throwBall(Vector2 vel) { rigid.velocity = vel * ballThrownSpeed; }

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

    virtual public void reset() { }
}
