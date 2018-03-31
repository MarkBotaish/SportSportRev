using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollision : MonoBehaviour {

    public PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Climbable")
        {
            if (gameObject.tag == "RGrab")
                player.setRightGrab(true);

            if (gameObject.tag == "LGrab")
                player.setLeftGrab(true);

            if (gameObject.tag == "Legs" && collision.tag == "Climbable")
                player.dontMoveX();

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Climbable")
        {
            if (gameObject.tag == "RGrab")
            {
                player.setRightGrab(false);
            }
            if (gameObject.tag == "LGrab")
                player.setLeftGrab(false);

            if (gameObject.tag == "Legs" && collision.tag == "Climbable")
                player.MoveX();
        }
    }
}
