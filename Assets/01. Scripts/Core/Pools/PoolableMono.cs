using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public abstract void ResetPoolableMono();

    public abstract void EnablePoolableMono();
}