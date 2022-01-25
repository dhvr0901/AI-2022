using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : AIBehavior
{
    [SerializeField]
    public Vector2 mTarget;
    
    public override Vector2 getSteerDirection()
    {
        base.getSteerDirection();
        Vector2 temp = new Vector2(mTarget.x - transform.position.x, mTarget.y - transform.position.y).normalized;
        return temp;
    }
}
