using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulator : MonoBehaviour
{
    [SerializeField]
    private FlowGrid grid;

    [SerializeField]
    private GameObject wall;

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
                    //toManip.ToggleAvailable();
                    if (toManip.GetAvailable())
                    {
                        GameObject temp = Instantiate(wall);
                        temp.transform.position = toManip.transform.position;

                        grid.RefreshGrid();
                    }
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
