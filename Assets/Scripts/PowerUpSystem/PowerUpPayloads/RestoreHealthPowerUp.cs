using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthPowerUp : PowerUp
{
    private const int HealthRestored = 3;

    private void Start()
    {
        base.powerUpPayload += RestoreHealth;
    }

    public void RestoreHealth(Player player)
    {
        if (player.curHealth + HealthRestored >= player.maxHealth) 
        { 
            player.curHealth = player.maxHealth;
        }
        else
        {
        player.curHealth += HealthRestored;
        }
        UIHealth.instance.UpdateHealth();
    }
}
 