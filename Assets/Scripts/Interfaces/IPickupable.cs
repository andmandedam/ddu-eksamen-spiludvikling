using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 
public class Spand : Item
{
    public int spandID;

    public Spand(int id)
    {
        this.spandID = id;
    }
}

public class SpandFabrik : IPickupable
{
    public Item OnPickup()
    {
        return new Spand(0);
    }
}

public class SpandCrafting : IPickupable
{
    public Item OnPickup()
    {
        return new Spand(1);
    }
}

*/

//public class Chest : IPickupable
//{
//    public Queue<Item> contents;

//    public Item OnPickup(IPickupper pickupper)
//    {
//        return contents.Dequeue();
//    }
//}

public interface IPickupable
{
    public Item OnPickup(IPickupper pickupper);
}





