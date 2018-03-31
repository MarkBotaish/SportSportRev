using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlood : MonoBehaviour
{
	private ParticleSystem particle;
	
	void Start()
	{
		particle = GetComponent<ParticleSystem>();
		Destroy(this.gameObject, particle.main.duration);
	}
}
