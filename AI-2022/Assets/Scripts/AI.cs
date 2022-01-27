using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField]
    Particle2D mBody;
    [SerializeField]
    AIBehavior mBehavior;
    [SerializeField]
    bool UsesAccel = false;
    [SerializeField]
    float MoveMax = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mBody.Velocity = mBehavior.getSteerDirection() * MoveMax;
        transform.LookAt(mBody.Velocity);
        //Debug.Log(mBody.Velocity);
    }
}
