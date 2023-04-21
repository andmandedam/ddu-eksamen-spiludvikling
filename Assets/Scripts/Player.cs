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
        public override float moveAccel => _moveAcceleration * (player.grounded ? 1f : 0.5f);
        public override float moveMaxSpeed => _moveMaxSpeed;

        public void Enable(Player player)
        {
            this.player = player;
            base.Enable();
        }

        public override void HorizontalEntry()
        {
            base.HorizontalEntry();
            player.animator.SetBool("running", true);
        }

        public override void HorizontalExit()
        {
            base.HorizontalExit();
            player.animator.SetBool("running", false);
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
        [SerializeField] private float _windupTime;
        [SerializeField] private float _downForce;
        [SerializeField] private int _maxIteration;

        public override Entity entity => player;
        public override float jumpForce => _jumpForce;
        public override float windupTime => _windupTime;
        public override bool canJump => remainingJumps > 0;
        public override float downForce => _downForce;
        public override float minJumpTime => _minJumpTime;
        public override int maxIteration => _maxIteration;


        public void Enable(Player player)
        {
            this.player = player;
            base.Enable();
        }

        public void Reset()
        {
            remainingJumps = _jumpCount;
        }

        public override void WindupEntry()
        {
            Debug.Log("WindupEntry");
            base.WindupEntry();
        }

        public override void WindupExit()
        {
            Debug.Log("WindupExit");
            base.WindupExit();
        }

        public override void JumpingEntry()
        {
            Debug.Log("JumpingEntry");
            base.JumpingEntry();
            remainingJumps--;
        }

        public override void JumpingExit()
        {
            base.JumpingExit();
            Debug.Log("JumpingExit");
            Debug.Log("Heigth" + player.transform.position);
        }
    }

    [Serializable]
    private class PlayerCrouch : Crouch
    {
        private Player _player;

        public override Entity entity => _player;
        public Animator animator => _player.animator;

        public void Enable(Player player)
        {
            _player = player;
        }

        public override void CrouchEntry()
        {
            base.CrouchEntry();
            animator.SetBool("crouching", true);
            _player.movement.Slow(10);
        }

        public override void CrouchExit()
        {
            base.CrouchExit();
            animator.SetBool("crouching", false);
            _player.movement.Slow(-10);
        }
    }

    [Serializable]
    private class PlayerAttack : HitscanAttack
    {
        Player player;

        public Animator animator => player.animator;
        public override Entity entity => player;
        public override Vector2 attackPoint
        {
            get
            {
                if (player.movement.facingVector.y == 0)
                {
                    return player.movement.facingVector + (Vector2)player.transform.position;
                }
                else
                {
                    return 2 * player.movement.facingVector + (Vector2)player.transform.position;
                }
            }
        }

        public void Enable(Player player)
        {
            this.player = player;
            base.Enable();
        }

        public override void WindupEntry()
        {
            base.WindupEntry();
            var onFoot = player.controls.actions.NinjaOnFoot;
            player.movement.End();
            onFoot.Move.Disable();
            onFoot.Jump.Disable();
        }

        public override void CooldownEntry()
        {
            base.CooldownEntry();
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
            var crouch = player.crouch;
            var attack = player.attack;

            var onFoot = actions.NinjaOnFoot;

            onFoot.Move.performed += (ctx) => movement.Begin(ctx.ReadValue<Vector2>());
            onFoot.Move.canceled += (ctx) => movement.End();
            onFoot.Jump.performed += (ctx) => jump.Begin();
            onFoot.Jump.canceled += (ctx) => jump.End();
            onFoot.Crouch.performed += (ctx) => crouch.Begin();
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
            if (grounded && !jump.isRunning)
        {
            jump.Reset();
        }
        // animator.SetBool("grounded", grounded);
        // animator.SetBool("falling", rigidbody.velocity.y < 0);
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
