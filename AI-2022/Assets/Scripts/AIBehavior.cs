using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    //returns the normalized direction that the AI should move in
    public virtual Vector2 getSteerDirection()
    {
        return new Vector2(0.0f, 0.0f);
    }
}
