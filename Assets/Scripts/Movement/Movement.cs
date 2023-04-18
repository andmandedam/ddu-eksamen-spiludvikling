using UnityEngine;

public abstract class Movement
{
    public abstract Entity entity { get; }

    public Rigidbody2D rigidbody => entity.rigidbody;

    public virtual float staticDrag => entity.staticDrag;
    public virtual float dynamicDrag => entity.dynamicDrag;

    public abstract float moveAccel { get; }
    public abstract float moveMaxSpeed { get; }

    public bool moving => movementVector != Vector2.zero;
    public Vector2 movementVector { get; private set; }

    public void Start(Vector2 dir)
    {
        dir.y = 0;
        movementVector = dir;
        rigidbody.drag = dynamicDrag;
    }

    public void FixedUpdate()
    {
        if (moving)
        {
            Vector2 velocity = rigidbody.velocity;
            float ratio = Mathf.Abs(velocity.x) / moveMaxSpeed; // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
            ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

            Vector2 force = moveAccel * ratio * movementVector;

            rigidbody.AddForce(force);
        }
    }

    public void End()
    {
        movementVector = Vector2.zero;
        rigidbody.drag = staticDrag;
    }
}
