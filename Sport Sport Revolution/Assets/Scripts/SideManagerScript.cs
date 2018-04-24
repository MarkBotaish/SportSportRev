using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideManagerScript : MonoBehaviour {


    public static SideManagerScript code;

    int numOfBallsOnP1 = 0;
    int numOfBallsOnP2 = 0;

    /*
     * 1 - top
     * 0 - bottom 
   */

    private void Start()
    {
        code = this;
    }
    public int getSpawnSide()
    {
        int rand = Random.Range(0, 2);

        if (numOfBallsOnP1 > numOfBallsOnP2)
        {
            rand = 1;
        }
        else if (numOfBallsOnP1 < numOfBallsOnP2)
        {
            rand = 0;
        }
        return rand;
    }

    public void changeTopCount(int num) { numOfBallsOnP1+= num; }
    public void changeBottomCount(int num) { numOfBallsOnP2 += num; }
}
