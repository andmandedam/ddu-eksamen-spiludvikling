using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSidescrollerMovement : MonoBehaviour
{
    private Rigidbody2D _playerRigidBody;
    private PlayerInputActions _playerInputActions;

    [Header("Movement Variables")]
    [Tooltip("The higher this number, the faster the player reaches his maximum speed.")]
    [SerializeField, Min(0.1f)]
    private float movementAcceleration = 15f;

    [Tooltip("The maximum speed of the player.")]
    [SerializeField, Min(0.1f)]
    private float maxMovementSpeed = 40f;

    [Tooltip(
        "The smaller the number, the longer the player slides after not pressing any button to move. (Ice glide effect)")]
    [SerializeField]
    private float linearDrag = 15f;

    private float _horizontalDirection;

    private bool _changingDirection => (_playerRigidBody.velocity.x > 0f && _horizontalDirection < 0f) ||
                                       (_playerRigidBody.velocity.x < 0f && _horizontalDirection > 0f);

    [Space]
    [Header("Jump Variables")]
    [SerializeField, Min(0.1f)]
    private float jumpForce = 10f;

    [SerializeField, Min(0.1f)] private float fallMultiplier = 3f;
    [SerializeField, Min(0.1f)] private float lowJumpFallMultiplier = 1f;
    [SerializeField, Min(0.1f)] private float airLinearDrag = 1f;
    [Tooltip("The amount of times the player can midair")]
    [SerializeField, Min(0)] private int extraJumps = 0;
    private int _remainingJumps;

    [Space]
    [Header("Layer Mask")]
    [Tooltip("The Layer, on which the player's jump resets")]
    [SerializeField]
    private LayerMask groundLayer;

    private RaycastHit2D _hit;

    [Space]
    [Header("Ground Collision Variables")]
    [SerializeField, Min(0.1f)]
    private float groundRaycastLength = 0.5f;

    [Tooltip("Creates two raycasts, used to create a raycast on the left and right side of the player.")]
    [SerializeField]
    private Vector3 groundRaycastOffset;

    private bool _onGround;

    
    [Space]
    [Header("Debug")]
    [SerializeField]
    private bool showRaycastLength = false;

    private void Start()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalDirection = _playerInputActions.NinjaOnFoot.Move.ReadValue<Vector2>().x;
        CheckCollisions();
        if (_playerInputActions.NinjaOnFoot.Jump.WasPerformedThisFrame() && (_onGround || _remainingJumps > 0))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        if (_onGround)
        {
            _remainingJumps = extraJumps;
            ApplyLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMulti();
        }
    }
    
    private void MoveCharacter()
    {
        float _quickAccelerateFactor = (1.2f - Mathf.Abs(_playerRigidBody.velocity.x) / maxMovementSpeed);
        _playerRigidBody.AddForce(new Vector2(_horizontalDirection, 0f) * movementAcceleration * _quickAccelerateFactor);

    }

    private void ApplyLinearDrag()
    {
        _playerRigidBody.gravityScale = 1f;
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _playerRigidBody.drag = linearDrag;
        }
        else
        {
            _playerRigidBody.drag = 0f;
        }
    }

    private void ApplyAirLinearDrag()
    {
        _playerRigidBody.drag = airLinearDrag;
    }

    private void FallMulti()
    {
        if (_playerRigidBody.velocity.y < 0)
        {
            _playerRigidBody.gravityScale = fallMultiplier;
        }
        else if (_playerRigidBody.velocity.y > 0 && _playerInputActions.NinjaOnFoot.Jump.WasPerformedThisFrame())
        {
            _playerRigidBody.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            _playerRigidBody.gravityScale = 1f;
        }
    }

    private void Jump()
    {
        if (!_onGround)
        {
            --_remainingJumps;
        }

        _playerRigidBody.velocity = new Vector2(_playerRigidBody.velocity.x, 0f);
        _playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

    }

    private void CheckCollisions()
    {
        _onGround = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);
                    //|| Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer); No idea why this is here
    }

    private void OnDrawGizmos()
    {
        if (showRaycastLength)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position + groundRaycastOffset,
                transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
            /*Gizmos.DrawLine(transform.position - groundRaycastOffset,
                transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);*/
        }
    }
}