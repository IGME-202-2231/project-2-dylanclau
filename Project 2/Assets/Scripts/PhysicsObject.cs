using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    // ----- fields ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;

    private Vector3 acceleration; // sum of all forces in a frame

    [SerializeField] private float mass; // mass of object

    [SerializeField] private float maxSpeed;

    [SerializeField] private float gravityStrength;

    [SerializeField] private float frictionCoeff;

    private Vector3 screenMax = Vector3.zero;



    // ----- properties -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public Vector3 Position { get { return position; } }

    public Vector3 Direction { get { return direction;} }

    public Vector3 Velocity { get {  return velocity; } }

    public float MaxSpeed { get { return maxSpeed; } }

    public Vector3 ScreenMax { get { return screenMax; } }


    // ----- start ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        position = transform.position;

        direction = UnityEngine.Random.insideUnitCircle.normalized;

        screenMax.y = Camera.main.orthographicSize;
        screenMax.x = screenMax.y * Camera.main.aspect;
    }



    // ----- update ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        // apply all forces first
        ApplyGravity(gravityStrength);
        ApplyFriction(frictionCoeff);

        // calculate velocity for this frame
        velocity += acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        position += velocity * Time.deltaTime;

        // get current direction from velocity
        direction = velocity.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.back, direction);

        transform.position = position;

        // zero out acceleration
        acceleration = Vector3.zero;
    }



    // ----- methods --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void ApplyGravity(float strength)
    {
        Vector3 force = new Vector3(0, strength, 0);
        acceleration += force;
    }

    void ApplyFriction(float coeff)
    {
        Vector3 friction = velocity * -1;
        friction.Normalize();
        friction = friction * coeff;
        ApplyForce(friction);
    }
}
