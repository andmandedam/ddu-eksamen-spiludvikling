using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public const float DEFAULT_ACCEL = 8f;
    public const float DEFAULT_SLOW_EXPONENT = 0.3f;
    public const float DEFAULT_MAX_SPEED = 8f;
    public const float DEFAULT_MIN_SPEED = 4f;
    public const float DEFAULT_GROUNDED_TURNAROUND_MULTIPLIER = 0.25f;
    public const float DEFAULT_AIRBORNE_TURNAROUND_MULTIPLIER = 0.75f;

        public abstract Entity entity { get; }


    public virtual float moveAccel => DEFAULT_ACCEL;
    public virtual float slowExponent => DEFAULT_SLOW_EXPONENT;
    public virtual float moveMaxSpeed => DEFAULT_MAX_SPEED;
    public virtual float moveMinSpeed => DEFAULT_MIN_SPEED;
    // public virtual float turnAroundMultiplier => DEFAULT_TURNAROUND_MULTIPLIER;

    private float moveSpeedFromSlow(float slowScore) => (moveMaxSpeed - moveMinSpeed) * Mathf.Exp(-slowExponent * slowScore) + moveMinSpeed;
    public float moveSpeed
    {
        get
        {
            float actualMod;
            if (speedModifier > 0)
            {
                actualMod = -moveSpeedFromSlow(speedModifier) + moveMaxSpeed;
            }
            else
            {
                actualMod = moveSpeedFromSlow(-speedModifier) - moveMaxSpeed;
            }
            actualMod = moveMaxSpeed + actualMod;
            return actualMod;
        }
    }

    public Rigidbody2D rigidbody => entity.rigidbody;
    public Vector2 facingVector => _facingVector;

    public bool isRunning => machine.running;
    public bool isHorizontal => machine.current == horizontalState;
    public bool isUpwards => machine.current == upwardsState;
    public bool isDownwards => machine.current == downwardsState;
    public bool turnaround => _turnaround;

    protected HashSet<PassthroughTrigger> passthroughTriggers = new();
    protected float speedModifier;

    private Vector2 _facingVector = Vector2.left;
    private bool _turnaround = false;

    private State horizontalState;
    private State upwardsState;
    private State downwardsState;
    private State.Machine machine;

    public virtual void HorizontalEntry()
    {
        entity.RequestDynamicDrag(this);

        if (turnaround)
        {
            var vel = rigidbody.velocity;
            if (entity.grounded)
            {
                vel.x *= DEFAULT_GROUNDED_TURNAROUND_MULTIPLIER;
            }
            else
            {
                vel.x *= DEFAULT_AIRBORNE_TURNAROUND_MULTIPLIER;
            }
            rigidbody.velocity = vel;
        }
        entity.transform.rotation = Quaternion.Euler(0, facingVector.x < 0 ? 180 : 0, 0);
    }

    public virtual object HorizontalDuring()
    {

        Vector2 velocity = rigidbody.velocity;
        float ratio = Mathf.Min(1, Mathf.Abs(velocity.x) / moveSpeed); // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
        ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

        Vector2 force = moveAccel * ratio * facingVector;

        rigidbody.AddForce(force);

        return new WaitForEndOfFrame();
    }

    public virtual void HorizontalExit()
    {
        entity.RequestStaticDrag(this);
    }
    public virtual void UpwardsEntry() { }
    public virtual object UpwardsDuring() => null;
    public virtual void UpwardsExit() { }

    public virtual void DownwardsEntry()
    {
        foreach (var trigger in passthroughTriggers)
        {
                trigger.AllowPassthroughFor(entity.bodyCollider);
            trigger.AllowPassthroughFor(entity.feetCollider);
        }
    }

    public virtual object DownwardsDuring() => null;
    public virtual void DownwardsExit() { }

    public void Enable()
    {
        machine = new(entity);
        horizontalState = new(HorizontalEntry, HorizontalExit, HorizontalDuring);
        upwardsState = new(UpwardsEntry, UpwardsExit, UpwardsDuring);
        downwardsState = new(DownwardsEntry, DownwardsExit, DownwardsDuring);
    }

    public void Begin(Vector2 dir)
    {
        if (machine.running) return;

        entity.RequestDynamicDrag(this);
        dir = new Vector2(Mathf.Round(dir.x), Mathf.Round(dir.y));
        _turnaround = dir == -facingVector;
        _facingVector = dir;

        if (_facingVector.x != 0)
        {
            machine.Run(horizontalState);
        }
        else if (facingVector.y < 0)
        {
            machine.Run(downwardsState);
        }
        else
        {
            machine.Run(upwardsState);
        }
    }

    public void End()
    {
        machine.Abort();
    }

    public void Slow(float x)
    {
        speedModifier -= x;
    }

    public void SpeedUp(float x)
    {
        speedModifier += x;
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
}
