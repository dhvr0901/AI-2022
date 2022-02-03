using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipsoidObstacle : MonoBehaviour
{
    [SerializeField]
    float magnitude;
    [SerializeField]
    Rigidbody body;
    Vector3 origin;
    float extendedTime;
    private void Start()
    {
        origin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        extendedTime += Time.deltaTime;
        extendedTime %= (2 * Mathf.PI);
        body.AddRelativeForce(Mathf.Sin(extendedTime) * magnitude, Mathf.Cos(extendedTime) * magnitude, 0);
    }
}
