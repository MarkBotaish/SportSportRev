using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : StopableObject {

    public enum BallType
    {
        Laser, Magic, Explosive
    }

    [Header("General")]

    public int playerId;
    public float speed;
    public float pullSpeed;
    public float freezeTime;
    public float throwAngle;
    public float laserLength;
    public List<int> numberOfParticles;
    public Color tint;
    public BallUIScript ballUI;

    bool hasPickedUp = false;
    bool isDead = false;
    bool isStunned = false;
    bool isInitted = false;
    bool canPickup = true;

    [Header("Laser")]
    public bool useLaser = false;
    public LineRenderer lineRenderer;
    public LayerMask layers;

    private float startingFreezeTime;
    private Vector2 throwingAngle = Vector2.up;
    private Color startingColor;
    private Vector3 startingPos;
    int particleIndex = 0;

   

    Rigidbody2D rigid;
    GameManagerScript code;
    GameObject ball = null;
    ParticleSystem pSystem;
    ParticleSystem.EmissionModule emissionSystem;
    Animator anim;

    public void setManager(GameManagerScript manage) { code = manage; }

    void stopLoadingAnimation() {  anim.SetBool("Loading", false); }
    void stopShootingAnimation() {
        anim.SetBool("Shooting", false);
        stopLoadingAnimation();
    }

    // Use this for initialization
    void Start() {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        startingPos = gameObject.transform.position;
        startingColor = gameObject.GetComponent<SpriteRenderer>().color;
        isInitted = true;
        pSystem = gameObject.transform.GetChild(1).GetComponent<ParticleSystem>();
        emissionSystem = pSystem.emission;
        startingFreezeTime = freezeTime;
        anim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() { 
        if (isForzen || isPaused)
            return;

        if (playerId == 0 && !isDead)
            playerOneMovement();
        else if (playerId == 0 && isDead)
            rigid.velocity = Vector2.up * pullSpeed * speed;
        else if (playerId == 1 && !isDead)
            playerTwoMovement();
        else
            rigid.velocity = Vector2.down * pullSpeed * speed;


        if (Mathf.Abs(gameObject.transform.position.y) < 0.6)
        {
            if (playerId == 1)
                code.firstPlayerWon();
            else
                code.secondPlayerWon();
        }

        if (hasPickedUp)
            emissionSystem.rateOverTime = numberOfParticles[getParticleNumber()];

        getLaserTarget();
    }

    void getLaserTarget()
    {
        if (useLaser) 
            laser();
    }

    void laser()
    {
        RaycastHit2D hit;
        if (playerId == 0)
             hit = Physics2D.Raycast(gameObject.transform.position,Vector2.up, Mathf.Infinity, layers);
        else
            hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, Mathf.Infinity, layers);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Player")
                hit.collider.gameObject.GetComponent<PlayerScript>().hit(hit.collider.gameObject,1); 
        }

        lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
        lineRenderer.SetPosition(1, hit.point);
    }


    void playerOneMovement()
    {
        Vector2 vel = Vector2.up * pullSpeed * speedMultiplier;

        if(!hasPickedUp)
            throwingAngle = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.S))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.A))
            vel += Vector2.left;
        if (Input.GetKey(KeyCode.D))
            vel += Vector2.right;
        if (Input.GetKey(KeyCode.Q))
            vel += (Vector2.left + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.E))
            vel += (Vector2.right + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.LeftShift))
            vel += (Vector2.left + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.C))
            vel += (Vector2.right + Vector2.down).normalized;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            throwingAngle = new Vector2(Mathf.Cos(throwAngle*(Mathf.PI/180)), Mathf.Sin(throwAngle * (Mathf.PI / 180)));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            throwingAngle = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            throwingAngle = new Vector2(Mathf.Cos((180-throwAngle) * (Mathf.PI / 180)), Mathf.Sin((180 - throwAngle) * (Mathf.PI / 180)));

        if(hasPickedUp || throwingAngle == Vector2.zero)
            gameObject.transform.localRotation = (Quaternion.Euler(0, 0, throwAngle * -throwingAngle.x));

        rigid.velocity = vel * speed;
        pickup(KeyCode.R, 1.0f);
    }

    void playerTwoMovement()
    {
        if (!hasPickedUp)
            throwingAngle = Vector2.zero;

        Vector2 vel = Vector2.down * pullSpeed * speedMultiplier;
        if (Input.GetKey(KeyCode.Keypad8))
            vel += Vector2.down;
        if (Input.GetKey(KeyCode.Keypad5))
            vel += Vector2.up;
        if (Input.GetKey(KeyCode.Keypad4))
            vel += Vector2.right;
        if (Input.GetKey(KeyCode.Keypad6))
            vel += Vector2.left;
        if (Input.GetKey(KeyCode.Keypad7))
            vel += (Vector2.right + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.Keypad9))
            vel += (Vector2.left + Vector2.down).normalized;
        if (Input.GetKey(KeyCode.Keypad1))
            vel += (Vector2.right + Vector2.up).normalized;
        if (Input.GetKey(KeyCode.Keypad3))
            vel += (Vector2.left + Vector2.up).normalized;
        if (Input.GetKeyDown(KeyCode.KeypadDivide))
            throwingAngle = new Vector2(Mathf.Cos(throwAngle * (Mathf.PI / 180)), Mathf.Sin(throwAngle * (Mathf.PI / 180)));
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            throwingAngle = new Vector2(Mathf.Cos((180 - throwAngle) * (Mathf.PI / 180)), Mathf.Sin((180 - throwAngle) * (Mathf.PI / 180)));
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            throwingAngle = Vector2.zero;

        if ((hasPickedUp || throwingAngle == Vector2.zero) && (ball != null && (int)ball.GetComponent<BallScript>().getBallType() != 1))
            gameObject.transform.localRotation = (Quaternion.Euler(0, 0, throwAngle * throwingAngle.x));

        rigid.velocity = vel * speed;

        pickup(KeyCode.KeypadPlus, -1.0f);

    }


    void pickup(KeyCode key, float y)
    {
        if (ball != null && !hasPickedUp && !ball.GetComponent<BallScript>().getIsInAir() && !isDead && canPickup)
        {
            anim.SetBool("Loading", true);
            ball.GetComponent<SpriteRenderer>().enabled = false;    
            ball.GetComponent<BallScript>().playerRestart();
            ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            ball.GetComponent<BallScript>().setPickUp(true);

            hasPickedUp = true;

            ballUI.setImage((int)ball.GetComponent<BallScript>().getBallType());

            throwingAngle = new Vector2(0,1);
            pSystem.Play();

            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        }
        else if ((Input.GetKeyDown(key)) && hasPickedUp)
        {
            ballUI.setImage(-1);
            anim.SetBool("Shooting", true);
            ball.GetComponent<SpriteRenderer>().enabled = true;
            BallScript ballScript = ball.GetComponent<BallScript>();
            pSystem.Stop();
            if (playerId == 1)
                throwingAngle.y *= -1f;

            if ((int)ballScript.getBallType() == 0)
            {
                StartCoroutine(startLaser());
                ball.GetComponent<LaserBall>().setLaserTime(laserLength);
            }
            else
            {
               
                ball.layer = 13;
            }

            ballScript.setSunMulti(particleIndex);
            ballScript.throwBall(throwingAngle, this);
            ball = null;
            
        }

        //Might need to change
        if (ball == null)
            hasPickedUp = false;

        if (hasPickedUp)
            ball.transform.position = gameObject.transform.GetChild(0).transform.position;

    }

    private int getParticleNumber()
    {
        if (Mathf.Abs(gameObject.transform.position.y / GameManagerScript.code.getBallLength()) <= 0.33f)
        {
            particleIndex = 2;
            return 2;
        }
            
        if (Mathf.Abs(gameObject.transform.position.y / GameManagerScript.code.getBallLength()) <= 0.66f)
        {
            particleIndex = 1;
            return 1;
        }

        particleIndex = 0;
        return 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ball" && ball == null && !hasPickedUp)
            ball = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 

        if (collision.transform.tag == "Ball" && ball == collision.gameObject)
        {
            ball = null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ball" && collision.gameObject.GetComponent<BallScript>().getIsInAir())
        {
            if(collision.gameObject.GetComponent<BallScript>().getRecentlyThrownPlayer() != gameObject)
                hit(collision.gameObject);
        }

        if (collision.transform.tag == "BackWall")
            canPickup = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "BackWall")
            canPickup = true;
    }

    public void hit(GameObject col, int flag = 0)
    {
        if(flag == 0)
        {
            col.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            col.GetComponent<BallScript>().setIsInAir(false);
            col.GetComponent<BallScript>().setSunMulti(0);
            col.layer = 8;
            freezeTime *= col.GetComponent<BallScript>().getStunMulit();
        }
        
        isDead = true;
        gameObject.GetComponent<SpriteRenderer>().color = tint;
      
        StartCoroutine(freeze());
    }


    IEnumerator freeze()
    {
        if (!isStunned)
        {
            if (playerId == 0)
                rigid.velocity = Vector2.up * pullSpeed;
            else
                rigid.velocity = Vector2.down * pullSpeed;

            isStunned = true;
            yield return new WaitForSeconds(freezeTime);
            freezeTime = startingFreezeTime;
            isStunned = false;
            isDead = false;
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
        }
    }

    IEnumerator startLaser()
    {
        useLaser = true;
        yield return new WaitForSeconds(laserLength);
        lineRenderer.SetPosition(0, Vector2.zero);
        lineRenderer.SetPosition(1, Vector2.zero);
        useLaser = false;
    }

    public override void restart()
    {
        base.restart();
        if (isInitted)
        {
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
            rigid.velocity = Vector3.zero;
            gameObject.transform.position = startingPos;
            throwingAngle = Vector2.zero;
            pSystem.Stop();

            stopLoadingAnimation();
            stopShootingAnimation();
            
        }
    }
    public override void togglePause()
    {
        base.togglePause();

        if (isPaused)
        {
            gameObject.GetComponent<SpriteRenderer>().color = startingColor;
            rigid.velocity = Vector3.zero;
        }
    }

    public bool getAction()
    {
        if ((playerId == 0 && Input.GetKey(KeyCode.F)) || (playerId == 1 && Input.GetKey(KeyCode.Keypad0)))
            return true;
        return false;
    }

}
