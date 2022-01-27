using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : AIBehavior
{
    [SerializeField]
    public AIBehavior mBehavior;
    [SerializeField]
    float mRayDistance;
    [SerializeField]
    float mFlankRayDistance;
    [SerializeField]
    float mBodyWidth;

    public override Vector2 getSteerDirection()
    {
        base.getSteerDirection();
        Vector2 direction = mBehavior.getSteerDirection();
        if (Physics.Raycast(gameObject.transform.position,
            gameObject.transform.position + new Vector3(direction.x, direction.y, 0.0f),
            mRayDistance))
        {
            bool leftFlank = Physics.Raycast(gameObject.transform.position - (new Vector3(direction.y, -direction.x, 0.0f) * mBodyWidth * 2),
            gameObject.transform.position + new Vector3(direction.x, direction.y, 0.0f),
            mFlankRayDistance);
            bool rightFlank = Physics.Raycast(gameObject.transform.position + (new Vector3(direction.y, -direction.x, 0.0f) * mBodyWidth * 2),
            gameObject.transform.position + new Vector3(direction.x, direction.y, 0.0f),
            mFlankRayDistance);
            //if flank triggered
            if (leftFlank || rightFlank)
            {
                //determine rotation direction
                bool moveRight = false;
                if (leftFlank)
                {
                    if (rightFlank)
                    {
                        if (Random.Range(0, 1) == 0)
                        {
                            moveRight = true;
                        }
                    }
                    else
                    {
                        moveRight = true;
                    }
                }

                //apply avoidance
                if(moveRight)
                {
                    direction += new Vector2(direction.y, -direction.x);

                    Debug.Log("redirect right triggered");
                }
                else
                {
                    direction += new Vector2(-direction.y, direction.x);

                    Debug.Log("redirect left triggered");
                }
            }
        }
        return direction.normalized;
    }
}
