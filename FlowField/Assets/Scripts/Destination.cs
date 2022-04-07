using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    [SerializeField]
    private FlowGrid grid;

    IEnumerator DelayedAsk()
    {
        yield return new WaitForSeconds(0.5f);

        Ray ray = new Ray(transform.position, new Vector3(0, -1, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //toggle node   
            if (hit.transform.gameObject.tag == "Node")
            {
                Node toManip = hit.transform.gameObject.GetComponent<Node>();
                grid.NewFlow(toManip);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedAsk());
    }
}
