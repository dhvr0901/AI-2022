using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField]
    private TowerType[] Towers = new TowerType[4];
    [SerializeField]
    private GameObject[] Visuals = new GameObject[4];

    [SerializeField]
    private LayerMask NodeMask;

    [SerializeField]
    private float firingTimer;
    private int chosenTower = 0;
    private Transform coreTrans;

    private void Start()
    {
        coreTrans = GameObject.FindGameObjectsWithTag("Core")[0].transform;   
    }

    private void Update()
    {
        Firing();
    }

    private void PropogateInfluence()
    {
        Debug.Log("propogating influence");

        //add weight to the graph
        float bIR = Towers[chosenTower].BonusInfluenceRange;
        Collider[] nearby = Physics.OverlapSphere(transform.position, Towers[chosenTower].maxRange + bIR, NodeMask, QueryTriggerInteraction.Collide);
        foreach(Collider collider in nearby)
        {
            Debug.Log("There are colliders");
            if (collider.tag == "Node" && chosenTower > 0)
            {
                Node temp = collider.GetComponent<Node>();
                float distance = (collider.transform.position - transform.position).magnitude;
                if (distance > Mathf.Clamp(Towers[chosenTower].minRange - bIR, 0, 100))
                {
                    temp.AddPing();
                }
                if (distance < Towers[chosenTower].maxRange + bIR/2 && distance > Towers[chosenTower].minRange - bIR / 2)
                {
                    temp.AddPing();
                }
                if (distance < Towers[chosenTower].maxRange && distance > Towers[chosenTower].minRange)
                {
                    temp.AddPing();
                }
            }
        }
    }

    private void ExtractInfluence()
    {
        //add weight to the graph
        float bIR = Towers[chosenTower].BonusInfluenceRange;
        Collider[] nearby = Physics.OverlapSphere(transform.position, Towers[chosenTower].maxRange + bIR, NodeMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in nearby)
        {
            
            if (collider.tag == "Node" && chosenTower > 0)
            {
                Node temp = collider.GetComponent<Node>();
                float distance = (collider.transform.position - transform.position).magnitude;
                if (distance > Mathf.Clamp(Towers[chosenTower].minRange - bIR, 0, 100))
                {
                    temp.RemovePing();
                }
                if (distance < Towers[chosenTower].maxRange + bIR / 2 && distance > Towers[chosenTower].minRange - bIR / 2)
                {
                    temp.RemovePing();
                }
                if (distance < Towers[chosenTower].maxRange && distance > Towers[chosenTower].minRange)
                {
                    temp.RemovePing();
                }
            }
        }
    }

    private void Firing()
    {
        //manage gun firing
        if (firingTimer < 0)
        {
            GameObject[] Agents = GameObject.FindGameObjectsWithTag("Agent");
            List<GameObject> CloseAgents = new List<GameObject>();
            foreach (GameObject agent in Agents)
            {
                if ((agent.transform.position - transform.position).magnitude < Towers[chosenTower].maxRange)
                {
                    CloseAgents.Add(agent);
                }
            }
            if (CloseAgents.Count > 0)
            {
                GameObject closestA = CloseAgents[0];
                float closestL = (closestA.transform.position - coreTrans.position).magnitude;

                foreach (GameObject agent in CloseAgents)
                {
                    float tempL = (agent.transform.position - coreTrans.position).magnitude;
                    if (tempL < closestL && (agent.transform.position - transform.position).magnitude > Towers[chosenTower].minRange)
                    {
                        closestA = agent;
                        closestL = tempL;

                    }
                }


                closestA.GetComponent<AgentCombat>().TakeDamage(Towers[chosenTower].damage);
                Debug.DrawLine(Visuals[chosenTower].transform.position, closestA.transform.position, Color.black, 0.35f);
                firingTimer = Towers[chosenTower].GetFireTimer();


            }
        }

        firingTimer -= Time.deltaTime;



    }

    public void CycleTower()
    {
        ExtractInfluence();
        Visuals[chosenTower].SetActive(false);
        chosenTower = (chosenTower + 1) % Towers.Length;
        Visuals[chosenTower].SetActive(true);
        PropogateInfluence();
    }


}
