using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager code;

	private void Awake()
	{
		if (code == null)
		{
			code = this;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
}
