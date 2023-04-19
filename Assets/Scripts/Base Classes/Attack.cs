using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack 
{
    public enum Phase
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
    public Phase state { get; private set; }

    public bool isReady => state == Phase.Ready;
    public bool isInWindup => state == Phase.Windup;
    public bool isAttacking => state == Phase.Attack;
    public bool isOnCooldown => state == Phase.Cooldown;
    
    private Coroutine currentRoutine = null;

    public virtual void OnReady() { }
    public virtual void OnWindup() { }
    public virtual void OnAttack() { }
    public virtual void OnCooldown() { }

    public virtual CustomYieldInstruction DuringWindup() => null;
    public virtual CustomYieldInstruction DuringAttack() => null;
    public virtual CustomYieldInstruction DuringCooldown() => null;

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

    private IEnumerator TimedCoroutine(float time, Func<CustomYieldInstruction> during)
    {
        float start = Time.time;
        Func<bool> timedout = () => Time.time - start >= time / 1000;
        while(!timedout())
        {
            var x = during();
            if (x == null) yield return new WaitUntil(timedout);
            else yield return x;
        }
    }

    private IEnumerator WindupRoutine()
    {
        state = Phase.Windup;
        OnWindup();
        var enumerator = TimedCoroutine(windupTime, DuringWindup);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(AttackRoutine()); 
    }

    private IEnumerator AttackRoutine()
    {
        state = Phase.Attack;
        OnAttack();
        var enumerator = TimedCoroutine(attackTime, DuringAttack);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = entity.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        state = Phase.Cooldown;
        OnCooldown();
        var enumerator = TimedCoroutine(cooldownTime, DuringCooldown);
        while (enumerator.MoveNext()) yield return enumerator.Current;
        currentRoutine = null;
        state = Phase.Ready;
        End();
        OnReady();
    }
}
