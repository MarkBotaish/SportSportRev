using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    public float ballThrownSpeed;
    public float fallSpeed;
    public Color startingColor;

    protected GameObject thrownPlayer;

    protected bool hasBeenPickedUp = false;
    protected bool isInAir = false;
    protected Rigidbody2D rigid;

    virtual protected void Start() { rigid = gameObject.GetComponent<Rigidbody2D>(); }

    public virtual void Update()
    {
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

    }

    protected void respawn()
    {
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            transform.position = new Vector3(transform.position.x, 8.0f, 0);
            rigid.velocity = Vector3.down * fallSpeed;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, -8.0f, 0);
            rigid.velocity = Vector3.up * fallSpeed;
        }
    }

    virtual public void setPickUp(bool tof) { hasBeenPickedUp = tof; }
    virtual public void setIsInAir(bool tof) { isInAir = tof; }
    virtual public bool getIsInAir() { return isInAir; }
    virtual public void throwBall(Vector2 vel) { rigid.velocity = vel * ballThrownSpeed; }
    public void setThrownPlayer(GameObject obj  ) { thrownPlayer = obj; }
    public GameObject getThrownPlayer() { return thrownPlayer; }

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

    virtual public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall" || collision.transform.tag == "Ball" && thrownPlayer != collision.gameObject)
        {
            isInAir = false;
        }

    }
}
