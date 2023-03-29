using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    new private Rigidbody2D rigidbody;

    private float initialHeigth;
    private bool jumping;

    [SerializeField]
    protected float minJumpHeigth;
    [SerializeField]
    protected float jumpForce;
    [SerializeField]
    protected float downForceScale;


    private void ResetVerticalVelocity() => rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
    private float GetHeigthDelta() => Mathf.Abs(transform.position.y - initialHeigth);
    private bool CanJump() => true;
    private bool CanEndJump() => minJumpHeigth < GetHeigthDelta();

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void JumpStart()
    {
        if (CanJump())
        {
            initialHeigth = transform.position.y;
            jumping = true;

            ResetVerticalVelocity();
            rigidbody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
        }   
    }

    private void Update()
    {
        if ((!jumping && CanEndJump()) || rigidbody.velocity.y < 0)
        {
            JumpForceEnd();
        }
    }

    public void JumpEnd()
    {
        jumping = false;
    }

    private void JumpForceEnd()
    {
        ResetVerticalVelocity();
        float deltaH = GetHeigthDelta();
        rigidbody.AddForce(downForceScale * deltaH * jumpForce * Vector2.down);
        
        jumping = false;
    }
}
