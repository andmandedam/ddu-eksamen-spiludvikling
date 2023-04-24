using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    public List<Sprite> powerUpSprites;
    public HUDPowerUp[] HUDPowerUps;
    private int amountOfPowerUpsDisplayed;

    private void Start()
    {
        instance = this;
        HUDPowerUps = GetComponentsInChildren<HUDPowerUp>();
        amountOfPowerUpsDisplayed = HUDPowerUps.Length;
    }

    public void AddPowerUpToInventory(Sprite powerUpSprite)
    {
        powerUpSprites.Reverse();
        powerUpSprites.Add(powerUpSprite);
        powerUpSprites.Reverse();
        if (powerUpSprites.Count > amountOfPowerUpsDisplayed) 
        { 
            powerUpSprites.RemoveAt(amountOfPowerUpsDisplayed); 
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < powerUpSprites.Count; i++)
        {
            HUDPowerUps[i].SetPowerUpSprite(powerUpSprites[i]);
        }
    }

    public void DisplayHUDText(string text)
    {
        throw new NotImplementedException();
    }
}
