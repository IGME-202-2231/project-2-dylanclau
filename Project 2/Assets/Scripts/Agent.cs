using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [SerializeField] protected PhysicsObject physicsObject;

    private float maxSpeed;

    // wander
    private float wanderAngle = 0f;
    public float maxWanderAngle = 45f;
    //public float maxWanderChangePerSecond = 10f;
    [SerializeField] protected float wanderTime;
    [SerializeField] protected float wanderRadius;


    // ----- start ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        maxSpeed = physicsObject.MaxSpeed;
    }



    // ----- update ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        CalcSteeringForces();
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

        wanderAngle += Random.Range(-maxWanderAngle, maxWanderAngle) * Mathf.Deg2Rad;

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
}
