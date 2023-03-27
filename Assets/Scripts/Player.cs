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
    class PlayerInventory : Inventory
    {
        // impl
        void OpenInventory()
        {

        }
    }

    class PlayerController : Controller
    {
        public PlayerController(PlayerInputActions actions)
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

    struct PlayerHealth
    {
        int max;
        int current;
    }

    struct PlayerMovement
    {
        int speed;
        int jumpHeigth;
    }

    PlayerHealth health;
    PlayerMovement movement;

    PlayerInventory inventory;
    PlayerController controls;

    void Start()
    {
        PlayerInputActions actions = new PlayerInputActions();
        actions.Enable();
        controls = new PlayerController(actions);
    }


    public Inventory GetInventory() => inventory;

    public Rigidbody2D GetRigidbody()
    {
        throw new System.NotImplementedException();
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
