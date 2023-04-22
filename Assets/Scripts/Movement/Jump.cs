using System;
using UnityEngine;

[Serializable]
public class Jump : Actor.Extension
{
    // Customization fields
    [Header("Jump")]
    [SerializeField] float _jumpForce;
    [SerializeField] float _downForce;
    [SerializeField] float _minWindupTime;
    [SerializeField] float _maxWindupTime;
    [SerializeField] float _minJumpTime;
    [SerializeField] int _maxIteration;

    // State fields
    private Actor _actor;
    private State _windup;
    private State _jump;
    private float _windupStartTime = 0;
    private float _jumpStartTime = 0;
    private bool _endRequest = false;
    private int _currentIteration = 0;
    private float _yVelocity = 0;

    public override Actor actor => _actor;
    public State windup => _windup;
    public State jump => _jump;
    public bool isInWindup => current == windup;
    public bool isInJump => current == jump;

    //  public int maxIteration => _maxIteration;
    public float jumpForce => _jumpForce;
    public float downForce => _downForce;
    public float minWindupTime => _minWindupTime;
    public float maxWindupTime => _maxWindupTime;
    public float minJumpTime => _minJumpTime;

    public virtual bool canJump => actor.grounded;

    public void Enable(Actor actor)
    {
        _actor = actor;
        _windup = new(OnWindup, DuringWindup, AfterWindup);
        _jump = new(OnJump, DuringJump, AfterJump);

        _windup.When(ExitWindup, jump);
        _jump.ExitWhen(ExitJump);
    }

    public void Begin()
    {
        if (canJump)
        {
            _endRequest = false;
            if (actor.grounded)
            {
                Run(windup);
            }
            else
            {
                Run(jump);
            }
        }
    }

    public void End()
    {
        _endRequest = true;
    }

    public virtual void OnWindup()
    {
        _windupStartTime = Time.time;
        actor.animator.SetBool("jumpWindup", true);
    }

    public virtual object DuringWindup() => null;
    public virtual void AfterWindup()
    {
        actor.animator.SetBool("jumpWindup", false);
    }
    public virtual bool ExitWindup()
    {
        float passed = Time.time - _windupStartTime;
        bool minPassed = passed > minWindupTime;
        bool maxPassed = passed > maxWindupTime;
        return maxPassed || (minPassed && _endRequest);
    }

    public virtual void OnJump()
    {
        _jumpStartTime = Time.time;
        _currentIteration = 0;

        actor.RequestDynamicDrag(this);

        var velocity = rigidbody.velocity;
        velocity.y = 0;
        rigidbody.velocity = velocity;

        rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        _yVelocity = rigidbody.velocity.y;
    }

    public virtual object DuringJump()
    {
        if (_currentIteration < _maxIteration)
        {
            _currentIteration++;

            var vel = rigidbody.velocity;
            vel.y = _yVelocity;
            rigidbody.velocity = vel;
        }
        return new WaitForEndOfFrame();
    }

    public virtual void AfterJump()
    {
        rigidbody.AddForce(downForce * Vector2.down, ForceMode2D.Impulse);
        actor.RequestStaticDrag(this);
    }

    public virtual bool ExitJump()
    {
        bool minPassed = Time.time - _jumpStartTime > minJumpTime;
        bool falling = actor.falling;
        bool doneIterating = _currentIteration == _maxIteration;
        return minPassed && (doneIterating || falling || _endRequest);
    }
}
