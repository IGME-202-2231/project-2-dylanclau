using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{
    protected override void CalcSteeringForces()
    {
        totalForce += Wander(wanderTime, wanderRadius);
        totalForce += StayInBounds();
    }
}
