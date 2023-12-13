using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Curse;

public class Sorcerer : Agent
{
    public enum SorcererStates
    {
        Hunt,
        Wander
    }

    [SerializeField] private float wanderWeight;
    [SerializeField] private float stayInBoundsWeight;
    [SerializeField] private float avoidObstaclesWeight;
    [SerializeField] private float seekWeight;

    [SerializeField] private float separateWeight;
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;


    private SorcererStates sorcererState;

    public SorcererStates SorcererState
    {
        get { return sorcererState; }
        set { sorcererState = value; }
    }

    protected override void CalcSteeringForces()
    {
        // always
        totalForce += AvoidObstacles(avoidTime) * avoidObstaclesWeight;
        totalForce += Separate(Manager.Sorcerers) * separateWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;


        switch (sorcererState)
        {
            // when another sorcerer is in the vicinity
            case SorcererStates.Hunt:
                totalForce += Cohesion(Manager.Sorcerers) * cohesionWeight;
                totalForce += Alignment(Manager.Sorcerers) * alignmentWeight;
                totalForce += Seek(FindClosestCurse().PhysicsObject.Position) * seekWeight;
                break;

            // when alone
            case SorcererStates.Wander:
                totalForce += Wander(wanderTime, wanderRadius) * wanderWeight;
                break;
        }
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

    public void ManageState()
    {
        float dist;

        foreach (Sorcerer s in Manager.Sorcerers)
        {
            dist = Vector3.Distance(this.PhysicsObject.Position, s.PhysicsObject.Position);
            if (dist <= 5 && dist > 0)
            {
                sorcererState = SorcererStates.Hunt;
                s.SorcererState = SorcererStates.Hunt;
            }
            else
            {
                sorcererState = SorcererStates.Wander;
                s.SorcererState = SorcererStates.Wander;
            }
        }
    }

    public Agent FindClosestCurse()
    {
        Agent closest = Manager.Curses[0];
        float smallestDist = Vector3.Distance(PhysicsObject.Position, closest.PhysicsObject.Position);

        foreach(Curse c in Manager.Curses)
        {
            float distance = Vector3.Distance(PhysicsObject.Position, c.PhysicsObject.Position);
            if (distance < smallestDist)
            {
                smallestDist = distance;
                closest = c;
            }
        }

        return closest;
    }
}
