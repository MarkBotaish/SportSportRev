using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(blink());
    }
    IEnumerator blink()
    {
        gameObject.GetComponent<Text>().enabled = !gameObject.GetComponent<Text>().enabled;
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(blink());
    }
}
