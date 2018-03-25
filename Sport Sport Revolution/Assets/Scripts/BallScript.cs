using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

    bool hasBeenPickedUp = false;
    bool isInAir = false;
    Rigidbody2D rigid;

	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
        if (rigid.velocity != Vector2.zero && rigid.velocity.magnitude < 0.4)
            rigid.velocity = Vector2.zero;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            isInAir = false;
        }
           
    }

    public void setPickUp(bool tof) { hasBeenPickedUp = tof; }
    public void setIsInAir(bool tof) { isInAir = tof; }
    public bool getIsInAir() { return isInAir; }
}
