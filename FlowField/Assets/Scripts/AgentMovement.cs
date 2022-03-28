using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Rigidbody>().AddForce(direction * speed);

        transform.rotation = Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity);
    }

    public void SetDirection(Vector3 toSet)
    {
        direction = toSet;
    }
}
