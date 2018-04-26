using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {

    public int playerId;

	// Use this for initialization
	void Start () {
        StartCoroutine(blink());
	}
	
	// Update is called once per frame
	void Update () {
        if (playerId == 0 && Input.GetKey(KeyCode.F))
            gameObject.SetActive(false);

        if (playerId == 1 && Input.GetKey(KeyCode.Keypad0))
            gameObject.SetActive(false);
    }

    IEnumerator blink()
    {
        gameObject.GetComponent<Text>().enabled = !gameObject.GetComponent<Text>().enabled;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(blink());
    }
}
