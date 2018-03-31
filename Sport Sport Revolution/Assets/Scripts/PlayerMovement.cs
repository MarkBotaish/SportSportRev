using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour {

    public int playerId;
    public float speed, jumpSpeed, climpSpeed;
    public Transform groundCheck;
    public GameObject playerObject;
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject rightGrab;
    public GameObject leftGrab;
	public LayerMask ground;

    Vector3 climbR = Vector3.zero;
    Player player;
    Rigidbody2D rb;
    float distance;

    bool grounded = false;

    public bool canRightGrab = false;
    public bool canLeftGrab = false;

    bool hasPressedRight = false;
    bool hasPressedLeft = false;

    public bool canJump = false;

    bool stopMovingX = false;

    float prevLeft = 0;
    float deltaLeft = 0;

    float prevRight = 0;
    float deltaRight = 0;

    float rightRotation = 0;
    float leftRotation = 0;

	//Particle Effects - Andy
	public GameObject grabEffect;

    float x;

    // Use this for initialization
    void Start () {
		player = ReInput.players.GetPlayer (playerId);
        rb = playerObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckInput ();
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);

        if (rb.velocity.y < -90)
            rb.velocity = new Vector2(rb.velocity.x, -90);
	}

	void FixedUpdate()
	{
		
	}

    void CheckInput()
    {
        if (player.GetAxis("Move Horizontal") != 0.0f && !(hasPressedLeft || hasPressedRight))
            rb.velocity = new Vector2(speed * player.GetAxis("Move Horizontal"), rb.velocity.y);

        if (player.GetAxis("Move Horizontal") == 0.0f && !(hasPressedLeft || hasPressedRight))
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
         

        if (player.GetButtonDown("Jump") && grounded && !canJump)
        {
            rb.velocity = transform.up * jumpSpeed;
        }else if (player.GetButtonDown("Jump") && canJump)
        {
            
            canJump = false;
            rb.gravityScale = (30.0f);
            rb.velocity = transform.up * jumpSpeed;
            hasPressedRight = false;
            hasPressedLeft = false;
            canLeftGrab = false;
            canRightGrab = false;
    
        }

        if (player.GetButton("Right Grab") && canRightGrab)
        {
            if (!hasPressedRight)
                InitClimb(rightGrab, ref hasPressedRight);
            
            if ((climbR.y >= transform.position.y || !stopMovingX) || (climbR.y >= transform.position.y && stopMovingX))
            {
                x = climbR.x;
                if (stopMovingX)
                    x = transform.position.x;
                
                gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, climbR.y,0.0f), 5.0f * Time.deltaTime);
                
                float rightAngle = Mathf.Atan2((rightArm.transform.position.y - climbR.y), -1.0f * (rightArm.transform.position.x - climbR.x)) * 180 / Mathf.PI;
                rightArm.transform.rotation = Quaternion.Euler(0, 0, -rightAngle);
         
            }

        }
        if (player.GetButton("Left Grab") && canLeftGrab)
        {
            if (!hasPressedLeft)
                InitClimb(leftGrab,ref hasPressedLeft);
            if (climbR.y > transform.position.y || !stopMovingX)
            {
                x = climbR.x;
                if (stopMovingX)
                    x = transform.position.x;

                gameObject.transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, climbR.y, 0.0f), 5.0f * Time.deltaTime);
                
                float leftAngle = Mathf.Atan2(-1.0f * (leftArm.transform.position.y - climbR.y), (leftArm.transform.position.x - climbR.x)) * 180 / Mathf.PI;
                leftArm.transform.rotation = Quaternion.Euler(0, 0, -leftAngle);
            }
        }


        if(!player.GetButton("Left Grab"))
        {
            hasPressedLeft = false;
        }
        if (!player.GetButton("Right Grab"))
        {
            hasPressedRight = false;
        }

        if((!hasPressedLeft && !hasPressedRight))
        {
            rb.gravityScale = (30.0f);
        }

        if ((player.GetAxis("Right Joystick X") != 0.0f || player.GetAxis("Right Joystick Y") != 0.0f) && !player.GetButton("Right Grab"))
        {
            rightRotation = Mathf.Atan2(-player.GetAxis("Right Joystick Y"), player.GetAxis("Right Joystick X")) * 180 / Mathf.PI;
            deltaRight = (rightRotation + 180) - prevRight;
            prevRight = rightRotation + 180;
            rightArm.transform.rotation = Quaternion.Euler(0, 0, -rightRotation);
        }

        if ((player.GetAxis("Right Joystick X") != 0.0f || player.GetAxis("Right Joystick Y") != 0.0f) && !player.GetButton("Left Grab"))
        {
            leftRotation = Mathf.Atan2(player.GetAxis("Right Joystick Y"), -player.GetAxis("Right Joystick X")) * 180 / Mathf.PI;
            deltaLeft = (leftRotation + 180) - prevLeft;
            prevLeft = leftRotation + 180;
            leftArm.transform.rotation = Quaternion.Euler(0, 0, -leftRotation);
        }
    }

    public void setRightGrab(bool tof)
    {
        canRightGrab = tof;
    }

    public void setLeftGrab(bool tof)
    {
        canLeftGrab = tof;
    }
    public void dontMoveX()
    {
        stopMovingX = true;
    }

    public void MoveX()
    {
        stopMovingX = false;
    }

    public float getdeltaLeft()
    {
        return Mathf.Abs(deltaLeft);
    }

    public float getdeltaRight()
    {
        return Mathf.Abs(deltaRight);
    }

    //Spawns particles at location of player's hand
    private void GrabEffect(Vector2 _spawnPos)
	{
		GameObject grabParticles = Instantiate(grabEffect);
		Vector2 spawnPos = _spawnPos;
		grabParticles.transform.position = spawnPos;
		SoundManager.code.PlayGrab();
	}

    void InitClimb(GameObject temp, ref bool leftOrRight)
    {
        canJump = true;
        leftOrRight = true;
        rb.velocity = Vector3.zero;
        rb.gravityScale = (0.0f);
        climbR = temp.transform.position;
        GrabEffect(climbR);
    }

    public Vector3 getRightAngle()
    {
        float x = rightGrab.transform.position.x - rightArm.transform.position.x;
        float y = rightGrab.transform.position.y - rightArm.transform.position.y;
        return new Vector3(x, y, 0.0f).normalized;
    }

    public Vector3 getLeftAngle()
    {
        float x = leftGrab.transform.position.x - leftArm.transform.position.x;
        float y = leftGrab.transform.position.y - leftArm.transform.position.y;
        return new Vector3(x, y, 0.0f).normalized;
    }

}
