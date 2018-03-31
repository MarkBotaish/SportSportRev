using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QATesting : MonoBehaviour {

    public GameObject playerOne;
    public GameObject playerTwo;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerOne.transform.position = new Vector3(93,-4,0);
            playerTwo.transform.position = new Vector3(98, -4, 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerOne.transform.position = new Vector3(170, -11, 0);
            playerTwo.transform.position = new Vector3(175, -11, 0);
        }

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			playerOne.transform.position = new Vector3(143, 15, 0);
			playerTwo.transform.position = new Vector3(148, 15, 0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			playerOne.transform.position = new Vector3(142, 29, 0);
			playerTwo.transform.position = new Vector3(147, 29, 0);
		}
    }
}
