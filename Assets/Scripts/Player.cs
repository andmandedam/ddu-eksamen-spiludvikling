using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
                        _moveAcceleration * (player.jump.jumping ? 0.5f : 1f);
        public override float moveMaxSpeed => _moveMaxSpeed;
        public override float dynamicDrag => player.dynamicDrag;
        public override float staticDrag => !player.grounded ? player.dynamicDrag : player.staticDrag;

        public void Enable(Player player)
        {
            this.player = player;
        }
    }

    [Serializable]
    private class PlayerJump : Jump
    {
        private Player player;
        private int remainingJumps;
        [SerializeField] private int _jumpCount;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _minJumpHeigth;
        [SerializeField] private float _downForceScale;

        public override Entity entity => player;
        public override float jumpForce => _jumpForce;
        public override float minJumpHeigth => _minJumpHeigth;
        public override float downForceScale => _downForceScale;
        public override bool canJump => remainingJumps > 0;


        protected override void Reset()
        {
            remainingJumps = _jumpCount;
        }

        protected override void OnJump()
        {
            remainingJumps--;
        }

        public void Enable(Player player)
        {
            this.player = player;
        }
    }

    [Serializable]
    private class PlayerCrouch : Crouch
    {
        [SerializeField] private Animator animator;

        public override void Start()
        {
            Debug.Log("Crouching");
            animator.SetBool("crouching", true);
        }

        public override void End()
        {
            animator.SetBool("crouching", false);
        }
    }

    [Serializable]
    private class PlayerAttack : HitscanAttack
    {
        private Player _player;
        [SerializeField] Rect _hitRect;
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackKnockback;
        [SerializeField] private LayerMask _attackLayer;
        [SerializeField] private float _windupTime;
        [SerializeField] private float _attackTime;
        [SerializeField] private float _cooldownTime;

        public override Rect hitRect
        {
            get
            {
                var sign = _player.transform.rotation == Quaternion.identity ? 1 : -1;
                var pos = _hitRect.position;
                pos.x = sign * pos.x;
                pos = pos + (Vector2)_player.transform.position;

                return 
                new(
                    pos,
                    new Vector2(sign * _hitRect.width, _hitRect.height)
                    );
            }
        }
        public override int attackDamage => _attackDamage;
        public override float attackKnockback => _attackKnockback;
        public override LayerMask attackLayer => _attackLayer;
        public override Entity entity => _player;
        public override float windupTime => _windupTime;
        public override float attackTime => _attackTime;
        public override float cooldownTime => _cooldownTime;
    
        public void Enable(Player player)
        {
            _player = player;
        }

        public override void Start()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            _player.movement.End();
            onFoot.Move.Disable();
            onFoot.Jump.Disable();
            base.Start();
        }

        public override void End()
        {
            var onFoot = _player.controls.actions.NinjaOnFoot;
            onFoot.Move.Enable();
            onFoot.Jump.Enable();
            base.End();
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

            onFoot.Move.performed += (ctx) => movement.Start(ctx.ReadValue<Vector2>());
            onFoot.Move.canceled += (ctx) => movement.End();
            onFoot.Jump.performed += (ctx) => jump.Start();
            onFoot.Jump.canceled += (ctx) => jump.End();
            onFoot.Passthrough.performed += (ctx) =>
            {
                foreach (var trigger in player._passthroughTriggers)
                {
                    trigger.AllowPassthroughFor(player.bodyCollider);
                    trigger.AllowPassthroughFor(player.feetCollider);
                }
            };
            onFoot.Crouch.performed += (ctx) => crouch.Start();
            onFoot.Crouch.canceled += (ctx) => crouch.End();
            onFoot.Attack.performed += (ctx) => attack.Start();
        }
    }


    [Header("Player")]
    [SerializeField] private float _staticDrag;
    [SerializeField] private float _dynamicDrag;
    [SerializeField] private LayerMask _platformLayer;

    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerJump jump;
    [SerializeField] private PlayerCrouch crouch;
    [SerializeField] private PlayerAttack attack;
    
    private HashSet<PassthroughTrigger> _passthroughTriggers = new();
    private PlayerControls controls = new();

    public override float staticDrag => _staticDrag;
    public override float dynamicDrag => _dynamicDrag;


    public override LayerMask platformLayer => _platformLayer;


    void Start()
    {
        movement.Enable(this);
        controls.Enable(this);
        jump.Enable(this);
        attack.Enable(this);
    }

    void FixedUpdate()
    {
        jump.FixedUpdate();
        movement.FixedUpdate();
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            _passthroughTriggers.Add(trigger);
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out PassthroughTrigger trigger))
        {
            _passthroughTriggers.Remove(trigger);
        }
    }
}