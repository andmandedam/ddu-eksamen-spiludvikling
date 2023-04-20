using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public enum State
    {
        Ready = 0,
        Windup = 1,
        Attack = 2,
        Cooldown = 3
    };

    public static int StateAsInt(State state) => (int)state;

    public abstract Entity entity { get; }
    public abstract float windupTime { get; }
    public abstract float attackTime { get; }
    public abstract float cooldownTime { get; }
    public abstract Animator animator { get; }

    public bool isReady => state == State.Ready;
    public bool isInWindup => state == State.Windup;
    public bool isAttacking => state == State.Attack;
    public bool isOnCooldown => state == State.Cooldown;
    public State state => _state;

    private Coroutine currentRoutine = null;
    private State _state = State.Ready;

    public virtual void OnReady() => animator.SetInteger("attackState", StateAsInt(State.Ready));
    public virtual void OnWindup() => animator.SetInteger("attackState", StateAsInt(State.Windup));
    public virtual void OnAttack() => animator.SetInteger("attackState", StateAsInt(State.Attack));
    public virtual void OnCooldown() => animator.SetInteger("attackState", StateAsInt(State.Cooldown));

    public virtual object DuringWindup() => null;
    public virtual object DuringAttack() => null;
    public virtual object DuringCooldown() => null;

    public virtual void Enable()
    {
        animator.SetFloat("windupTime", 1 / windupTime);
        animator.SetFloat("attackTime", 1 / attackTime);
        animator.SetFloat("cooldownTime", 1 / cooldownTime);
    }

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
        _state = State.Windup;
        OnWindup();
        var enumerator = Util.TimedRoutine(windupTime, DuringWindup);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _state = State.Attack;
        OnAttack();
        var enumerator = Util.TimedRoutine(attackTime, DuringAttack);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        _state = State.Cooldown;
        OnCooldown();
        var enumerator = Util.TimedRoutine(cooldownTime, DuringCooldown);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = null;
        _state = State.Ready;
        End();
        OnReady();
    }
}
