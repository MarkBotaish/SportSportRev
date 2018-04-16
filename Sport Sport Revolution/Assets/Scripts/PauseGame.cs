using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    public GameObject panel1;
    public GameObject panel2;

    // Update is called once per frame
    void Update () {
        checkPause();
	}

    void checkPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel1.SetActive(!panel1.activeSelf);
            panel2.SetActive(!panel2.activeSelf);
            GameManagerScript.code.togglePause();
        }
            
    }
}
