using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : 
    MonoBehaviour,
    IKillable,
    IDamageable,
    IPhysical,
    IPickupper
{
    [SerializeField]
    PlayerHealth health;

    [SerializeField]
    PlayerMovement movement;

    PlayerInventory inventory;
    PlayerController controls;

    new Rigidbody2D rigidbody;

    class PlayerHealth : Health
    {
        public PlayerHealth(Player player)
        {
            //
        }
    }

    class PlayerMovement : Movement
    {
        public PlayerMovement(Player player)
        {

        }

        public override Rigidbody2D GetRigidbody() => player.GetRigidbody();
    }

    class PlayerInventory : Inventory
    {
        public PlayerInventory(Player player)
        {

        }

        // impl
        void OpenInventory()
        {

        }
    }

    class PlayerController : Controller
    {
        public PlayerController(Player player)
        {
            var onFoot = actions.NinjaOnFoot;
            onFoot.Jump.performed += Jump_performed;
            onFoot.Move.performed += Move_performed;
        }

        private void Move_performed(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }

        private void Jump_performed(InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        health = new(this);
        movement = new(this);
        controls = new(this);
        inventory = new(this);
    }

    public Inventory GetInventory() => inventory;
    
    public Rigidbody2D GetRigidbody() => rigidbody;

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
