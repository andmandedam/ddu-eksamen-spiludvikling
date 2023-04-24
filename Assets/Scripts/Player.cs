using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Serializable]
    public class PlayerJump : Jump
    {
        public int jumpCount;
        int _remainingJumps;

        private Player player => actor as Player;
        public override bool canJump => _remainingJumps > 0;

        public void Reset()
        {
            _remainingJumps = jumpCount;
        }

        public override void OnWindup()
        {
            base.OnWindup();
            AudioManager.instance.PlaySound("NinjaJump");
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
    public class PlayerAttack : HitscanAttack
    {
        public override Vector2 attackPoint => (actor.facing.y == 0 ? 1 : 2) * actor.facing + (Vector2)transform.position;
        private Player player => actor as Player;

        public override void OnWindup()
        {
            base.OnWindup();
            // AudioManager.instance.PlaySound("Windup");
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

    public class PlayerControls : Controls
    {
        public PlayerInputActions actions;
        private float lookUpAmount = 5;

        public void Initialize()
        {
            actions = new PlayerInputActions();
            actions.Enable();
        }

        public void Enable(Player player)
        {
            var movement = player.movement;
            var jump = player.jump;
            var attack = player.attack;

            var onFoot = actions.NinjaOnFoot;

            onFoot.Move.performed += (ctx) =>
            {
                Vector2 input = ctx.ReadValue<Vector2>();
                input.Round();
                player._facing = input;
                movement.Begin((int)input.x);
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

            onFoot.Passthrough.performed += player.Passthrough;

            onFoot.LookUp.performed += (ctx) => { LookUp(ctx); player.animator.SetBool("lookUp", true); };
            onFoot.LookUp.canceled += (ctx) => { LookUp(ctx); player.animator.SetBool("lookUp", false); };
        }

        private void LookUp(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            Debug.Log("LookUp" + ctx);
            if (ctx.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                GameManager.instance.mainCameraScript.cameraOffset.y = 0;
            }
            else
            {
                GameManager.instance.mainCameraScript.cameraOffset.y = lookUpAmount;
            }
        }
    }

    [Header("Player")]
    [SerializeField] private Movement _movement;
    public PlayerJump _jump;
    [SerializeField] private PlayerAttack _attack;
    [SerializeField] private float _crouchDashForce;

    private Vector2 _facing;

    public Movement movement => _movement;
    public PlayerJump jump => _jump;
    public PlayerAttack attack => _attack;
    public override Vector2 facing => _facing;

    public PlayerControls controls;
        private float lookUpAmount;

        void Start()
    {
        var persistantObject = FindObjectOfType<PersistantObject>();
        if (persistantObject.playerControls == null)
        {
            persistantObject.playerControls = new PlayerControls();
            persistantObject.playerControls.Initialize();
        }
        controls = persistantObject.playerControls;
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

    public override void OnLand()
    {
        _jump.Reset();
            AudioManager.instance.PlaySound("NinjaLand");
    }
    public override void Damage(Entity source, int damage)
    {
        base.Damage(source, damage);
        AudioManager.instance.PlaySound("NinjaTakeDamage");
        UIHealth.instance.UpdateHealth();
    }

    public override void Die()
    {
        base.Die();
        InventoryUI.instance.DisplayHUDText("You final score was: " + ScoreUI.instance.score, float.PositiveInfinity);
    }
}
