using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Agent : MonoBehaviour
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] private Manager manager;
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] protected float maxForce = 10;
    protected float maxSpeed;
    protected Vector3 totalForce;

    // wander
    [SerializeField] private float wanderAngle = 0f;
    [SerializeField] protected float wanderStep = 1f;
    [SerializeField] protected float wanderTime = 1f;
    [SerializeField] protected float wanderRadius;

    // avoid obstacles
    protected List<Vector3> foundObstacles = new List<Vector3>();
    [SerializeField] protected float avoidTime = 1f;
    [SerializeField] protected Vector3 totalAvoidForces;

    // ----- start ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        maxSpeed = physicsObject.MaxSpeed;
    }



    // ----- update ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        totalForce = Vector3.zero;
        CalcSteeringForces();
        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);
        physicsObject.ApplyForce(totalForce);
        transform.rotation = Quaternion.LookRotation(Vector3.back, physicsObject.Direction);
    }

    // ----- methods --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected abstract void CalcSteeringForces();

    protected Vector3 Seek(Vector3 targetPos)
    {
        // calculate desired velocity
        Vector3 desiredVelocity = targetPos - physicsObject.Position;

        // set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // calculate seek steering force
        Vector3 seekingForce = desiredVelocity - physicsObject.Velocity;

        // return seek steering force
        return seekingForce;

    }

    public Vector3 Seek(PhysicsObject target)
    {
        return Seek(target.Position);
    }

    protected Vector3 Flee(Vector3 targetPos)
    {
        // Calculate desired velocity
        Vector3 desiredVelocity = physicsObject.Position - targetPos;

        // Set desired = max speed
        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        // Calculate seek steering force
        Vector3 fleeingForce = desiredVelocity - physicsObject.Velocity;

        // Return seek steering force
        return fleeingForce;

    }

    public Vector3 Flee(PhysicsObject target)
    {
        return Flee(target.Position);
    }

    public Vector3 CalcFuturePosition(float time)
    {
        return physicsObject.Velocity * time + transform.position;
    }

    protected Vector3 Wander(float time, float radius)
    {
        Vector3 targetPos = CalcFuturePosition(time);

        wanderAngle += Random.Range(-wanderStep, wanderStep) * Mathf.Deg2Rad;

        targetPos.x += Mathf.Cos(wanderAngle) * radius;
        targetPos.y += Mathf.Sin(wanderAngle) * radius;

        return Seek(targetPos);
    }

    protected Vector3 StayInBounds()
    {
        Vector3 futurePosition = CalcFuturePosition(wanderTime);

        if (futurePosition.x > physicsObject.ScreenMax.x ||
           futurePosition.x < -physicsObject.ScreenMax.x ||
           futurePosition.y > physicsObject.ScreenMax.y ||
           futurePosition.y < -physicsObject.ScreenMax.y)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

    // get a separate in there!!! (kenzie sent one but watch the video too)

    // (1 - fDot / dist)
    protected Vector3 AvoidObstacles(float time)
    {
        totalAvoidForces = Vector3.zero;
        foundObstacles.Clear();

        foreach (Obstacle obs in manager.Obstacles)
        {
            Vector3 agentToObstacle = obs.transform.position - transform.position;
            float rightDot = 0, forwardDot = 0;

            forwardDot = Vector3.Dot(physicsObject.Direction, agentToObstacle);

            Vector3 futurePos = CalcFuturePosition(time);
            float dist = Vector3.Distance(transform.position, futurePos) + physicsObject.Radius;

            // if in front of me
            if (forwardDot >= -obs.Radius)
            {

                // within the box in front of us
                if (forwardDot <= dist + obs.Radius)
                {
                    // how far left/right
                    rightDot = Vector3.Dot(transform.right, agentToObstacle);

                    Vector3 steeringForce = transform.right * (1 - forwardDot / dist) * physicsObject.MaxSpeed;

                    // is the obstacle within the safe box width
                    if (Mathf.Abs(rightDot) <= physicsObject.Radius + obs.Radius)
                    {
                        foundObstacles.Add(obs.transform.position);

                        // if left, steer right
                        if (rightDot < 0)
                        {
                            totalAvoidForces += steeringForce;
                        }

                        // if right, steer left
                        else
                        {
                            totalAvoidForces -= steeringForce;
                        }
                    }
                }
            }
        }

        return totalAvoidForces;
    }
}
