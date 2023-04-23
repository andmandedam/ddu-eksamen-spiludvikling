using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class PowerUp : MonoBehaviour
{
    public bool isPermanent = false;

    protected delegate void PowerUpPayload(Player player);
    protected PowerUpPayload powerUpPayload;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collison");
        if (!collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Debug.Log("Collison with not player");
            return;
        }
        Debug.Log("Collison with player");
        if (isPermanent)
        {
            AddPowerUpToInventory();
        }

        ActivatePayload(collision.GetComponent<Player>(), powerUpPayload);
        Destroy(gameObject);
    }

    private void ActivatePayload(Player player, PowerUpPayload powerUpPayload)
    {
        powerUpPayload(player);
    }

    private void AddPowerUpToInventory()
    {
        throw new NotImplementedException();
    }
}
