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
    public float maxWanderChangePerSecond = 10f;
    [SerializeField] protected float wanderTime;



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

    protected Vector3 Wander()
    {
        // update the angle of our current wander
        float maxWanderChange = maxWanderChangePerSecond * Time.deltaTime;
        wanderAngle += UnityEngine.Random.Range(-maxWanderChange, maxWanderChange);

        wanderAngle = Mathf.Clamp(wanderAngle, -maxWanderAngle, maxWanderAngle);

        // get a position that is defined by the wander angle
        Vector3 wanderTarget = Quaternion.Euler(0, 0, wanderAngle) * physicsObject.Direction.normalized + physicsObject.Position;

        // seek towards our wander position
        return Seek(wanderTarget);
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
