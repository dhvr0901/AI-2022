using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekController : MonoBehaviour
{
    [SerializeField]
    List<Seek> mSeeks;
    [SerializeField]
    Camera mCam;
    //[SerializeField]
    //int mTimeMax = 20;
    //[SerializeField]
    //Collider buffer;
    //bool inside;

    //float mTimer = 0;

    public void Update()
    {
        /*if(mTimer <=0.0f)
        {
            mTimer = mTimeMax;
            //repeat if collider detects object
            int randX = Random.Range(-20, 20);
            int randY = Random.Range(-15, 15);
            transform.position = new Vector3(randX, randY, 0);
            mSeek.mTarget = new Vector2(randX, randY);
        }
        if(inside)
        {

            int randX = Random.Range(-20, 20);
            int randY = Random.Range(-20, 20);
            transform.position = new Vector3(randX, randY, 0);
            mSeek.mTarget = new Vector2(randX, randY);
            inside = false;
        }
        mTimer -= Time.deltaTime;*/

        gameObject.transform.position = mCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30.0f));

        foreach (Seek mSeek in mSeeks)
        {
            mSeek.mTarget = gameObject.transform.position;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            //inside = true;
            //Debug.Log("relocating");
        }
    }
}

