using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class CutSceneChange : MonoBehaviour {
	public float videoTime;
	public int nextSceneIndex;
	public int playerId;
	Player player;
	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer (playerId);
		StartCoroutine(cutSceneTimer(videoTime, nextSceneIndex));
	}

	// Update is called once per frame
	void Update () {
		if (player.GetButtonDown ("Skip")) {
			StopCoroutine ("cutSceneTimer");
			SceneManager.LoadScene (nextSceneIndex);
		}
	}

	IEnumerator cutSceneTimer(float time, int sceneIndex)
	{
		yield return new WaitForSeconds (time);
		SceneManager.LoadScene (sceneIndex);
	}
}
