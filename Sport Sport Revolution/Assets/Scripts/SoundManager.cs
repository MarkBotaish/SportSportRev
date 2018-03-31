using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager code;

	private AudioSource source;
	public AudioClip[] sound;

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

	void Start()
	{
		source = GetComponent<AudioSource>();
	}

	public void PlayGrab()
	{
		source.clip = sound[0];
		source.Play();
	}

	public void Hurt()
	{
		source.clip = sound[1];
		source.Play();
	}
}
