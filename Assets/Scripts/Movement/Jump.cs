using UnityEngine;

public abstract class Jump
{
    public abstract Entity entity { get; }
    public abstract float jumpForce { get; }
    public abstract float minJumpTime { get; }
    public abstract float downForceScale { get; }
    public abstract bool canJump { get; }
    public abstract float windupTime { get; }

    public Rigidbody2D rigidbody => entity.rigidbody;
    public float dynamicDrag => entity.dynamicDrag;
    public float staticDrag => entity.staticDrag;

    public bool isRunning => machine.running;
    public bool isInWindup => machine.current == windupState;
    public bool isJumping => machine.current == jumpingState;

    private State windupState;
    private State jumpingState;
    private State.Machine machine;

    public virtual void WindupEntry() { }
    public virtual object WindupDuring() => null;
    public virtual void WindupExit() { }

    public virtual void JumpingEntry() {
        entity.RequestDynamicDrag(this);
        var velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;

        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }
    public virtual object JumpingDuring() => null;
    public virtual void JumpingExit() {
        entity.RequestStaticDrag(this);
    }

    public virtual void Enable()
    {
        machine = new(entity);
        windupState = new(WindupEntry, WindupExit, WindupDuring);
        jumpingState = new(JumpingEntry, JumpingExit, JumpingDuring);

        windupState.After(windupTime, jumpingState);
        jumpingState.AddTransition(() => entity.rigidbody.velocity.y < 0, null);
    }

    public virtual void Begin()
    {
        if (canJump)
        {
            machine.Run(windupState);
        }
    }

    public virtual void End()
    {
        machine.Abort();
    }
}
