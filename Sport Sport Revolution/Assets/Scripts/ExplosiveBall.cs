using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBall : BallScript {

    PlayerScript hitPlayer;

    protected override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void doAction()
    {
        base.doAction();
        isInAir = false;
        print("BOOM");
        if (hitPlayer != null)
            hitPlayer.hit(gameObject);
        StartCoroutine(childEnable());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            hitPlayer = collision.gameObject.GetComponent<PlayerScript>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            hitPlayer = null;
    }

    IEnumerator childEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
