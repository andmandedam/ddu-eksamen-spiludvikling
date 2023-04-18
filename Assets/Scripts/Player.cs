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


    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerJump jump;
    [SerializeField] private Collider2D _feetCollider;
    [SerializeField] private float _staticDrag;
    [SerializeField] private float _dynamicDrag;
    [SerializeField] private LayerMask _platformLayer;

    private PlayerControls controls = new();
    
    public override Collider2D feetCollider => _feetCollider;
    public override float staticDrag => _staticDrag;
    public override float dynamicDrag => _dynamicDrag;
    public override LayerMask platformLayer => _platformLayer;

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
