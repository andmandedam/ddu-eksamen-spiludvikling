//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;


//public class Player : 
//    MonoBehaviour,
//    IKillable,
//    IDamageable,
//    IPhysical,
//    IPickupper
//{
//    [SerializeField] PlayerHealth health;
//    [SerializeField] PlayerMovement movement;
//    [SerializeField] PlayerInventory inventory;
//    [SerializeField] PlayerController controls;
                                
//    new Rigidbody2D rigidbody;

//    [Serializable]
//    class PlayerHealth : Health
//    {
//        public PlayerHealth(Player player)
//        {
//            //
//        }
//    }

//    [Serializable]
//    class PlayerMovement : Movement
//    {
//        Player player;

//        public PlayerMovement(Player player) : base(acceleration: 20, maxSpeed: 20)
//        {
//            this.player = player;
//        }

//        public override Rigidbody2D GetRigidbody() => player.GetRigidbody();
//    }

//    [Serializable]
//    class PlayerInventory : Inventory
//    {
//        public PlayerInventory(Player player)
//        {

//        }

//        // impl
//        void OpenInventory()
//        {

//        }
//    }

//    class PlayerController : Controller
//    {
//        PlayerInputActions actions;
//        public PlayerController(Player player)
//        {
//            actions = new PlayerInputActions();
//            actions.Enable();
//        }
            
//        public void FixedUpdate(Player player)
//        {
//            var onFoot = actions.NinjaOnFoot;
//            if (onFoot.enabled) OnFootHandler(player, onFoot);
//        }

//        public void OnFootHandler(Player player, PlayerInputActions.NinjaOnFootActions onFoot)
//        {
//            if (onFoot.Move.inProgress)
//            {
//                player.movement.Move(onFoot.Move.ReadValue<Vector2>());
//            }
//        }
//    }

//    void Start()
//    {
//        rigidbody = GetComponent<Rigidbody2D>();

//        health = new PlayerHealth(this);
//        movement = new PlayerMovement(this);
//        controls = new PlayerController(this);
//        inventory = new PlayerInventory(this);
//    }

//    void FixedUpdate()
//    {
//        controls.FixedUpdate(this);
//        Debug.LogFormat("Current speed: {0}", rigidbody.velocity);
//    }

//    public Inventory GetInventory() => inventory;
    
//    public Rigidbody2D GetRigidbody() => rigidbody;

//    public void Damage(int dmg)
//    {
//        throw new System.NotImplementedException();
//    }

//    public void Kill()
//    {
//        throw new System.NotImplementedException();
//    }
//}

////public class Sword : MonoBehaviour
////{
////    private void OnCollisionEnter2D(Collision2D collision)
////    {
////        collision.gameObject.GetComponent<IDamageable>().Damage(10);
////    }
////}
