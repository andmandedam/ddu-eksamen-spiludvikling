using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : IPhysical
{
    [Header("Horisontal movement")]
    protected float speed;
    [SerializeField]
    protected float acceleration = 15f;
    [SerializeField]
    protected float maxSpeed = 40f;

    [Space, Header("Vertical movement")]
    [SerializeField]
    protected float jumpForce;
    [SerializeField, Min(0)]
    protected int extraJumps = 0;
    protected int remainingJumps;

    [Space, Header("Ground")]
    [SerializeField, Tooltip("The ")]
    private LayerMask groundLayer;

    public abstract Rigidbody2D GetRigidbody();

    public void Move(float x)
    {
        var rigidbody = GetRigidbody();
        var velocity = rigidbody.velocity;
        float ratio = Mathf.Abs(velocity.x) / maxSpeed; // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed

    }

}
