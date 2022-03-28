using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NodeHeat : MonoBehaviour
{
    [SerializeField]
    private float heat;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Agent") ///this should, perhaps, be offboarded further
        {
            GetComponent<NodeHeat>().AddHeat(0.1f);
        }
    }

    public void AddHeat(float add)
    {
        heat += add;
        heat = Mathf.Clamp01(heat);
    }

    public void DecayHeat(float decayFactor)
    {
        heat -= heat * decayFactor * Time.deltaTime;
        if(heat < 0.001f)
        {
            heat = 0;
        }
    }


}
