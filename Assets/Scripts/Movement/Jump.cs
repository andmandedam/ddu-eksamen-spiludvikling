using UnityEngine;

public abstract class Jump
{
    public abstract Entity entity { get; }
    public abstract float jumpForce { get; }
    public abstract float minJumpTime { get; }
    public abstract float downForceScale { get; }
    public abstract bool canJump { get; }

    public Rigidbody2D rigidbody => entity.rigidbody;
    public float dynamicDrag => entity.dynamicDrag;
    public float staticDrag => entity.staticDrag;

    public bool jumping => _jumping;

    private bool _jumping = false;
    private Coroutine coroutine = null;

    public virtual void Begin()
    {
        if (canJump)
        {
            _jumping = true;
            entity.SetRequestDynamic(this);
            rigidbody.drag = dynamicDrag; OnJump();
            var time = Time.time;
            coroutine = entity.StartCoroutine(Util.WhileRoutine(() => !entity.grounded || (Time.time - time < minJumpTime), DuringJump));
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
            _jumping = false;
                entity.SetRequestStatic(this);
            EndJump();
        }
    }

    protected virtual void OnJump()
    {
        var velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;

        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    protected virtual object DuringJump() => null;

    protected virtual void EndJump() { }
}
