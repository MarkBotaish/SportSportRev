using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopableObject : MonoBehaviour
{
    protected bool isForzen = false;

    virtual public void unfreeze() { isForzen = false; }
    virtual public void restart() { isForzen = true; }
}
