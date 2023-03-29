using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    new private Rigidbody2D rigidbody;

    [Header("Horisontal movement")]
    [SerializeField]
    protected Vector2 movementVector;
    [SerializeField]
    protected float acceleration;
    [SerializeField, Min(0.1f)]
    protected float maxSpeed;
    [SerializeField]
    protected float linearDrag;

    // [SerializeField, Min(0)]
    // int extraJumps;
    // int remainingJumps;

    //[Space, Header("Ground")]
    //[SerializeField, Tooltip("The ground layer")]
    //LayerMask groundLayer;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.drag = linearDrag;
    }

    public void FixedUpdate()
    {
        if (movementVector != Vector2.zero)
        {
            Vector2 velocity = rigidbody.velocity;
            float ratio = Mathf.Abs(velocity.x) / maxSpeed; // ratio in [0, 1], 0 => speed = 0; 1 => speed = maxSpeed;
            ratio = 1 - ratio; // Mirror ratio interval, ratio now in [0, 1], 0 => speed = maxSpeed; 1 => speed = 0;

            Vector2 force = acceleration * ratio * movementVector;

            rigidbody.AddForce(force);
        }
    }

    public void MoveStart(Vector2 dir)
    {
        movementVector = dir;
    }

    public void MoveEnd()
    {
        movementVector = Vector2.zero;
    }
}
