using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Agent : MonoBehaviour
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] protected PhysicsObject physicsObject;
    [SerializeField] protected float maxForce = 10;

    private float maxSpeed;

    // wander
    [SerializeField] private float wanderAngle = 0f;
    [SerializeField] protected float wanderStep = 1f;
    [SerializeField] protected float wanderTime = 1f;
    [SerializeField] protected float wanderRadius;

    protected Vector3 totalForce;


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

    /*
    protected Vector3 AvoidObstacles(float time)
    {
        Vector3 totalAvoidForces = Vector3.zero;
        foundObstacles.Clear();

        foreach (Obstacle obs in ScreenManager.Obstacles)
        {
            Vector3 agentToObstacle = obs.transform.position - transform.position;
            float rightDot = 0;
            float forwardDot = 0;
            forwardDot = Vector3.Dot(physicsObject.direction, agentToObstacle);

            Vector3 futurePos = GetFuturePosition(time);
            float dist = Vector3.Distance(transform.position, futurePos) + radius;

            if (forwardDot >= -obs.radius)
            {
                if (forwardDot <= obs.radius)
                {
                    rightDot = Vector3.Dot(transform.right, agentToObstacle);
                    Vector3 steeringForce = transform.right * (forwardDot / dist) * physicsObject.maxSpeed;

                    if (Mathf.Abs(rightDot) <= radius + obs.radius)
                    {
                        foundObstacles.Add(obs.transform.position);

                        if (rightDot < 0)
                        {
                            totalAvoidForces += steeringForce;
                        }
                        else
                        {
                            totalAvoidForces += -steeringForce;
                        }
                    }
                }
            }
        }

        return totalAvoidForces;
    }
    */
}
