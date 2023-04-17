using UnityEngine;

public abstract class Movement
{
    public abstract Rigidbody2D rigidbody { get; }
    public abstract float staticDrag { get;  }
    public abstract float dynamicDrag { get; }
    public abstract float moveAcceleration { get; }
    public abstract float moveMaxSpeed { get; }
    public bool moving => movementVector != Vector2.zero;

    public Vector2 movementVector { get; private set; }

    public void Start(Vector2 dir)
    {
        dir.y = 0;
        movementVector = dir;
        rigidbody.drag = dynamicDrag;
        Debug.LogFormat("{0} set drag to {1}", this, rigidbody.drag);
    }

    public void FixedUpdate()
    {
        if (movementVector != Vector2.zero)
        {
            Vector2 velocity = rigidbody.velocity;
            float ratio = Mathf.Abs(velocity.x) / moveMaxSpeed; // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
            ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

            Vector2 force = moveAcceleration * ratio * movementVector;

            rigidbody.AddForce(force);
        }
    }

    public void End()
    {
        movementVector = Vector2.zero;
        rigidbody.drag = staticDrag;
        Debug.LogFormat("{0} set drag to {1}", this, rigidbody.drag);
    }
}
