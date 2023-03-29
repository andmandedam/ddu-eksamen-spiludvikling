using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement), typeof(Jump))]
public class PlayerController : MonoBehaviour
{
    public void Start()
    {
        PlayerInputActions actions = new PlayerInputActions();
        actions.Enable();

        var movement = GetComponent<Movement>();
        var jump = GetComponent<Jump>();

        var onFoot = actions.NinjaOnFoot;
        onFoot.Move.performed += (ctx) => movement.MoveStart(ctx.ReadValue<Vector2>());
        onFoot.Move.canceled += (ctx) => movement.MoveEnd();
        onFoot.Jump.performed += (ctx) => jump.JumpStart();
        onFoot.Jump.canceled += (ctx) => jump.JumpEnd();
    }

}
