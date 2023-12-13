using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorcerer : Agent
{
    [SerializeField] private float wanderWeight;
    [SerializeField] private float stayInBoundsWeight;
    [SerializeField] private float avoidObstaclesWeight;

    [SerializeField] private float separateWeight;
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;

    protected override void CalcSteeringForces()
    {
        totalForce += Wander(wanderTime, wanderRadius) * wanderWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;
        totalForce += AvoidObstacles(avoidTime) * avoidObstaclesWeight;

        // flocking
        totalForce += Separate(Manager.Sorcerers) * separateWeight;
        totalForce += Cohesion(Manager.Sorcerers) * cohesionWeight;
        totalForce += Alignment(Manager.Sorcerers) * alignmentWeight;
    }

    private void OnDrawGizmos()
    {
        //
        //  Draw safe space box
        //
        Vector3 futurePos = CalcFuturePosition(avoidTime);

        float dist = Vector3.Distance(transform.position, futurePos) + physicsObject.Radius;

        Vector3 boxSize = new Vector3(physicsObject.Radius * 2f,
            dist
            , physicsObject.Radius * 2f);

        Vector3 boxCenter = Vector3.zero;
        boxCenter.y += dist / 2f;

        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(boxCenter, boxSize);
        Gizmos.matrix = Matrix4x4.identity;


        //
        //  Draw lines to found obstacles
        //
        Gizmos.color = Color.yellow;

        foreach (Vector3 pos in foundObstacles)
        {
            Gizmos.DrawLine(transform.position, pos);
        }
    }
}
