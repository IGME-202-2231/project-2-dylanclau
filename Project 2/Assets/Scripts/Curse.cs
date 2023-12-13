using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Sorcerer;

public class Curse : Agent
{
    public enum CurseStates
    {
        Passive,
        Fearful
    }

    [SerializeField] private float wanderWeight;
    [SerializeField] private float stayInBoundsWeight;
    [SerializeField] private float avoidObstaclesWeight;
    [SerializeField] private float fleeWeight;

    [SerializeField] private float separateWeight;
    [SerializeField] private float cohesionWeight;
    [SerializeField] private float alignmentWeight;

    private CurseStates curseState;

    public CurseStates CurseState
    { 
        get { return curseState; } 
        set { curseState = value; }
    }
    protected override void CalcSteeringForces()
    {
        // always
        totalForce += Separate(Manager.Curses) * separateWeight;
        totalForce += StayInBounds() * stayInBoundsWeight;
        totalForce += AvoidObstacles(avoidTime) * avoidObstaclesWeight;

        switch (curseState)
        {
            case CurseStates.Passive:
                totalForce += Wander(wanderTime, wanderRadius) * wanderWeight;
                totalForce += Cohesion(Manager.Curses) * cohesionWeight;
                totalForce += Alignment(Manager.Curses) * alignmentWeight;
                break;

            case CurseStates.Fearful:
                totalForce += Flee(FindClosestSorcerer().PhysicsObject.Position) * fleeWeight;
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

            if (dist <= 3)
            {
                curseState = CurseStates.Fearful;
            }
            else
            {
                curseState = CurseStates.Passive;
            }
        }
    }

    public Agent FindClosestSorcerer()
    {
        Agent closest = Manager.Sorcerers[0];
        float smallestDist = Vector3.Distance(PhysicsObject.Position, closest.PhysicsObject.Position);

        foreach (Sorcerer s in Manager.Sorcerers)
        {
            float distance = Vector3.Distance(PhysicsObject.Position, s.PhysicsObject.Position);
            if (distance < smallestDist)
            {
                smallestDist = distance;
                closest = s;
            }
        }

        return closest;
    }
}
