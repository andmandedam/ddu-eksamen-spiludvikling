using UnityEngine;

public abstract class Jump
{
    // Configuration variables
    public abstract float jumpForce { get; }
    public abstract float minJumpHeigth { get; }
    public abstract float downForceScale { get; }
    public abstract bool canJump { get; }
    public abstract Rigidbody2D rigidbody { get; }

    public float initialJumpHeigth { get; private set; } // Elevation when jump started
    public bool jumpEndRequested { get; private set; }
    public bool jumping { get; private set; }

    // Properties
    private float jumpHeigthDelta => Mathf.Abs(rigidbody.transform.position.y - initialJumpHeigth);
    private bool canEndJump => minJumpHeigth < jumpHeigthDelta;

    // Functions
    private void ResetVerticalVelocity() => rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);

    public virtual void Start()
    {
        if (canJump)
        {
            DoJump();
        }
    }

    protected void DoJump()
    {
        initialJumpHeigth = rigidbody.transform.position.y;
        jumping = true;

        ResetVerticalVelocity();

        var force = jumpForce * Vector2.up;
        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public virtual void FixedUpdate()
    {
        if (
            jumping &&
            ((jumpEndRequested && canEndJump) || rigidbody.velocity.y < 0)
            )
        {
            JumpHardEnd();
        }
    }

    public virtual void End()
    {
        jumpEndRequested = true;
    }

    private void JumpHardEnd()
    {
        if (jumping)
        {
            ResetVerticalVelocity();

            var force = downForceScale * jumpHeigthDelta * jumpForce * Vector2.down;
            rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
        jumpEndRequested = false;
        jumping = false;
    }
}
