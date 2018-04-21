using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManagerScript : MonoBehaviour {

    public static WallManagerScript code;

   public List<GameObject> updatingWalls;

    public GameObject middleWallP1;
    public GameObject middleWallP2;

    public GameObject leftPrefab;
    public GameObject rightPrefab;

    Vector3 wallP1Position;
    Vector3 wallP2Position;


    public float delay;
    int index = -1;
    int yPositionIndex = 0;

	// Use this for initialization
	void Start () {
        wallP1Position = middleWallP1.transform.position;
        wallP2Position = middleWallP2.transform.position;
        code = this;
    }

    public bool antiCamper()
    {

        if (updatingWalls.Count <= 0)
            return false;

        if(Mathf.Abs(updatingWalls[index].transform.position.x) > 2.75)
        {
            updatingWalls[index -3].transform.position -= new Vector3(0.05f, 0, 0);
            updatingWalls[index -2].transform.position += new Vector3(0.05f, 0, 0);

            updatingWalls[index - 1].transform.position -= new Vector3(0.05f, 0, 0);
            updatingWalls[index].transform.position += new Vector3(0.05f, 0, 0);
            return true;
        }
        else if(middleWallP1.transform.position.y <= -10.25 + ((yPositionIndex) * 1.5f))
        {
            middleWallP1.transform.position += new Vector3(0.0f, 0.05f, 0.0f);
            middleWallP2.transform.position -= new Vector3(0.0f, 0.05f, 0.0f);
            return true;
        }
        
        return false;
       
    }

    public void spawnWalls()
    {
        updatingWalls.Add( Instantiate(rightPrefab, new Vector2(7.75f, -7.5f + ((yPositionIndex) * 1.5f)), Quaternion.identity));
        updatingWalls.Add(Instantiate(leftPrefab, new Vector2(-7.75f, -7.5f + ((yPositionIndex) * 1.5f)), Quaternion.identity));

        updatingWalls.Add(Instantiate(leftPrefab, new Vector2(7.75f, 7.5f - ((yPositionIndex) * 1.5f)), Quaternion.identity));
        updatingWalls[updatingWalls.Count - 1].transform.rotation = Quaternion.Euler(0, 0, 180);
        updatingWalls.Add(Instantiate(rightPrefab, new Vector2(-7.75f, 7.5f - ((yPositionIndex) * 1.5f)), Quaternion.identity));
        updatingWalls[updatingWalls.Count - 1].transform.rotation = Quaternion.Euler(0, 0, 180);
        index += 4;
        yPositionIndex++;
    }

    public void resetWalls()
    {
        for(int i = 0; i < updatingWalls.Count; i++)
            Destroy( updatingWalls[i]);

        updatingWalls.Clear();
        index = -1;
        yPositionIndex = 0;

        middleWallP1.transform.position = wallP1Position;
        middleWallP2.transform.position = wallP2Position;
    }
}
