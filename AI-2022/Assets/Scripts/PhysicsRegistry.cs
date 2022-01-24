using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRegistry : MonoBehaviour
{

    public List<Particle2D> particleRegistry = new List<Particle2D>();

    public void AddParticle(Particle2D toAdd)
    {
        particleRegistry.Add(toAdd);
    }

    public void RemoveParticle(Particle2D toRemove)
    {
        particleRegistry.Remove(toRemove);
    }
}

