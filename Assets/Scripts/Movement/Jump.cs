using UnityEngine;

public abstract class Jump
{
    enum State
    {
        None, 
        Windup,
        Jumping,
    }

    public abstract Entity entity { get; }
    public abstract float jumpForce { get; }
    public abstract float minJumpTime { get; }
    public abstract float downForceScale { get; }
    public abstract bool canJump { get; }
    public abstract float windupTime { get; }

    public Rigidbody2D rigidbody => entity.rigidbody;
    public float dynamicDrag => entity.dynamicDrag;
    public float staticDrag => entity.staticDrag;

    public bool jumping => state == State.Jumping;
    public bool jumpWindup => state == State.Windup;


    private State state = State.None;
    private Coroutine coroutine = null;

    public virtual void Begin()
    {
        if (canJump)
        {
            state = State.Windup;
            entity.StartCoroutine(Util.TimedRoutine(
                windupTime,
                DuringWindup,
                () =>
                {
                    state = State.Jumping;
                    entity.StartCoroutine(Util.TimedWhileRoutine(
                        minJumpTime,
                        () => !entity.grounded,
                        DuringJump,
                        End
                    ));
                })
            );
        }
    }

    public virtual void End()
    {
        if (coroutine != null)
        {
            entity.StopCoroutine(coroutine);
            coroutine = null;
        }
        if (jumping)
        {
            state = State.None;
            entity.SetRequestStatic(this);
            EndJump();
        }
    }

    protected virtual void OnWindup(){}
    protected virtual object DuringWindup() => null;

    protected virtual void OnJump()
    {
        var velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;

        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    protected virtual object DuringJump()
    {
        if (entity.rigidbody.velocity.y < 0)
        {
            End();
        }
        return new WaitForEndOfFrame();
    }

    protected virtual void EndJump() { }
}
