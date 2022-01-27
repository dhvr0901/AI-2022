using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekController : MonoBehaviour
{
    [SerializeField]
    Seek mSeek;
    [SerializeField]
    int mTimeMax = 20;
    float mTimer = 0;

    public void Update()
    {
        if(mTimer <=0.0f)
        {
            mTimer = mTimeMax;
            int randX = Random.Range(-50, 50);
            int randY = Random.Range(-30, 30);
            mSeek.mTarget = new Vector2(randX, randY);
        }
        mTimer -= Time.deltaTime;
    }
}
