using UnityEngine;

    public abstract class InteractiveJump : Jump
{
    public abstract int maxIteration { get; }
    public abstract float downForce { get; }

    [SerializeField] private int iteration = 0;
    [SerializeField] private float yvel = 0;

    public override bool JumpShouldEnd()
    {
        return base.JumpShouldEnd() || iteration == maxIteration;
    }

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
        rigidbody.AddForce(Vector2.down * downForce, ForceMode2D.Impulse);
        iteration = 0;
    }
}

