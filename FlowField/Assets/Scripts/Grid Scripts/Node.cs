using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Vector3 flowDirection;

    [SerializeField]
    //even index are direct, odds are diagonal, starting with x+1 and continuing clockwise
    private Node[] connections = new Node[8];
    private bool available = true;
    [SerializeField]
    private float weight = 0;
    private float maxWeight = 255;
    [SerializeField]
    private int numberPings = 0;

    [SerializeField]
    private MeshRenderer mRenderer;
    private Material mMaterial;
    [SerializeField]
    private Color minCol, maxCol;

    private int WPP = 7;

    [SerializeField]
    private Material active, inactive;

    private void Start()
    {
        mMaterial = mRenderer.material;
        UpdateColor();
    }

    IEnumerator ColorAfter(float time)
    {
        yield return new WaitForSeconds(time);
        UpdateColor();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Agent" && available)
        {
            other.GetComponent<AgentMovement>().SetDirection(flowDirection);
        }
        if (other.tag == "Obstacle")
        {
            //Debug.Log("a thingus has occured");
            available = false;
            transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = inactive;
            transform.GetChild(2).gameObject.SetActive(!available);
        }
    }

    void OnTriggerLeave(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            available = true;
            transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = active;
            transform.GetChild(2).gameObject.SetActive(!available);
        }
    }


    public Vector3 GetFlow()
    {
        return flowDirection;
    }

    public Node GetConnection(int loc)
    {
        return connections[loc];
    }

    public void SetFlow(Vector3 dir)
    {
        flowDirection = dir.normalized;
        //change indicator to point correctly
        transform.GetChild(0).rotation = Quaternion.LookRotation(flowDirection);
    }

    public void SetConnection(int loc, Node toSet)
    {
        connections[loc] = toSet;
    }

    public bool GetAvailable()
    {
        return available;
    }

    public void ToggleAvailable()
    {
        available = !available;
        if(available)
        {
            transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = active;
        }
        else
        {
            transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = inactive;
        }

        transform.GetChild(2).gameObject.SetActive(!available);
        //transform.GetChild(0).gameObject.SetActive(available);
    }

    public float GetWeight()
    {
        return Mathf.Clamp(weight + (WPP * numberPings), 1, 250);
    }

    public void ScrollWeight(float scrollDelta)
    {
        SetWeight(Mathf.Clamp((weight + scrollDelta), 1, 20));
    }
    public void SetWeight(float toSet)
    {
        weight = toSet;
        UpdateColor();
    }

    private void UpdateColor()
    {
        
        float currentWeight = Mathf.Clamp(weight + (WPP * numberPings), 1, 255) / maxWeight;
        //Debug.Log("Updating Color " + currentWeight);

        float[] colVals = new float[3];
        colVals[0] = Mathf.Lerp(minCol.r, maxCol.r, currentWeight);
        colVals[1] = Mathf.Lerp(minCol.g, maxCol.g, currentWeight);
        colVals[2] = Mathf.Lerp(minCol.b, maxCol.b, currentWeight);



        //Debug.Log("Color Chosen: " + colVals[0] + ", " + colVals[1] + ", " + colVals[2]);

        mMaterial.SetFloat("cVal0", colVals[0]);
        mMaterial.SetFloat("cVal1", colVals[1]);
        mMaterial.SetFloat("cVal2", colVals[2]);
    }

    public void AddPing()
    {
        numberPings++;
        UpdateColor();
    }
    public void RemovePing()
    {
        numberPings--;
        UpdateColor();
    }
}
