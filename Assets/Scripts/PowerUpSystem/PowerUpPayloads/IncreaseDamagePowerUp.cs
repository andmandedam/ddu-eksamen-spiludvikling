using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamagePowerUp : PowerUp
{
    private const int DamageIncrease = 10;
    

    private void Start()
    {
        base.powerUpPayload += IncreaseDamage;
    }

    public void IncreaseDamage(Player player)
    {
        if (player == null) return;

        player.attack.IncreaseDamage(DamageIncrease);
    }
}
