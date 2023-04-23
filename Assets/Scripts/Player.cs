using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Serializable]
    private class PlayerJump : Jump
    {
        [SerializeField] int _jumpCount;
        int _remainingJumps;

        private Player player => actor as Player;
        public override bool canJump => _remainingJumps > 0;

        public void Reset()
        {
            _remainingJumps = _jumpCount;
        }

        public override void OnWindup()
        {
            base.OnWindup();
            // Debug.Log("OnWindup");
        }

        public override void AfterWindup()
        {
            base.AfterWindup();
            // Debug.Log("AfterWindup");
        }

        public override void OnJump()
        {
            base.OnJump();
            _remainingJumps--;
            // Debug.Log("OnJump");
        }

        public override void AfterJump()
        {
            base.AfterJump();
            // Debug.Log("AfterJump");
        }
    }

    [Serializable]
    private class PlayerAttack : HitscanAttack
    {
        public override Vector2 attackPoint => (actor.facing.y == 0 ? 1 : 2) * actor.facing + (Vector2)transform.position;
        private Player player => actor as Player;

        public override void OnWindup()
        {
            base.OnWindup();
            var onFoot = player.controls.actions.NinjaOnFoot;
            player.movement.End();
            onFoot.Move.Disable();
            onFoot.Jump.Disable();
        }

        public override void AfterAttack()
        {
            base.AfterAttack();
            var onFoot = player.controls.actions.NinjaOnFoot;
            onFoot.Move.Enable();
            onFoot.Jump.Enable();
        }
    }

    private class PlayerControls : Controls
    {
        public PlayerInputActions actions;
        public void Enable(Player player)

        {
            actions = new PlayerInputActions();
            actions.Enable();

            var movement = player.movement;
            var jump = player.jump;
            var attack = player.attack;

            var onFoot = actions.NinjaOnFoot;

            onFoot.Move.performed += (ctx) =>
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                input.Round();
                player._facing = input;
                switch (input.y)
                {
                    case (-1): player.Passthrough(); break;
                    case (0): movement.Begin((int)input.x); break;
                    case (1): player.animator.SetBool("lookUp", true); break;
                }
            };
            onFoot.Move.canceled += (ctx) =>
            {
                player.animator.SetBool("lookUp", false);
                movement.End();
            };
            onFoot.Jump.performed += (ctx) => jump.Begin();
            onFoot.Jump.canceled += (ctx) => jump.End();
            onFoot.Crouch.performed += (ctx) =>
            {
                if (player.movement.isInProgress && player.movement.HasAccelerated())
                {
                    player.rigidbody.AddForce(movement.moveDirection * player._crouchDashForce, ForceMode2D.Impulse);
                }

                player.movement.Slow(100);
                player.animator.SetBool("crouch", true);
            };
            onFoot.Crouch.canceled += (ctx) =>
            {
                player.movement.SpeedUp(100);
                player.animator.SetBool("crouch", false);
            };
            onFoot.Attack.performed += (ctx) => attack.Begin();
        }
    }

    [Header("Player")]
    [SerializeField] private Movement _movement;
    [SerializeField] private PlayerJump _jump;
    [SerializeField] private PlayerAttack _attack;
    [SerializeField] private float _crouchDashForce;

    private Vector2 _facing;

    public Movement movement => _movement;
    public Jump jump => _jump;
    public Attack attack => _attack;
    public override Vector2 facing => _facing;
    private PlayerControls controls = new();

    void Start()
    {
        controls.Enable(this);
        _movement.Enable(this);
        _jump.Enable(this);
        _attack.Enable(this);
    }

    void Update()
    {
        if (grounded)
        {
            // Grounded
            if (!jump.isInProgress) _jump.Reset();
        }
        else if (falling)
        {
            // Airborne, going downwards
        }
        else
        {
            // Airborne, going upwards

        }
    }
}
