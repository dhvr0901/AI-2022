using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : AIBehavior
{
    [SerializeField]
    public AIBehavior mBehavior;
    [SerializeField]
    private Particle2D mHost;
    [SerializeField]
    float mAvoidPower;
    [SerializeField]
    float mStrafePower;
    [SerializeField]
    float mRayDistance = 0.0f;
    [SerializeField]
    float mFlankAngle;

    Quaternion LeftAngle, RightAngle;
    private Vector3 Origin, DirectionMid, DirectionLeft, DirectionRight;
    private Vector2 Vel;
    private RaycastHit Hit, HitLeft, HitRight;
    private bool Mid, Left, Right;

    //update raycasts
    private void FixedUpdate()
    {
        Mid = Physics.Raycast(Origin, DirectionMid, out Hit, mRayDistance);
        Left = Physics.Raycast(Origin, DirectionLeft, out HitLeft, mRayDistance);
        Right = Physics.Raycast(Origin, DirectionRight, out HitRight, mRayDistance);
    }

    //-1 for left, 1 for right
    float mLastAvoid = 0;

    public void Start()
    {
        mHost = gameObject.GetComponent<Particle2D>();
        LeftAngle = Quaternion.Euler(0, 0, mFlankAngle);
        RightAngle = Quaternion.Euler(0, 0, -mFlankAngle);
        UpdateValues();
    }

    public override Vector2 getSteerDirection()
    {
        base.getSteerDirection();
        Vector2 toGo = mBehavior.getSteerDirection();
        UpdateValues();

        //draw Middle Ray
        if (Mid)
        {
            Debug.DrawLine(Origin, Origin + DirectionMid.normalized, Color.black, Time.deltaTime, false);
        }
        else
        {
            Debug.DrawLine(Origin, Origin + DirectionMid, Color.white, Time.deltaTime, false);
        }


        //draw Left Ray
        if (Left)
        {
            Debug.DrawLine(Origin, Origin + DirectionLeft/*.normalized * HitLeft.distance*/, Color.green, Time.deltaTime, false);
        }
        else
        {
            Debug.DrawLine(Origin, Origin + DirectionLeft, Color.blue, Time.deltaTime, false);
        }

        //draw Right Ray
        if (Right)
        {
            Debug.DrawLine(Origin, Origin + DirectionRight/*.normalized * HitRight.distance*/, Color.yellow, Time.deltaTime, false);
        }
        else
        {
            Debug.DrawLine(Origin, Origin + DirectionRight, Color.red, Time.deltaTime, false);
        }

        //calculate avoidance
        if (Left || Right)
        {
            //determine rotation direction
            bool moveRight = false;
            if (Left)
            {
                if (Right)
                {
                    if(mLastAvoid != 0)
                    {
                        if(mLastAvoid > 0)
                        {
                            moveRight = true;
                        }
                    }
                    else if (Random.Range(0, 1) == 0)
                    {
                        moveRight = true;
                        mLastAvoid = 1;
                    }
                }
                else
                {
                    moveRight = true;
                    mLastAvoid = 1;
                }
            }
            else
            {
                mLastAvoid = -1;
            }

            //apply avoidance
            if(moveRight)
            {
                //toGo = new Vector2(toGo.y, -toGo.x) * mStrafePower - toGo * mAvoidPower;

                toGo = new Vector2(toGo.y, -toGo.x) /* (2 / HitLeft.distance)*/ * mStrafePower - toGo * HitLeft.distance * mAvoidPower;
            }
            else
            {
                //toGo = new Vector2(toGo.y, -toGo.x) * -mStrafePower - toGo * mAvoidPower;

                toGo = new Vector2(toGo.y, -toGo.x) /* (2 / HitRight.distance)*/ * -mStrafePower - toGo * HitRight.distance * mAvoidPower;
            }
        }
        else
        {
            mLastAvoid = 0;
        }
        return toGo.normalized;
    }

    private void UpdateValues()
    {
        Vel = mHost.Velocity;
        Origin = gameObject.transform.position;
        DirectionMid = new Vector3(Vel.x, Vel.y, 0.0f).normalized * mRayDistance;
        DirectionLeft = (LeftAngle * new Vector3(Vel.x, Vel.y, 0.0f)).normalized * mRayDistance;
        DirectionRight = (RightAngle * new Vector3(Vel.x, Vel.y, 0.0f)).normalized * mRayDistance;
    }
}
