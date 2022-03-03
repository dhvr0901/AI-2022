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
    float mRayDistance = 0.0f;
    [SerializeField]
    float mStallThresh;
    [SerializeField]
    float mStallMax;
    private float mStallTime = 0.0f;
    bool mStall = false;
    private Vector3 lastLocation;
    private Vector3 lastClosest;
    private Vector3 xtraPush = new Vector3();
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
        if (!Left)
        {
            Left = Physics.Raycast(Origin, new Vector2(-DirectionMid.y, DirectionMid.x), out HitLeft, mRayDistance);
        }
        Right = Physics.Raycast(Origin, DirectionRight, out HitRight, mRayDistance);
        if (!Right)
        {
            Right = Physics.Raycast(Origin, new Vector2(DirectionMid.y, -DirectionMid.x), out HitRight, mRayDistance);
        }

        Vector3 temp = transform.position - lastLocation;

        if (temp.magnitude < mStallThresh * Time.deltaTime)
        {
            mStallTime += Time.fixedDeltaTime;
        }
        if (mStallTime > mStallMax)
        {
            mStall = true;
        }
        else if (mStallTime < 0)
        {
            mStall = false;
        }
        if (mStall)
        {
            //fix stalling
            if (Mid)
            { lastClosest = Hit.point; }
            if (Left)
            {
                lastClosest = (lastClosest - transform.position).magnitude > (HitLeft.point - transform.position).magnitude
                      ? HitLeft.point : lastClosest;
            }
            if (Right)
            {
                lastClosest = (lastClosest - transform.position).magnitude > (HitRight.point - transform.position).magnitude
                      ? HitRight.point : lastClosest;
            }

            xtraPush = transform.position - lastClosest;
            mStallTime -= Time.fixedDeltaTime;
        }
        else
        {
            lastClosest = new Vector3();
            xtraPush = new Vector3();
        }

    }

    public void Start()
    {
        lastLocation = transform.position;

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
            Debug.DrawRay(Origin, DirectionLeft/*.normalized * HitLeft.distance*/, Color.green, Time.deltaTime, false);
            Debug.DrawRay(Origin, new Vector2(-DirectionMid.y, DirectionMid.x), Color.black, Time.deltaTime, false);
        }
        else
        {
            Debug.DrawRay(Origin, DirectionLeft, Color.blue, Time.deltaTime, false);
            Debug.DrawRay(Origin, new Vector2(-DirectionMid.y, DirectionMid.x), Color.white, Time.deltaTime, false);
        }

        //draw Right Ray
        if (Right)
        {
            Debug.DrawRay(Origin, DirectionRight/*.normalized * HitRight.distance*/, Color.yellow, Time.deltaTime, false);
            Debug.DrawRay(Origin, new Vector2(DirectionMid.y, -DirectionMid.x), Color.black, Time.deltaTime, false);
        }
        else
        {
            Debug.DrawRay(Origin, DirectionRight, Color.red, Time.deltaTime, false);
            Debug.DrawRay(Origin, new Vector2(DirectionMid.y, -DirectionMid.x), Color.white, Time.deltaTime, false);
        }

        Vector2 tempDir = toGo;
        //calculate avoidance
        if (Left || Right || Mid)
        {
            float tempMagnitude = toGo.magnitude;
            if (Left)
            {
                toGo += tempMagnitude * new Vector2(HitLeft.normal.x, HitLeft.normal.y);
            }
            if (Right)
            {
                toGo += tempMagnitude * new Vector2(HitRight.normal.x, HitRight.normal.y);
            }
            if (Mid)
            {
                toGo += tempMagnitude * new Vector2(Hit.normal.x, Hit.normal.y);
            }
        }
        toGo += new Vector2(xtraPush.x, xtraPush.y);
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
