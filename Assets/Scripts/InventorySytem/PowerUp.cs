using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpPayload
{

}

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp", order = 1)]
public class PowerUp : ScriptableObject
{
    public new string name;
    public int ID;
    public Sprite sprite;

    public Player affectedEntity;
    public PowerUpPayload payload;

}
