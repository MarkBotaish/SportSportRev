using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallUIScript : MonoBehaviour {

    public List<Sprite> balls;
    Image image; 

	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
	}
	
	public void setImage(int index)
    {
        if (image == null)
            return;

        if (index < 0 || index >= balls.Count)
        {
            image.sprite = null;
            image.enabled = false;

        }   
        else{
            image.enabled = true;
            image.sprite = balls[index];
        }
          
    }
}
