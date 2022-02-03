using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI : MonoBehaviour
{
    [SerializeField]
    private UnityEvent mScore;
    [SerializeField]
    Particle2D mBody;
    [SerializeField]
    Collider mCol;
    [SerializeField]
    AIBehavior mBehavior;
    //[SerializeField]
    //bool UsesAccel = false;
    [SerializeField]
    float BehaviorForce = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = mBehavior.getSteerDirection();
        mBody.Acceleration = (direction * BehaviorForce);
        transform.LookAt(gameObject.transform.position + new Vector3 (mBody.Velocity.x, mBody.Velocity.y, 0.0f));
        //Debug.Log(mBody.Velocity);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit: " + other.gameObject.tag);
        if(other.gameObject.tag == "WALL")
            mScore.Invoke();
    }
}
