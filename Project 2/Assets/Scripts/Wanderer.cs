using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{
    [SerializeField] float wanderWeight;
    [SerializeField] float stayInBoundsWeight;
    protected override void CalcSteeringForces()
    {
        totalForce += Wander(wanderTime, wanderRadius) * wanderWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;

        // add in a separate
    }
}
