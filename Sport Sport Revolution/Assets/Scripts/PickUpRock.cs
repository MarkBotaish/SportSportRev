using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PickUpRock : MonoBehaviour {

	public int playerId;
	private Player player;
    Transform Larm;
    Transform Rarm;
	Rigidbody2D rb;
    float velocity = 0;

    bool isHolding = false;
    bool right = false;
    bool left = false;

	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer (playerId);
		rb = GetComponent<Rigidbody2D> ();
		Larm = null;
        Rarm = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetButtonDown("Right Object") && Rarm != null && !isHolding){
            GetComponent<BoxCollider2D>().isTrigger = true;
            if (playerId == 0)
                gameObject.layer = 10;
            else
                gameObject.layer = 11;
            transform.SetParent(Rarm);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            isHolding = true;

            right = true;
            left = false;
        }
        if (player.GetButtonDown("Left Object") && Larm != null && !isHolding)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            isHolding = true;
            transform.SetParent(Larm);
            left = true;
            right = false;
        }
      
        if (isHolding && player.GetButtonUp("Right Object") && right) {
            resetCube();
		}else if (isHolding && player.GetButtonUp("Left Object") && left) {
            resetCube();
		}

        if (player.GetButton("Left Grab") || player.GetButton("Right Grab"))
            resetCube();
        if (isHolding)
        {
            if (right)
            {
                gameObject.transform.position = Rarm.position;
                velocity = transform.root.gameObject.GetComponent<PlayerMovement>().getdeltaRight();
            }
            else
            {
                gameObject.transform.position = Larm.position;
                velocity = transform.root.gameObject.GetComponent<PlayerMovement>().getdeltaLeft();
            }
                
        }

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "RGrab" && !isHolding)
            Rarm = col.transform;
        
        if (col.gameObject.tag == "LGrab" && !isHolding)
            Larm = col.transform;
        
    }
	void OnTriggerExit2D(Collider2D col)
	{
        if (col.gameObject.tag == "RGrab" && !isHolding)
            Rarm = null;
        
        if (col.gameObject.tag == "LGrab" && !isHolding)
            Larm = null;

        if (Larm == null && Rarm == null)
            isHolding = false;
        if (col.gameObject.tag == "Legs" && Larm == null && Rarm == null)
            GetComponent<BoxCollider2D>().isTrigger = false;

    }

    public void resetCube()
    {

        gameObject.layer = 15;
        Vector3 direction = Vector3.zero ;
        if (left)
            direction = transform.root.GetComponent<PlayerMovement>().getLeftAngle();
        else if (right)
            direction = transform.root.GetComponent<PlayerMovement>().getRightAngle();

        if (Rarm != null)
            Rarm.DetachChildren();

        if (Larm != null)
            Larm.DetachChildren();
        GetComponent<BoxCollider2D>().isTrigger = false;
        rb.constraints = RigidbodyConstraints2D.None;
       

        rb.velocity = direction * (velocity / 180) * 200;
        if (rb.velocity.magnitude > 15)
            rb.velocity = rb.velocity.normalized * 15;

        isHolding = false;

        if (right)
            right = false;
        else
            left = false;
    }
}
