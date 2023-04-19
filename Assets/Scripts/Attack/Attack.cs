using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    enum State
    {
        Ready,
        Windup,
        Attack,
        Cooldown
    };

    public abstract Entity entity { get; }
    public abstract float windupTime { get; }
    public abstract float attackTime { get; }
    public abstract float cooldownTime { get; }

    public bool isReady => state == State.Ready;
    public bool isInWindup => state == State.Windup;
    public bool isAttacking => state == State.Attack;
    public bool isOnCooldown => state == State.Cooldown;

    private Coroutine currentRoutine = null;
    private State state = State.Ready;

    public virtual void OnReady() { }
    public virtual void OnWindup() { }
    public virtual void OnAttack() { }
    public virtual void OnCooldown() { }

    public virtual object DuringWindup() => null;
    public virtual object DuringAttack() => null;
    public virtual object DuringCooldown() => null;

    public virtual void Start()
    {
        if (isReady)
        {
            if (!entity.grounded)
            {
                entity.rigidbody.gravityScale = 0;
                entity.rigidbody.velocity = Vector3.zero;
            }

            entity.StartCoroutine(WindupRoutine());
        }
    }

    public virtual void Cancel()
    {
        if (entity != null)
        {
            entity.StopCoroutine(currentRoutine);
        }
    }

    public virtual void End()
    {
        entity.rigidbody.gravityScale = entity.grav;
    }

    private IEnumerator WindupRoutine()
    {
        state = State.Windup;
        OnWindup();
        var enumerator = Util.TimedRoutine(windupTime, DuringWindup);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        state = State.Attack;
        OnAttack();
        var enumerator = Util.TimedRoutine(attackTime, DuringAttack);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        state = State.Cooldown;
        OnCooldown();
        var enumerator = Util.TimedRoutine(cooldownTime, DuringCooldown);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = null;
        state = State.Ready;
        End();
        OnReady();
    }
}
