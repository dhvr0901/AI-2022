using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    //store mass as inverse mass (1/mass)
    [SerializeField]
    private float Mass = 1;
    public float invMass = 1;
    public float radius = 0.5f;
    public float RestitutionCo = 1f;
    public Vector2 Velocity = new Vector2(0, 0);
    [SerializeField]
    public Vector2 Acceleration = new Vector2(0, 0.0f);
    [SerializeField]
    private Vector2 AdditiveAcceleration = new Vector2(0, 0.0f);
    [SerializeField]
    private Vector2 AccumulatedForces = new Vector2(0, 0);
    [SerializeField]
    private float DampingConstant = 0.999f;
    private Integrator TheIntegrator;

    [SerializeField]
    public bool active = true;


    // Start is called before the first frame update
    void Start()
    {
        TheIntegrator = (Integrator)GameObject.FindGameObjectWithTag("Integrator").GetComponent(typeof(Integrator));
        GameObject.FindGameObjectWithTag("PhysicsRegistry").GetComponent<PhysicsRegistry>().AddParticle(this);
        invMass = 1 / Mass;
        radius = transform.localScale.x / 2;
    }

    private void OnDestroy()
    {
        //GameObject.FindGameObjectWithTag("PhysicsRegistry").GetComponent<CollisionSystem>().RemoveParticle(this);
    }

    private void Update()
    {

        if (active)
        {
            TheIntegrator.Integrate(this, gameObject.transform.position, Velocity, Acceleration + AdditiveAcceleration, AccumulatedForces, invMass, DampingConstant);
            AccumulatedForces = new Vector2(0, 0);
            //Acceleration = new Vector2(0, 0);
        }
    }

    //Fixed Update
    void FixedUpdate()
    {
        if (active)
        {

        }
    }

    public void SeedParticle(Vector2 velocity, Vector2 accelerationMod, float radius, float damping = 0.999f, float mass = 1, bool fire = false, float spawnInterval = 60)
    {
        Velocity = velocity;
        AdditiveAcceleration += accelerationMod;
        DampingConstant = damping;
        Mass = mass;
        invMass = 1 / mass;

    }
    public void ReceiveValues(Vector2 newPosition, Vector2 newVelocity, Vector2 newAcceleration)
    {
        gameObject.transform.position = newPosition;
        Velocity = newVelocity;
        Acceleration = newAcceleration;
    }

    public void AddForce(Vector2 added)
    {
        AccumulatedForces += added;
    }

    private void ResetForces()
    {
        AccumulatedForces = new Vector2(0, 0);
    }


}

