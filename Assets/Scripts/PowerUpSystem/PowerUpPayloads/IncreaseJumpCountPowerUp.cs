using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseJumpCountPowerUp : PowerUp
{
    private const int jumpCountIncrease = 1;
    

    private void Start()
    {
        base.powerUpPayload += IncreaseJumpCount;
    }

    public void IncreaseJumpCount(Player player)
    {
        if (player == null) return;
        
        player._jump.jumpCount += jumpCountIncrease;
    }
}
