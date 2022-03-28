using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnTime = 1;
    [SerializeField]
    private bool team;
    [SerializeField]
    GameObject agent;

    private float timer = 0;

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SpawnAgent();
            timer = spawnTime;
        }
    }

    private void SpawnAgent()
    {
        AgentCombat temp = Instantiate(agent).GetComponent<AgentCombat>();
        temp.transform.position = new Vector3(transform.position.x, 1.5f, Random.Range(1, 399));
        temp.Initialize(team);
    }
}
