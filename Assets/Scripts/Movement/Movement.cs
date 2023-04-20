using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    enum State
    {
        None,
        Horizontal,
        Upwards,
        Downwards,
    };

    public abstract Entity entity { get; }
    public abstract float moveAccel { get; }
    public abstract float moveMaxSpeed { get; }

    public Rigidbody2D rigidbody => entity.rigidbody;
    public float staticDrag => entity.staticDrag;
    public float dynamicDrag => entity.dynamicDrag;

    public Vector2 facingVector => _facingVector;
    public bool isNone => state == State.None;
    public bool isHorizontal => state == State.Horizontal;
    public bool isUpwards => state == State.Upwards;
    public bool isDownwards => state == State.Downwards;
    public bool turnaround => _turnaround;

    protected HashSet<PassthroughTrigger> passthroughTriggers = new();
    private Vector2 _facingVector = Vector2.left;
    private Coroutine coroutine = null;
    private State state = State.None;
    private bool _turnaround = false;

    public void Begin(Vector2 dir)
    {
        if (state != State.None) return;

        entity.SetRequestDynamic(this);
        dir = new Vector2(Mathf.Round(dir.x), Mathf.Round(dir.y));
        _turnaround = dir == -facingVector;
        _facingVector = dir;

        if (_facingVector.x != 0)
        {
            state = State.Horizontal;
            BeginMoveHorizontal();
            coroutine = entity.StartCoroutine(Util.TillNullRoutine(DuringMoveHorizontal));
        }
        else if (facingVector.y < 0)
        {
            state = State.Downwards;
            BeginMoveDownwards();
            coroutine = entity.StartCoroutine(Util.TillNullRoutine(DuringMoveDownwards));
        }
        else
        {
            state = State.Upwards;
            BeginMoveUpwards();
            coroutine = entity.StartCoroutine(Util.TillNullRoutine(DuringMoveUpwards));
        }
    }

    public void End()
    {
        entity.SetRequestStatic(this);
        if (coroutine != null)
        {
            entity.StopCoroutine(coroutine);
            coroutine = null;
        }

        State old = state;
        state = State.None; // Should be set to None before end methods are called.
        switch (old)
        {
            case (State.Horizontal): EndMoveHorizontal(); break;
            case (State.Downwards): EndMoveDownwards(); break;
            case (State.Upwards): EndMoveUpwards(); break;
            default: break;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            passthroughTriggers.Add(trigger);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            passthroughTriggers.Remove(trigger);
        }
    }

    protected virtual void BeginMoveHorizontal()
    {
        if (turnaround && entity.grounded)
        {
            var vel = rigidbody.velocity;
            vel.x = 0;
            rigidbody.velocity = vel;
        }
        entity.transform.rotation = Quaternion.Euler(0, facingVector.x < 0 ? 180 : 0, 0);
    }

    protected virtual object DuringMoveHorizontal()
    {
        Vector2 velocity = rigidbody.velocity;
        float ratio = Mathf.Abs(velocity.x) / moveMaxSpeed; // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
        ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

        Vector2 force = moveAccel * ratio * facingVector;

        rigidbody.AddForce(force);

        return new WaitForEndOfFrame();
    }

    protected virtual void EndMoveHorizontal() { }
    protected virtual void BeginMoveUpwards() { }
    protected virtual object DuringMoveUpwards() => null;
    protected virtual void EndMoveUpwards() { }

    protected virtual void BeginMoveDownwards()
    {
        foreach (var trigger in passthroughTriggers)
        {
            trigger.AllowPassthroughFor(entity.bodyCollider);
            trigger.AllowPassthroughFor(entity.feetCollider);
        }
    }

    protected virtual object DuringMoveDownwards() => null;
    protected virtual void EndMoveDownwards() { }
}
