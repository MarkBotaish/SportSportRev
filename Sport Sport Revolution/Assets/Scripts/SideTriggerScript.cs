using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideTriggerScript : MonoBehaviour {

    SideManagerScript parent;

    /*
     * 1- top
     * 0- bottom 
     */
    public int side;

	// Use this for initialization
	void Start () {
        parent = gameObject.transform.parent.GetComponent<SideManagerScript>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Ball")
        {
            if (side == 0)
                parent.changeTopCount(1);
            else
                parent.changeBottomCount(1);
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            if (side == 0)
                parent.changeTopCount(-1);
            else
                parent.changeBottomCount(-1);
        }
           
    }
}
