using System;
using UnityEngine;

[Serializable]
public class Movement : Actor.Extension
{
    // Constant speed modifier is multiplied with before being input to logistic function, see {LogisticInInterval}.
    // High values indicate that a low speedModifier is needed to approach min/max speed.
    // Low values indicate that a high speedModifier is needed to approach min/max speed.
    private const float SPEED_MODIFIER_IMPACT = 0.1f;

    [Header("Movement")]
    [SerializeField] float _minMoveAccel;
    [SerializeField] float _maxMoveAccel;
    [SerializeField] float _minMoveSpeed;
    [SerializeField] float _maxMoveSpeed;

    [SerializeField] float _groundedAccelMult;
    [SerializeField] float _airborneAccelMult;
    [SerializeField] float _groundedTurnMult;
    [SerializeField] float _airborneTurnMult;

    private Actor _actor;
    private State _horizontal;
    private int _moveDirection;
    private bool _turnaround;
    private float _speedModifier;

    public override Actor actor => _actor;
    public State horizontal => _horizontal;
    public Vector2 moveDirection => new Vector2(_moveDirection, 0);
    public float speedModifier => _speedModifier;
    private float turnMultiplier => actor.grounded ? _groundedTurnMult : _airborneTurnMult;

    public void Enable(Actor actor)
    {
        _actor = actor;
        _horizontal = new(OnHorizontal, DuringHorizontal, AfterHorizontal);
        _moveDirection = 0;
        _turnaround = false;
        _speedModifier = 0;
    }

    public void Begin(int dir)
    {
        if (
            (dir != 0) &&
            !(dir == _moveDirection && isInProgress) // not already in the process of moving in the direction
        )
        {
            _turnaround = _moveDirection == -dir;
            _moveDirection = dir;

            Run(horizontal);
        }
    }

    public void End() => Abort();

    private static float LogisticInInterval(float x, float low, float high)
    {
        return low + (high - low) / (1 + Mathf.Exp(-SPEED_MODIFIER_IMPACT * x));
    }

    // Returns the speed the actor "would like to" go.
    public float GetCurrentMaxMoveSpeed() => LogisticInInterval(speedModifier, _minMoveSpeed, _maxMoveSpeed);

    public float GetCurrentMoveAccel() =>
                (actor.grounded ? _groundedAccelMult : _airborneAccelMult) *
                LogisticInInterval(speedModifier, _minMoveAccel, _maxMoveAccel);

    public bool HasAccelerated() => 0.9f < rigidbody.velocity.x / GetCurrentMaxMoveSpeed();

    public virtual void OnHorizontal()
    {
        actor.RequestDynamicDrag(this);
        actor.animator.SetBool("moving", true);

        // The turnaround multiplier is always applied.
        // Either we are tuning around, which is when this is relevant
        // Otherwise the actor wasn't moving before this was called, so the velocity is zero.
        // In that case it's doesn't matter wheter we apply the multiplier since 0 * x == 0
        var vel = rigidbody.velocity;
        vel.x *= turnMultiplier;
        rigidbody.velocity = vel;

        actor.transform.rotation = Quaternion.Euler(0, _moveDirection < 0 ? 180 : 0, 0);
    }

    public virtual object DuringHorizontal()
    {
        Vector2 velocity = rigidbody.velocity;
        float ratio = Mathf.Min(1, Mathf.Abs(velocity.x) / GetCurrentMaxMoveSpeed()); // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
        ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

        Vector2 force = GetCurrentMoveAccel() * ratio * moveDirection;

        rigidbody.AddForce(force);

        return new WaitForEndOfFrame();
    }

    public virtual void AfterHorizontal()
    {
        actor.RequestStaticDrag(this);
        actor.animator.SetBool("moving", false);
    }

    public void Slow(float x)
    {
        _speedModifier -= x;
    }

    public void SpeedUp(float x)
    {
        _speedModifier += x;
    }

}
