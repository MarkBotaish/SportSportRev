using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabbyCaveThings : MonoBehaviour {

	public GameObject bloodEffect;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "PlayerHead" || collision.gameObject.tag == "Player")
		{
			Blood(collision.gameObject);
		}
	}

	void Blood(GameObject col)
	{
		GameObject blood = Instantiate(bloodEffect);
		Vector2 spawnPos = col.transform.position;
		spawnPos += new Vector2(0, 1.5f);
		blood.transform.position = spawnPos;
		blood.transform.parent = col.transform;
		SoundManager.code.Hurt();
	}

}
