using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Agent
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] PhysicsObject target;

    protected override void CalcSteeringForces()
    {
        // seek to where it is now
        physicsObject.ApplyForce(Seek(target));
    }
}
