using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveThreshold; //distance from the edge of the screen when movement starts
    [SerializeField]
    private float uiBuffer;
    [SerializeField]
    private float maxVel;
    [SerializeField]
    private float scrollFactor;
    [SerializeField]
    private Vector2 zoomLimits;
    [SerializeField]
    private Vector2 zoomDelay;
    private Vector2 timer;


    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;

        Vector2 crossover = new Vector2();
        //x and y set to the percentage of the border the mouse is pushed into, otherwise 0
                    
        crossover.x =
                    //if exceeding threshold, set to value exceeding by (clamped to threshold)
            screenPoint.x + moveThreshold.x > Screen.width ? 
            Mathf.Clamp(screenPoint.x + moveThreshold.x - Screen.width, 0, moveThreshold.x) : 
                    //if below threshold, set to that amount (clamped to -threshold)
            screenPoint.x - moveThreshold.x < 0 ? 
            Mathf.Clamp(screenPoint.x - moveThreshold.x, -moveThreshold.x, 0) : 0;
        //convert to percentage of threshold
        crossover.x = crossover.x / moveThreshold.x;


        crossover.y =
                    //if exceeding threshold, set to value exceeding by (clamped to threshold)
            screenPoint.y + moveThreshold.y > Screen.height ? 
            Mathf.Clamp(screenPoint.y + moveThreshold.y - Screen.height, 0, moveThreshold.y) :
                    //if below threshold, set to that amount (clamped to -threshold)
            screenPoint.y - moveThreshold.y < /*uiBuffer && screenPoint.y - moveThreshold.y >*/ 0 ? 
            Mathf.Clamp(screenPoint.y - moveThreshold.y/* + uiBuffer*/, -moveThreshold.y, 0) : 0;
        //convert to percentage of threshold
        crossover.y = crossover.y / moveThreshold.y;

        gameObject.transform.position = new Vector3(Mathf.Clamp(transform.position.x + (maxVel * Time.deltaTime * crossover.x), 10, 290), transform.position.y, transform.position.z);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z + (maxVel * Time.deltaTime * crossover.y), 10, 290));

        //zoom based on scroll
        float scroll = Input.mouseScrollDelta.y; 
        if(scroll != 0)
        {
            //round scroll to the nearest interger furthest from 0 (absolute value rounded up times sign of scroll)
            scroll = -Mathf.Ceil(Mathf.Abs(scroll)) * (scroll / Mathf.Abs(scroll));
            gameObject.transform.position = new Vector3(transform.position.x, 
                Mathf.Clamp(transform.position.y + (maxVel * scrollFactor * Time.deltaTime * scroll), 
                zoomLimits.x, zoomLimits.y), transform.position.z);
        }
    }
}
