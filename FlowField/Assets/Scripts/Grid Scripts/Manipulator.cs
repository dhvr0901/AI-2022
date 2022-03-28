using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour
{
    [SerializeField]
    private FlowGrid grid;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //toggle node   
                if (hit.transform.gameObject.tag == "Node")
                {
                    Node toManip = hit.transform.gameObject.GetComponent<Node>();
                    toManip.ToggleAvailable();
                    grid.RefreshGrid();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
    }
}
