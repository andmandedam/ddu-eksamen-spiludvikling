using UnityEngine;

public abstract class InteractiveJump : Jump
{
    public abstract int maxIteration { get; }

    [SerializeField] private int iteration = 0;
    [SerializeField] private float yvel = 0;

    protected override void OnJump()
    {
        base.OnJump();
        yvel = rigidbody.velocity.y;
    }

    protected override object DuringJump()
    {
        if (iteration < maxIteration)
        {
            iteration++;
            var vel = rigidbody.velocity;
            vel.y = yvel;
            rigidbody.velocity = vel;

            return new WaitForFixedUpdate();
        }
        return null;
    }

    protected override void EndJump()
    {
        iteration = 0;
    }
}

