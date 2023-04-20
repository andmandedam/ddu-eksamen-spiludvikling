using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Serializable]
    private class PlayerMovement : Movement
    {
        private Player player;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _moveMaxSpeed;

        public override Entity entity => player;
        public override float moveAccel =>
                        _moveAcceleration * (player.grounded ? 1f : 0.5f);
        public override float moveMaxSpeed => _moveMaxSpeed;

        public void Enable(Player player)
        {
            this.player = player;
        }

        protected override void BeginMoveHorizontal()
        {
            player.animator.SetBool("running", true);
            base.BeginMoveHorizontal();
        }

        protected override void EndMoveHorizontal()
        {
            player.animator.SetBool("running", false);
            base.EndMoveHorizontal();
        }
    }

    [Serializable]
    private class PlayerJump : InteractiveJump
    {
        private Player player;
        [SerializeField] private int remainingJumps;
        [SerializeField] private int _jumpCount;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _minJumpTime;
        [SerializeField] private float _downForceScale;
        [SerializeField] private int _maxIteration;

        public override Entity entity => player;
        public override float jumpForce => _jumpForce;
        public override float minJumpTime => _minJumpTime;
        public override float downForceScale => _downForceScale;
        public override bool canJump => remainingJumps > 0;
        public override int maxIteration => _maxIteration;


        public void Enable(Player player)
        {
            this.player = player;
        }

        public void Reset()
        {
            remainingJumps = _jumpCount;
        }

        protected override void OnJump()
        {
            base.OnJump();
            remainingJumps--;
        }
    }

    [Serializable]
    private class PlayerCrouch : Crouch
    {
        private Player _player;
        public Animator animator => _player.animator;

        public void Enable(Player player)
        {
            _player = player;
        }

        public override void Start()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            animator.SetBool("crouching", true);
            _player.movement.Slow(10);
        }

        public override void End()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            animator.SetBool("crouching", false);
            _player.movement.Slow(-10);
        }
    }

    [Serializable]
    private class PlayerAttack : HitscanAttack
    {
        private Player _player;
        public override Animator animator => _player.animator;
        public override Entity entity => _player;
        public override Vector2 attackPoint
        {
            get
            {
                if (_player.movement.facingVector.y == 0)
                {
                    return _player.movement.facingVector + (Vector2)_player.transform.position;
                }
                else
                {
                    return 2 * _player.movement.facingVector + (Vector2)_player.transform.position;
                }
            }
        }
        public void Enable(Player player)
        {
            _player = player;
            base.Enable();
        }

        public override void OnWindup()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            _player.movement.End();
            onFoot.Move.Disable();
            onFoot.Jump.Disable();
            base.OnWindup();
        }

        public override void OnCooldown()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            onFoot.Move.Enable();
            onFoot.Jump.Enable();
            base.OnCooldown();
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
            var crouch = player.crouch;
            var attack = player.attack;

            var onFoot = actions.NinjaOnFoot;

            onFoot.Move.performed += (ctx) => movement.Begin(ctx.ReadValue<Vector2>());
            onFoot.Move.canceled += (ctx) => movement.End();
            onFoot.Jump.performed += (ctx) => jump.Begin();
            onFoot.Jump.canceled += (ctx) => jump.End();
            onFoot.Crouch.performed += (ctx) => crouch.Start();
            onFoot.Crouch.canceled += (ctx) => crouch.End();
            onFoot.Attack.performed += (ctx) => attack.Start();
        }
    }


    [Header("Player")]
    [SerializeField] private float _staticDrag;
    [SerializeField] private float _dynamicDrag;
    [SerializeField] private LayerMask _platformLayer;
    [SerializeField] private Animator _animator;

    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerJump jump;
    [SerializeField] private PlayerCrouch crouch;
    [SerializeField] private PlayerAttack attack;

    private PlayerControls controls = new();

    public override float staticDrag => _staticDrag;
    public override float dynamicDrag => _dynamicDrag;
    public Animator animator => _animator;

    public override LayerMask platformLayer => _platformLayer;


    void Start()
    {
        movement.Enable(this);
        controls.Enable(this);
        jump.Enable(this);
        attack.Enable(this);
        crouch.Enable(this);
    }

    void FixedUpdate()
    {
        if (grounded && !jump.jumping)
        {
            jump.Reset();
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        movement.OnTriggerEnter2D(collider);
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        movement.OnTriggerEnter2D(collider);
    }
}
