 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NinjaMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isGrounded;
    [SerializeField]private float jumpForce = 5;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("JUMP! " + context);

        if (context.phase.IsInProgress() && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }


    }


    public void Move(InputAction.CallbackContext context)
    {

    }

    public void Dash(InputAction.CallbackContext context)
    {

    }
}
