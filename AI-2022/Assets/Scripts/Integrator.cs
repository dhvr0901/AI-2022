using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Integrator : MonoBehaviour
{
    [SerializeField]
    float worldMaxSpeed;

    //Integrates based on the given value
    public void Integrate(Particle2D subject, Vector2 location, Vector2 velocity, Vector2 acceleration, Vector2 forces, float mass, float damping)
    {
        Vector2 newPosition = (location + (velocity * Time.deltaTime));

        Vector2 newVelocity = (velocity * Mathf.Pow(damping, Time.deltaTime) + acceleration * Time.deltaTime);
        if(newVelocity.magnitude > worldMaxSpeed)
        {
            newVelocity -= newVelocity - newVelocity.normalized * worldMaxSpeed;
        }

        Vector2 newAcceleration = (forces * mass * Time.deltaTime);

        subject.ReceiveValues(newPosition, newVelocity, newAcceleration);
    }
}
