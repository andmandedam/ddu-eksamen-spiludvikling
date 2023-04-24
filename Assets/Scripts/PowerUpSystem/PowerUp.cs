using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 
public class PowerUp : MonoBehaviour
{
    public bool isPermanent = false;
    public Sprite inventorySprite;
    public new string name;

    protected delegate void PowerUpPayload(Player player);
    protected PowerUpPayload powerUpPayload;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            return;
        }
        ActivatePayload(collision.GetComponent<Player>(), powerUpPayload);
    }

    private void ActivatePayload(Player player, PowerUpPayload powerUpPayload)
    {
        if (isPermanent)
        {
            InventoryUI.instance.AddPowerUpToInventory(inventorySprite);
        }

        powerUpPayload(player);

        Destroy(gameObject);
    }
}
