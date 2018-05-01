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

        int rand = -1;

        if (numOfBallsOnP1 >= numOfBallsOnP2)
        {
            rand = 1;
            if(numOfBallsOnP1 > 0)
                numOfBallsOnP1--;
            numOfBallsOnP2++;

        }
        else if (numOfBallsOnP1 < numOfBallsOnP2)
        {
            rand = 0;
            if (numOfBallsOnP2 > 0)
                numOfBallsOnP2--;
            numOfBallsOnP1++;
        }

        return rand;
    }
}
