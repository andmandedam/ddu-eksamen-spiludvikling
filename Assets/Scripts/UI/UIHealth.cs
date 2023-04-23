using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealth : MonoBehaviour
{
    public static UIHealth instance;

    [SerializeField]
    private Entity player;

    private UIHeart[] uIHearts;
    private void Start()
    {
        uIHearts = GetComponentsInChildren<UIHeart>();
        UpdateHealth();
        instance = this;
    }

    public void UpdateHealth()
    {        
        int heartIndexFromCurHealth = player.curHealth / 2 + (player.curHealth % 2) - 1; // Intended integer division    
        int heartIndexFromMaxHealth = player.maxHealth / 2 + (player.maxHealth % 2) - 1; // Intended integer division
        
        for (int i = 0; i < uIHearts.Length; i++)
        {
            if (i < heartIndexFromCurHealth)
            {
                uIHearts[i].SetHeartState((int) UIHeart.UIHeartState.Full);
                continue;
            }
            if (heartIndexFromCurHealth == i)
            {
                uIHearts[i].SetHeartState(2 - (player.curHealth % 2));
                continue;
            }
            if (i > heartIndexFromCurHealth && i <=heartIndexFromMaxHealth)
            {
                uIHearts[i].SetHeartState((int) UIHeart.UIHeartState.Empty);
            }
            if (i > heartIndexFromMaxHealth)
            {
                uIHearts[i].SetHeartState((int)UIHeart.UIHeartState.Disabled);
            }
        }
    }
}
