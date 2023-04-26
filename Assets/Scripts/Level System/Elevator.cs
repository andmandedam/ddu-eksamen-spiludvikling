using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    public bool isBottom = false;
    public bool isTop = false;

    private const int ElevatorHeight = 4;
    private Transform playerTransform = null;
    private PlayerInputActions playerController = null;
    private bool playerInElevator = false;

    //private void MoveDown(InputAction.CallbackContext ctx)
    //{
    //    if (isBottom || !playerInElevator) return;
    //    playerTransform.Translate(new Vector3(0, -ElevatorHeight, 0));
    //    AudioManager.instance.PlaySound("ElevatorArrival");
    //}
    //private void MoveUp(InputAction.CallbackContext ctx)
    //{
    //    if (isTop || !playerInElevator) return;
    //    playerTransform.Translate(new Vector3(0, ElevatorHeight, 0));
    //    AudioManager.instance.PlaySound("ElevatorArrival");
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!(collision.gameObject.layer == LayerMask.NameToLayer("Player")))
    //    {
    //        return;
    //    }

    //    playerInElevator = true;
    //    Debug.Log("Player enter elevator: " + name);

    //    if (playerTransform == null)
    //    {
    //        playerTransform = collision.transform;
    //    }
        
    //    if (playerController == null) 
    //    {
    //        playerController = collision.gameObject.GetComponent<Player>().controls.actions;
    //        playerController.NinjaOnFoot.Passthrough.performed += MoveDown;
    //        playerController.NinjaOnFoot.LookUp.performed += MoveUp;
    //    }
    //}    
    
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
    //        playerInElevator = false;
    //}

    //private void OnDestroy()
    //{
    //    if (playerController == null) return;
    //    playerController.NinjaOnFoot.Passthrough.performed -= MoveDown;
    //    playerController.NinjaOnFoot.LookUp.performed -= MoveUp;
    //}
}
