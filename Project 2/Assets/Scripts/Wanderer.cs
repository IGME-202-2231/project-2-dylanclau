using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{
    protected override void CalcSteeringForces()
    {
        physicsObject.ApplyForce(Wander(wanderTime, wanderRadius));
        physicsObject.ApplyForce(StayInBounds());
    }
}
