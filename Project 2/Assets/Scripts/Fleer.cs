using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fleer : Agent
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] PhysicsObject target;



    // ----- methods --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void CalcSteeringForces()
    {
        physicsObject.ApplyForce(Flee(target));
    }
}
