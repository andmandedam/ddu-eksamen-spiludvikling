using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crouch
{
    public abstract Entity entity { get; }

    private State crouchingState;
    private State.Machine machine;

    public virtual void Enable()
    {
        machine = new(entity);
        crouchingState = new(CrouchEntry, CrouchExit, CrouchDuring);
    }

    public void Begin()
    {
        machine.Run(crouchingState);
    }

    public void End()
    {
        machine.Abort();
    }

    public virtual void CrouchEntry() { }
    public virtual object CrouchDuring() => null;
    public virtual void CrouchExit() { }
}
