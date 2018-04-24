using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBall : BallScript {

    PlayerScript hitPlayer;

    protected override void doAction()
    {
        base.doAction();
        isInAir = false;
        recentlyThrownPlayer = null;
        activatePlayer = null;
        if (hitPlayer != null)
            hitPlayer.hit(gameObject);
        StartCoroutine(childEnable());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player One" || collision.gameObject.tag == "Player Two")
            hitPlayer = collision.gameObject.GetComponent<PlayerScript>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player One" || collision.gameObject.tag == "Player Two")
            hitPlayer = null;
    }

    IEnumerator childEnable()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
