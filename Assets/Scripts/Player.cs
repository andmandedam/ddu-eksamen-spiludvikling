using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Serializable]
    private class PlayerMovement : Movement
    {
        private Player player;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _moveMaxSpeed;
        public override float moveAcceleration =>
                        _moveAcceleration * (player.jump.jumping ? 0.5f : 1f);
        public override float moveMaxSpeed => _moveMaxSpeed;
        public override float dynamicDrag => player.dynamicDrag;
        public override float staticDrag => player.jump.jumping ? player.dynamicDrag : player.staticDrag;
        public override Rigidbody2D rigidbody => player.rigidbody;

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
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Collider2D footCollider;
        public float dynamicDrag => player.dynamicDrag;
        public float staticDrag => player.movement.moving ? player.dynamicDrag : player.staticDrag;
        public override float jumpForce => _jumpForce;
        public override float minJumpHeigth => _minJumpHeigth;
        public override float downForceScale => _downForceScale;
        public override bool canJump => remainingJumps > 0;
        public override Rigidbody2D rigidbody => player.rigidbody;


        private void Reset()
        {
            remainingJumps = _jumpCount;
            Debug.LogFormat("{0} set drag to {1}", this, rigidbody.drag);
        }

        public override void Start()
        {
            if (canJump)
            {
                remainingJumps--;
                rigidbody.drag = dynamicDrag;
                Debug.LogFormat("{0} set drag to {1}", this, rigidbody.drag); 
                DoJump();
            }
        }

        
        // Poor implementation of multiple jumps.
        // Currently the players jumps get reset whenever they touch the ground layer
        // Potentially the players jumps will get reset even though their feet aren't touching the ground
        // FIX:
        // 		Seperate colliders for player feet?
        public override void FixedUpdate()
        {
            if (footCollider.IsTouchingLayers(_groundLayer))
            {
                Reset();
            }
            base.FixedUpdate();
        }

        public void Enable(Player player)
        {
            this.player = player;
        }
    }

    private class PlayerControls : Controls
    {
        public void Enable(Player player)
        {
            PlayerInputActions actions = new PlayerInputActions();
            actions.Enable();

            var movement = player.movement;
            var jump = player.jump;

            var onFoot = actions.NinjaOnFoot;

            onFoot.Move.performed += (ctx) => movement.Start(ctx.ReadValue<Vector2>());
            onFoot.Move.canceled += (ctx) => movement.End();
            onFoot.Jump.performed += (ctx) => jump.Start();
            onFoot.Jump.canceled += (ctx) => jump.End();
        }
    }


    [SerializeField] private float staticDrag;
    [SerializeField] private float dynamicDrag;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerJump jump;
    private PlayerControls controls = new();
    new public Rigidbody2D rigidbody { get; private set; }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        movement.Enable(this);
        controls.Enable(this);
        jump.Enable(this);
    }

    void FixedUpdate()
    {
        jump.FixedUpdate();
        movement.FixedUpdate();

   
    }

    public void Damage(int dmg)
    {
        throw new System.NotImplementedException();
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }
}

//public class Sword : MonoBehaviour
//{
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        collision.gameObject.GetComponent<IDamageable>().Damage(10);
//    }
//}
