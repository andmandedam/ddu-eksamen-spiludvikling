using UnityEngine;

public abstract class InteractiveJump : Jump
{
    public abstract int maxIteration { get; }

    private int iteration = 0;
    private float yvel = 0;

    public override void JumpingEntry()
    {
        base.JumpingEntry();
        yvel = rigidbody.velocity.y;
    }

    public override object JumpingDuring()
    {
        if (iteration < maxIteration)
        {
            iteration++;
            var vel = rigidbody.velocity;
            vel.y = yvel;
            rigidbody.velocity = vel;

            return new WaitForFixedUpdate();
        }
        return base.JumpingDuring();
    }

    public override void JumpingExit()
    {
        base.JumpingExit();
        iteration = 0;
    }
}

