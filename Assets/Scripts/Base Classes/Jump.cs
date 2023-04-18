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

        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public virtual void FixedUpdate()
    {
        if (
            jumping &&
            ((jumpEndRequested && canEndJump)) // || rigidbody.velocity.y < 0)
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
            //ResetVerticalVelocity();

            // When the player jumps far the downwards force should be smaller
            // When the player jumps low the downwards force should be greater
            // The downwards force is inversily proportional to the heigth(jumpHeigthDelta) for the player.

            var forceScale = downForceScale * jumpForce / jumpHeigthDelta;
            rigidbody.AddForce(forceScale * Vector3.down, ForceMode2D.Impulse);
        }
        jumpEndRequested = false;
        jumping = false;
    }
}
