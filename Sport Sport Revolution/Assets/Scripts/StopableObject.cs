using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopableObject : MonoBehaviour
{
    protected bool isForzen = false;
    protected bool isPaused = false;

    virtual public void unfreeze() { isForzen = false; }
    virtual public void togglePause() { isPaused = !isPaused; }
    virtual public void restart() { isForzen = true; }
}
