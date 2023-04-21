using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public abstract Entity entity { get; }
    public abstract Animator animator { get; }

    public abstract float windupTime { get; }
    public abstract float attackTime { get; }
    public abstract float cooldownTime { get; }

    // public abstract WindupHandler windupHandler { get; }
    // public abstract AttackHandler attackHandler { get; }
    // public abstract CooldownHandler cooldownHandler { get; }

    public bool isRunning => machine.running;
    public bool isInWindup => machine.current == windupState;
    public bool isAttacking => machine.current == attackState;
    public bool isOnCooldown => machine.current == cooldownState;

    private State.Machine machine;
    private State windupState;
    private State attackState;
    private State cooldownState;

    public virtual void WindupEntry() { }
    public virtual object WindupDuring() => null;
    public virtual void WindupExit() { }
    public virtual void AttackEntry() { }
    public virtual object AttackDuring() => null;
    public virtual void AttackExit() { }
    public virtual void CooldownEntry() { }
    public virtual object CooldownDuring() => null;
    public virtual void CooldownExit() { }

    public virtual void Enable()
    {
        machine = new(entity);
        windupState = new(WindupEntry, WindupExit, WindupDuring);
        attackState = new(AttackEntry, AttackExit, AttackDuring);
        cooldownState = new(CooldownEntry, CooldownExit, CooldownDuring);

        windupState.After(windupTime, attackState);
        attackState.After(attackTime, cooldownState);
        cooldownState.After(cooldownTime, null);

        // animator.SetFloat("windupTime", 1 / windupHandler.duration);
        // animator.SetFloat("attackTime", 1 / attackHandler.duration);
        // animator.SetFloat("cooldownTime", 1 / cooldownHandler.duration);
        // stateManager.Relax(State.Relaxed)
        //             .Add(windupHandler)
        //             .Add(attackHandler)
        //             .Add(cooldownHandler);
    }

    public virtual void Start()
    {
        if (!machine.running)
        {
            if (!entity.grounded)
            {
                entity.rigidbody.gravityScale = 0;
                entity.rigidbody.velocity = Vector3.zero;
            }
            machine.Run(windupState);
        }
    }

    public virtual void End()
    {
        // entity.rigidbody.gravityScale = entity.grav;
        machine.Abort();
    }
}
