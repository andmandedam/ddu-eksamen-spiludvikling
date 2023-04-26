using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack : Actor.Extension
{
    [Header("Attack")]
    [SerializeField] float _windupTime;
    [SerializeField] float _attackTime;
    [SerializeField] float _cooldownTime;

    private Actor _actor;
    private State _windup;
    private State _attack;
    private State _cooldown;
    private float _startTime;

    public override Actor actor => _actor;
    public State windup => _windup;
    public State attack => _attack;
    public State cooldown => _cooldown;

    public float windupTime => _windupTime;
    public float attackTime => _attackTime;
    public float cooldownTime => _cooldownTime;
    public bool isInWindup => current == windup;
    public bool isInAttack => current == attack;
    public bool isInCooldown => current == cooldown;

    public void Enable(Actor actor)
    {
        _actor = actor;
        _windup = new(OnWindup, DuringWindup, AfterWindup);
        _attack = new(OnAttack, DuringAttack, AfterAttack);
        _cooldown = new(OnCooldown, DuringCooldown, AfterCooldown);

        _windup.When(ExitWindup, attack);
        _attack.When(ExitAttack, cooldown);
        _cooldown.ExitWhen(ExitCooldown);
    }

    public void Begin()
    {
        if (!isInProgress)
        {
            Run(windup);
        }
    }

    public virtual void OnWindup()
    {
        _startTime = Time.time;
        actor.animator.SetInteger("attackState", 1);
    }
    public virtual object DuringWindup() => null;
    public virtual void AfterWindup() { }
    public virtual bool ExitWindup() => Time.time - _startTime > windupTime;

    public virtual void OnAttack()
    {
        _startTime = Time.time;
        actor.animator.SetInteger("attackState", 2);
    }

    public virtual object DuringAttack() => null;
    public virtual void AfterAttack() { }
    public virtual bool ExitAttack() => Time.time - _startTime > attackTime;

    public virtual void OnCooldown()
    {
        _startTime = Time.time;
        actor.animator.SetInteger("attackState", 0);
        // actor.animator.SetBool("attackOnCooldown", true);
    }
    public virtual object DuringCooldown() => null;
    public virtual void AfterCooldown()
    {
        // actor.animator.SetBool("attackOnCooldown", false);
    }
    public virtual bool ExitCooldown() => Time.time - _startTime > cooldownTime;
}
