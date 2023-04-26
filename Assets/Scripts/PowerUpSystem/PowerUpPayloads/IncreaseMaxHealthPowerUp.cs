using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxHealthPowerUp : PowerUp
{
    private const int maxHealthIncrease = 4;
    private const int healthRestored = 2;
    

    private void Start()
    {
        base.powerUpPayload += IncreaseMaxHealth;
    }

    public void IncreaseMaxHealth(Player player)
    {
        if (player == null) return;
        
        player.maxHealth += maxHealthIncrease;
        player.curHealth += healthRestored;
        UIHealth.instance.UpdateHealth();
    }
}
