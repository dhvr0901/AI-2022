using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCombat : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 10, damage = 3, timerMax = 2;

    [SerializeField]
    private Material teamTrue, teamFalse;

    private bool team;
    private float health, timer = -1;

    private void Start()
    {
        health = maxHealth;
    }

    public void Initialize(bool setTeam)
    {
        team = setTeam;
        if(team)
        {
            GetComponent<MeshRenderer>().material = teamTrue;
        }
        else
        {
            GetComponent<MeshRenderer>().material = teamFalse;
        }
    }

    private void FixedUpdate()
    {
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        if(health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Agent")
        {
            AgentCombat enemy = collision.gameObject.GetComponent<AgentCombat>();
            if (timer < 0 && enemy.team != this.team)
            {
                Attack(enemy);
            }
        }
    }

    private void Attack(AgentCombat opponent)
    {
        opponent.health -= damage;
        timer = timerMax;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
